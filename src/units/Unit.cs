using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;
using Newtonsoft.Json;

namespace Tcc.Units
{
    public abstract class Unit: StatObject
    {
        
        protected readonly int constellationLevel;
        protected readonly int burstEnergyCost;
        public readonly WeaponType weaponType;
        public readonly Element element;

        // Base stats
        protected readonly CapacityStats startingCapacityStats;
        protected readonly GeneralStats startingGeneralStats;
        protected readonly Dictionary<Types, AbilityStats> startingAbilityStats = new();

        // Snapshottable buffs
        protected readonly List<Buff<CapacityModifier>> capacityBuffs = new();
        protected readonly List<Buff<FirstPassModifier>> firstPassBuffs = new();
        protected readonly List<Buff<SecondPassModifier>> secondPassBuffs = new();

        // Unsnapshottable buffs
        protected readonly List<Buff<EnemyBasedModifier>> enemyBasedBuffs = new();
        protected readonly Dictionary<Element, List<Buff<ElementBasedModifier>>> elementBasedBuffs = new();
        protected readonly Dictionary<Types, List<Buff<AbilityModifier>>> abilityBuffs = new();

        // Hooks
        public event EventHandler<Timestamp> skillActivatedHook;
        public event EventHandler<Timestamp> burstActivatedHook;
        public event EventHandler<(Timestamp timestamp, int reaction)> triggeredReactionHook; // TODO Not fired by anything
        public event EventHandler<(Timestamp timestamp, Element? element)> particleCollectedHook; // TODO Not fired by anything

        public event EventHandler<(Timestamp timestamp, Reaction reaction, Enemy.Enemy enemy)> swirlTriggeredHook;

        public event EventHandler<NormalAttackArgs> normalAttackHook;
        
        
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
            WeaponType weaponType) 
        {
            this.constellationLevel = constellationLevel;
            this.element = element;
            this.weaponType = weaponType;
            // dumb file directory hack but i don't know a better solution
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            
            using (var sr = new StreamReader(dir + "/src/units/characterData/" + name + "MVS.json"))
            {
                String content = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Dictionary<string, double[][]>>(content);
                startingAbilityStats[Types.NORMAL] = new AbilityStats(motionValues: data["normal"][autoLevel]);
                startingAbilityStats[Types.CHARGED] = new AbilityStats(motionValues:data["charged"][autoLevel]);
                startingAbilityStats[Types.PLUNGE] = new AbilityStats(motionValues: data["plunge"][autoLevel]);
                startingAbilityStats[Types.BURST] = new AbilityStats(motionValues: data["burst"][burstLevel]);
                startingAbilityStats[Types.SKILL] = new AbilityStats(motionValues: data["skill"][skillLevel]);
            }
            
            using (var sr = new StreamReader(dir + "/src/units/characterData/" + name + "Stats.json"))
            {
                String content = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<JSONSTats>(content);
                // needs some kind of validation or something here but am lazy
                String ascension = data.ascencion;
                double[] stats = data.stats[level];
                burstEnergyCost = data.energy;
                CurrentEnergy = burstEnergyCost;
                startingGeneralStats = new GeneralStats(baseAttack: stats[1], baseDefence: stats[2],
                    critRate: stats[4], critDamage: stats[5]);
                startingCapacityStats = new CapacityStats(baseHp: stats[0], energy: burstEnergyCost);
                
                // shit code
                switch (ascension)
                {
                    case "Energy Recharge":
                        startingGeneralStats += new GeneralStats(energyRecharge: stats[3]);
                        break;
                    case "ATK":
                        startingGeneralStats += new GeneralStats(attackPercent: stats[3]);
                        break;
                    case "DEF":
                        startingGeneralStats += new GeneralStats(defencePercent: stats[3]);
                        break;
                    case "HP":
                        startingCapacityStats += new CapacityStats(hpPercent: stats[3]);
                        break;
                    case "Healing Bonus":
                        startingGeneralStats += new GeneralStats(healingBonus: stats[3]);
                        break;
                    case "Anemo DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.ANEMO, stats[3]));
                        break;
                    case "Cryo DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.ANEMO, stats[3]));
                        break;
                    case "Dendro DMG Bonus":
                        // :kleek:
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.DENDRO, stats[3]));
                        break;
                    case "Electro DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.ELECTRO, stats[3]));
                        break;
                    case "Geo DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.GEO, stats[3]));
                        break;
                    case "Hydro DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.HYDRO, stats[3]));
                        break;
                    case "Physical DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.PHYSICAL, stats[3]));
                        break;
                    case "Pyro DMG Bonus":
                        startingGeneralStats += new GeneralStats(elementalBonus: 
                            new KeyedPercentBonus<Element>(Element.PYRO, stats[3]));
                        break;
                }
            }
            base.startingCapacityStats = startingCapacityStats;
            base.startingGeneralStats = startingGeneralStats;
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
        
        public AbilityStats GetAbilityStats(SecondPassStatsPage statsFromUnit, Types type, Element element, Enemy.Enemy enemy, Timestamp timestamp)
        {
            enemyBasedBuffs.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            foreach (var list in abilityBuffs.Values) list.RemoveAll((buff) => buff.ShouldRemove(timestamp));
            
            AbilityStats result = statsFromUnit.generalStats;

            if (startingAbilityStats.TryGetValue(type, out var startingStats)) result += startingStats;

            if (enemy != null)
            {
                foreach (var buff in enemyBasedBuffs) result += buff.GetModifier((this, timestamp, enemy, statsFromUnit.firstPassStats));
            }

            if (elementBasedBuffs.TryGetValue(element, out var elementBuffList))
            {
                foreach (var buff in elementBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            if (abilityBuffs.TryGetValue(type, out var abilityBuffList))
            {
                foreach (var buff in abilityBuffList) result += buff.GetModifier((this, timestamp, statsFromUnit.firstPassStats));
            }

            return result;
        }
        

        public void GiveEnergy(int energy) => CurrentEnergy = Math.Min(CurrentEnergy + energy, CapacityStats.Energy);
        public void LoseEnergy(int energy) => CurrentEnergy = Math.Max(CurrentEnergy - energy, 0);

        protected WorldEvent SkillActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => skillActivatedHook?.Invoke(this, timestamp),$"Skill activated by {this}");
        }

        protected WorldEvent NormalAttackUsed(Timestamp timestamp, Timestamp duration)
        {
            return new WorldEvent(timestamp, world => normalAttackHook?.Invoke(this, new NormalAttackArgs(timestamp, duration, world)));
        }
        
        protected WorldEvent NormalAttackGeneralUsed(Timestamp timestamp, Timestamp duration)
        {
            return new WorldEvent(timestamp, world => {
                foreach (var unit in world.GetUnits())
                {
                    unit?.normalAttackHook?.Invoke(this, new NormalAttackArgs(timestamp, duration, world));
                }
            });
        }


        protected WorldEvent BurstActivated(Timestamp timestamp)
        {
            return new WorldEvent(timestamp, (world) => burstActivatedHook?.Invoke(this, timestamp), $"Burst activated by {this}");
        }

        protected WorldEvent TriggeredReaction(Timestamp timestamp, int reaction)
        {
            return new WorldEvent(timestamp, (world) => triggeredReactionHook?.Invoke(this, (timestamp, reaction)));
        }

        protected WorldEvent ParticleCollected(Timestamp timestamp, Element? element)
        {
            return new WorldEvent(timestamp, (world) => particleCollectedHook?.Invoke(this, (timestamp, element)));
        }

        public WorldEvent TriggeredSwirl(Timestamp timestamp, Reaction reaction, Enemy.Enemy enemy)
        {
            if (!ReactionTypes.IsSwirl(reaction))
            {
                // throw some error
            }

            return new WorldEvent(timestamp, _ => swirlTriggeredHook?.Invoke(this, (timestamp, reaction, enemy)),
                $"{this} triggered swirl at {timestamp}", 1);
        }

        //public abstract Dictionary<string, Delegate> GetCharacterEvents();
    }
}