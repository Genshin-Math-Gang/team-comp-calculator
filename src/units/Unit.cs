using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Tcc.elements;
using Tcc.enemy;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public abstract class Unit: StatObject
    {
        private static Dictionary<String, Stats> fileConvert = new Dictionary<string, Stats>
        {
            {"Energy Recharge", Stats.EnergyRecharge},
            {"EM", Stats.ElementalMastery},
            {"ATK", Stats.AtkPercent},
            {"DEF", Stats.DefPercent},
            {"HP", Stats.HpPercent},
            {"Healing Bonus", Stats.HealingBonus},
            {"Anemo DMG Bonus", Stats.AnemoDamageBonus},
            {"Geo DMG Bonus", Stats.GeoDamageBonus},
            {"Pyro DMG Bonus", Stats.PyroDamageBonus},
            {"Hydro DMG Bonus", Stats.HydroDamageBonus},
            {"Electro DMG Bonus", Stats.ElectroDamageBonus},
            {"Cryo DMG Bonus", Stats.CryoDamageBonus},
            {"Dendro DMG Bonus", Stats.DendroDamageBonus}, // :kleek:
            {"Physical DMG Bonus", Stats.PhysicalDamageBonus}
            
        };

        protected readonly int ConstellationLevel;
        protected readonly int BurstEnergyCost;
        public readonly WeaponType WeaponType;
        public readonly Element Element;
        public readonly string Name;
        
        // assume 60 fps
        protected int[] AutoAttackFrameData;
        protected int[] AutoAttackHits = new[] {1, 1, 1, 1, 1, 1, 1};

        // Base stats
        public readonly Dictionary<Types, AbilityStats> StartingAbilityStats = new();
        
        // Artifact stats
        public ArtifactStats ArtifactStats;
        
        // Default ability ICDs
        protected ICDCreator NormalICD, ChargedICD, SkillICD, BurstICD;

        // kinda hack to make normal attack stuff work
        protected bool HasHeavyAttacks;
        
        /*
        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> CapacityBuffs = new();
        protected readonly List<Buff<FirstPassModifier>> FirstPassBuffs = new();
        protected readonly List<Buff<SecondPassModifier>> SecondPassBuffs = new();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> EnemyBasedBuffs = new();
        protected readonly Dictionary<Element, List<Buff<ElementBasedModifier>>> ElementBasedBuffs = new();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> AbilityBuffs = new();*/

        // Hooks
        public event EventHandler<DealDamageArgs> DealDamageHook; 
        
        public event EventHandler<double> SkillActivatedHook;
        public event EventHandler<double> BurstActivatedHook;
        public event EventHandler<(double timestamp, Reaction reaction, World world)> TriggeredReactionHook;
        public event EventHandler<(double timestamp, Element? element)> ParticleCollectedHook; // TODO Not fired by anything

        public event EventHandler<(double timestamp, Reaction reaction, Enemy enemy)> SwirlTriggeredHook;

        public event EventHandler<NormalAttackArgs> NormalAttackHook;

        public event EventHandler<double> EnemyDeathHook;

        // TODO: probably make args class for all of these and  move them somewhere
        public class NormalAttackArgs: EventArgs
        {
            public double Timestamp { get;}
            public double Duration { get;}
            
            public World World { get; }

            public NormalAttackArgs(double timestamp, double duration, World world)
            {
                Timestamp = timestamp;
                Duration = duration;
                World = world;
            }
        }
        
        public class DealDamageArgs: EventArgs
        {
            public double Timestamp { get;}
            public Unit Unit { get;}
            
            public World World { get; }

            public DealDamageArgs(double timestamp, Unit unit, World world)
            {
                Timestamp = timestamp;
                Unit = unit;
                World = world;
            }
        }
        
        // I want to make something to represent level at some point but I couldn't get a lazy enum to work so i will
        // work on that later
        protected Unit(
            string name, String level, int constellationLevel, int autoLevel, int skillLevel, int burstLevel, Element element, 
            WeaponType weaponType) : base(level)
        {
            ConstellationLevel = constellationLevel;
            Element = element;
            WeaponType = weaponType;
            // jank
            TextInfo ti = new CultureInfo("en-US").TextInfo;
            Name = ti.ToTitleCase(name);
            
            // not initializing other icd here could bite me later but if that happens hopefully i remember this
            // if that happens you are a dumbass
            NormalICD = new ICDCreator();

            // dumb file directory hack but i don't know a better solution
            string dir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            
            using (var sr = new StreamReader(dir + "/src/units/characterData/" + name + "MVS.json"))
            {
                String content = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Dictionary<string, double[][]>>(content);
                Debug.Assert(data != null, "data loaded from file got fucked");
                StartingAbilityStats[Types.NORMAL] = new AbilityStats(motionValues: data["normal"][autoLevel]);
                StartingAbilityStats[Types.CHARGED] = new AbilityStats(motionValues:data["charged"][autoLevel]);
                StartingAbilityStats[Types.PLUNGE] = new AbilityStats(motionValues: data["plunge"][autoLevel]);
                StartingAbilityStats[Types.BURST] = new AbilityStats(motionValues: data["burst"][burstLevel]);
                StartingAbilityStats[Types.SKILL] = new AbilityStats(motionValues: data["skill"][skillLevel]);
            }
            
            using (var sr = new StreamReader(dir + "/src/units/characterData/" + name + "Stats.json"))
            {
                String content = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<JSONSTats>(content);
                // needs some kind of validation or something here but am lazy
                Debug.Assert(data != null, "data loaded from file got fucked");
                String ascension = data.ascension;
                double[] stats = data.stats[level];
                BurstEnergyCost = data.energy;
                CurrentEnergy = BurstEnergyCost;
                StartingStatsPage = new StatsPage(new Dictionary<Stats, double>
                {
                    {Stats.HpBase, stats[0]},
                    {Stats.AtkBase, stats[1]},
                    {Stats.DefBase, stats[2]},
                    {Stats.CritRate, stats[4]},
                    {Stats.CritDamage, stats[5]}
                });
                StartingStatsPage += new StatsPage(fileConvert[ascension], stats[3]);
                ArtifactStats = new ArtifactStats();
            }
            
        }

        public virtual void Reset()
        {
            // need to implement this for all characters to make sure internal state gets updated
            DealDamageHook = null;
            BurstActivatedHook = null;
            SkillActivatedHook = null;
            EnemyDeathHook = null;
            NormalAttackHook = null;
            ParticleCollectedHook = null;
            SwirlTriggeredHook = null;
            TriggeredReactionHook = null;
            // make some max hp thing
            /*CurrentHp = MaxHp*/
            base.Reset();
        }

        public abstract List<WorldEvent> Skill(double timestamp, params object[] p);
        
        public abstract List<WorldEvent> Burst(double timestamp);


        // TODO: needs to be overwritten for bow and claymore characters who have weird CA
        public List<WorldEvent> AutoAttack(double timestamp, AutoString autoString)
        {
            int normalCount = 1 + (int) autoString / 2;
            int doCharged = (int) autoString % 2;
            // for now i am going to write a naive implementation and later worry about characters with  
            // multiple hits corresponding to 1 normal or charged attack, ie xiangling and childe
            List<WorldEvent> hits = new List<WorldEvent>();
            double start = timestamp;
            double duration;
            HitType hitType = new HitType(Element.PHYSICAL, false, heavy: HasHeavyAttacks);
            for (int i = 1; i < normalCount + 1; i++)
            {
                duration = (AutoAttackFrameData[i] - AutoAttackFrameData[i - 1]) / 60.0;
                start += AutoAttackFrameData[i-1] / 60.0;

                hits.Add(NormalAttackGeneralUsed(start, duration));
                // rn i'm assuming that the hit lands at the end of the duration which seems reasonable
                // but idk if that is true
                // TODO: add check if user has elemental infusion
                // TODO: apparently itto works weirdly with how his stats work so need to check that
                // TODO: when i get around to adding different levels of aoe this will need to be changed
                hits.Add(new Hit(timestamp + duration, i, GetStatsPage, this, Types.NORMAL, hitType, 
                    ToString() + " normal " + (i+1)));
            }

            if (doCharged == 1)
            {
                int i = AutoAttackFrameData.Length - 1;
                hits.Add(new Hit(timestamp + AutoAttackFrameData[i], i, GetStatsPage, this, 
                    Types.CHARGED, new HitType(Element.PHYSICAL, false, heavy: HasHeavyAttacks), ToString() + "charged attack"));
            }

            return hits;
        }

        public Weapon Weapon { get; set; }
        
        
        /*public double GetAbilityGauge(Types type)
        {
            // this will probably need to be modified later to make it work with swirl 
            if (type == Types.TRANSFORMATIVE)
            {

                return 0;
            }
            //return startingAbilityStats[type];
            return 1;
        }*/

        public List<WorldEvent> SwitchUnit(double timestamp) 
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }
        
        public AbilityStats GetAbilityStats(SecondPassStatsPage statsFromUnit, Types type, Element element, Enemy enemy, double timestamp)
        {
            EnemyBasedBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            foreach (var list in AbilityBuffs.Values) list.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            
            AbilityStats result = statsFromUnit + ArtifactStats.Stats;

            if (StartingAbilityStats.TryGetValue(type, out var startingStats)) result.Add(startingStats);

            if (enemy != null)
            {
                foreach (var buff in EnemyBasedBuffs) result.Add(buff.GetModifier((this, timestamp, enemy, statsFromUnit.firstPassStats)));
            }

            if (ElementBasedBuffs.TryGetValue(element, out var elementBuffList))
            {
                foreach (var buff in elementBuffList) result.Add(buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats)));
            }

            if (AbilityBuffs.TryGetValue(type, out var abilityBuffList))
            {
                foreach (var buff in abilityBuffList) result.Add(buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats)));
            }

            return result;
        }
        

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, BurstEnergyCost);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(double timestamp)
        {
            return new WorldEvent(timestamp, (world) => SkillActivatedHook?.Invoke(this, timestamp),$"Skill activated by {this}");
        }

        protected WorldEvent NormalAttackUsed(double timestamp, double duration)
        {
            return new WorldEvent(timestamp, world => NormalAttackHook?.Invoke(this, new NormalAttackArgs(timestamp, duration, world)));
        }

        public WorldEvent ReactionTriggered(double timestamp, Reaction reaction)
        {
            return new WorldEvent(timestamp, world =>
            {
                foreach (var unit in world.GetUnits())
                {
                    unit?.TriggeredReactionHook?.Invoke(this, (timestamp, reaction, world));
                }
            });
        }
        
        
        protected WorldEvent NormalAttackGeneralUsed(double timestamp, double duration)
        {
            return new WorldEvent(timestamp, world =>
            {
                var args = new NormalAttackArgs(timestamp, duration, world);
                foreach (var unit in world.GetUnits())
                {
                    unit?.NormalAttackHook?.Invoke(this, args);
                }
            });
        }

        public WorldEvent DealtDamage(double timestamp, Unit unit)
        {
            return new WorldEvent(timestamp, world => DealDamageHook?.Invoke(this, 
                new DealDamageArgs(timestamp, unit, world)));
        }

        public WorldEvent EnemyDeath(double timestamp)
        {
            return new WorldEvent(timestamp, world => EnemyDeathHook?.Invoke(this, timestamp));
        }


        protected WorldEvent BurstActivated(double timestamp)
        {
            return new WorldEvent(timestamp, (world) => BurstActivatedHook?.Invoke(this, timestamp), $"Burst activated by {this}");
        }

        protected WorldEvent TriggeredReaction(double timestamp, Reaction reaction)
        {
            return new WorldEvent(timestamp, (world) => TriggeredReactionHook?.Invoke(this, (timestamp, reaction, world)));
        }

        protected WorldEvent ParticleCollected(double timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => ParticleCollectedHook?.Invoke(this, (timestamp, element)));
        }

        public WorldEvent TriggeredSwirl(double timestamp, Reaction reaction, Enemy enemy)
        {
            // make triggered swirl also activate reaction hook
            if (!ReactionTypes.IsSwirl(reaction))
            {
                // throw some error
            }

            return new WorldEvent(timestamp, world =>
                {
                    SwirlTriggeredHook?.Invoke(this, (timestamp, reaction, enemy));
                    // TODO: idk what the fuck i am doing
                    foreach (var unit in world.GetUnits())
                    {
                        unit?.TriggeredReactionHook?.Invoke(this, (timestamp, reaction, world));
                    }
                    
                },
                $"{this} triggered swirl at {timestamp}", 1);
        }

        //public abstract Dictionary<string, Delegate> GetCharacterEvents();

        public override string ToString()
        {
            return this.Name;
        }

        public static Unit UnitCreator(UnitCreator u)
        {
            // this is terrible code but i cant find a way to programmatically ensure all derived classes implement
            // a constructor with the same arguments
            return u.Character switch
            {
                Character.Bennett => new Bennett(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Ganyu => new Ganyu(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Raiden => new Raiden(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Sucrose => new Sucrose(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Xiangling => new Xiangling(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Xingqiu => new Xingqiu(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Fischl => new Fischl(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                Character.Beidou => new Beidou(u.Cons, u.Level, u.AutoLevel, u.SkillLevel, u.BurstLevel),
                _ => throw new ArgumentOutOfRangeException(nameof(u), "terrible unit constructor bullshit")
            };
        }
    }
}