using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        // assume 60 fps
        protected int[] AutoAttackFrameData;

        // Base stats
        protected readonly Dictionary<Types, AbilityStats> StartingAbilityStats = new();
        
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
        public event EventHandler<Timestamp> SkillActivatedHook;
        public event EventHandler<Timestamp> BurstActivatedHook;
        public event EventHandler<(Timestamp timestamp, int reaction)> TriggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> ParticleCollectedHook; // TODO Not fired by anything

        public event EventHandler<(Timestamp timestamp, Reaction reaction, Enemy enemy)> SwirlTriggeredHook;

        public event EventHandler<NormalAttackArgs> NormalAttackHook;

        public event EventHandler<Timestamp> EnemyDeathHook;
        
        // TODO: probably make args class for all of these and  move them somewhere
        public class NormalAttackArgs: EventArgs
        {
            public Timestamp Timestamp { get;}
            public Timestamp Duration { get;}
            
            public World World { get; }

            public NormalAttackArgs(Timestamp timestamp, Timestamp duration, World world)
            {
                Timestamp = timestamp;
                Duration = duration;
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

        // TODO: needs to be overwritten for bow and claymore characters who have weird CA
        public List<WorldEvent> AutoAttack(Timestamp timestamp, AutoString autoString)
        {
            int normalCount = 1 + (int) autoString / 2;
            int doCharged = (int) autoString % 2;
            // for now i am going to write a naive implementation and later worry about characters with  
            // multiple hits corresponding to 1 normal or charged attack, ie xiangling and childe
            List<WorldEvent> hits = new List<WorldEvent>();
            Timestamp start = timestamp;
            Timestamp duration;
            for (int i = 0; i < normalCount; i++)
            {
                start += AutoAttackFrameData[i] / 60.0;
                if (i == 0)
                {
                    duration = AutoAttackFrameData[i] / 60.0;
                }
                else
                {
                    duration = (AutoAttackFrameData[i] - AutoAttackFrameData[i - 1]) / 60.0;
                }

                hits.Add(NormalAttackGeneralUsed(start, duration));
                // rn i'm assuming that the hit lands at the end of the duration which seems reasonable
                // but idk if that is true
                // TODO: add check if user has elemental infusion
                // TODO: apparently itto works weirdly with how his stats work so need to check that
                // TODO: when i get around to adding different levels of aoe this will need to be changed
                hits.Add(new Hit(timestamp + duration, Element.PHYSICAL, i, GetStatsPage, this, 
                    Types.NORMAL, new HitType(false, heavy: HasHeavyAttacks), ToString() + " normal " + (i+1)));
            }

            if (doCharged == 1)
            {
                int i = AutoAttackFrameData.Length - 1;
                hits.Add(new Hit(timestamp + AutoAttackFrameData[i], Element.PHYSICAL, i, GetStatsPage, this, 
                    Types.CHARGED, new HitType(false, heavy: HasHeavyAttacks), ToString() + "charged attack"));
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

        public List<WorldEvent> SwitchUnit(Timestamp timestamp) 
        {
            return new List<WorldEvent> { new SwitchUnit(timestamp, this) };
        }
        
        public AbilityStats GetAbilityStats(SecondPassStatsPage statsFromUnit, Types type, Element element, Enemy enemy, Timestamp timestamp)
        {
            EnemyBasedBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            foreach (var list in AbilityBuffs.Values) list.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            
            AbilityStats result = statsFromUnit + ArtifactStats.Stats;

            if (StartingAbilityStats.TryGetValue(type, out var startingStats)) result += startingStats;

            if (enemy != null)
            {
                foreach (var buff in EnemyBasedBuffs) result += buff.GetModifier((this, timestamp, enemy, statsFromUnit.firstPassStats));
            }

            if (ElementBasedBuffs.TryGetValue(element, out var elementBuffList))
            {
                foreach (var buff in elementBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            if (AbilityBuffs.TryGetValue(type, out var abilityBuffList))
            {
                foreach (var buff in abilityBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            return result;
        }
        

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, BurstEnergyCost);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => SkillActivatedHook?.Invoke(this, timestamp),$"Skill activated by {this}");
        }

        protected WorldEvent NormalAttackUsed(Timestamp timestamp, Timestamp duration)
        {
            return new WorldEvent(timestamp, world => NormalAttackHook?.Invoke(this, new NormalAttackArgs(timestamp, duration, world)));
        }
        
        protected WorldEvent NormalAttackGeneralUsed(Timestamp timestamp, Timestamp duration)
        {
            return new WorldEvent(timestamp, world => {
                foreach (var unit in world.GetUnits())
                {
                    unit?.NormalAttackHook?.Invoke(this, new NormalAttackArgs(timestamp, duration, world));
                }
            });
        }

        public WorldEvent EnemyDeath(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, world => EnemyDeathHook?.Invoke(this, timestamp));
        }


        protected WorldEvent BurstActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => BurstActivatedHook?.Invoke(this, timestamp), $"Burst activated by {this}");
        }

        protected WorldEvent TriggeredReaction(Timestamp timestamp, int reaction)
        {
            return new WorldEvent(timestamp, (world) => TriggeredReactionHook?.Invoke(this, (timestamp, reaction)));
        }

        protected WorldEvent ParticleCollected(Timestamp timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => ParticleCollectedHook?.Invoke(this, (timestamp, element)));
        }

        public WorldEvent TriggeredSwirl(Timestamp timestamp, Reaction reaction, Enemy enemy)
        {
            if (!ReactionTypes.IsSwirl(reaction))
            {
                // throw some error
            }

            return new WorldEvent(timestamp, _ => SwirlTriggeredHook?.Invoke(this, (timestamp, reaction, enemy)),
                $"{this} triggered swirl at {timestamp}", 1);
        }

        //public abstract Dictionary<string, Delegate> GetCharacterEvents();
    }
}