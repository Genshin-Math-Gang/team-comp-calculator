using System.Linq;
using System.Collections.Generic;
using Tcc.Stats;
using Tcc.Events;
using System;

namespace Tcc.Units
{
    public class Unit
    {
        protected int constellationLevel;

        private Stats.Stats stats;
        private Dictionary<Types, Stats.Stats> modifiers = new Dictionary<Types, Stats.Stats>(){
            {Types.NORMAL, new Stats.Stats()},
            {Types.CHARGED, new Stats.Stats()},
            {Types.PLUNGE, new Stats.Stats()},
            {Types.SKILL, new Stats.Stats()},
            {Types.BURST, new Stats.Stats()} 
        };

        private Dictionary<Types, Stats.Stats> snapshots = new Dictionary<Types, Stats.Stats>(){
            {Types.NORMAL, new Stats.Stats()},
            {Types.CHARGED, new Stats.Stats()},
            {Types.PLUNGE, new Stats.Stats()},
            {Types.SKILL, new Stats.Stats()},
            {Types.BURST, new Stats.Stats()} 
        };
        private Dictionary<Types, Dictionary<string, Stats.Stats>> buffs = new Dictionary<Types, Dictionary<string, Stats.Stats>>(){
            {Types.NORMAL, new Dictionary<string, Stats.Stats>()},
            {Types.CHARGED, new Dictionary<string, Stats.Stats>()},
            {Types.PLUNGE, new Dictionary<string, Stats.Stats>()},
            {Types.SKILL, new Dictionary<string, Stats.Stats>()},
            {Types.BURST, new Dictionary<string, Stats.Stats>()},
            {Types.EVERYTHING, new Dictionary<string, Stats.Stats>()}
        };

        protected Unit(int constellationLevel, Stats.Stats stats, Stats.Stats burst, Stats.Stats skill, Stats.Stats normal, Stats.Stats charged, Stats.Stats plunge)
        {
            this.constellationLevel = constellationLevel;

            this.stats = stats;

            this.modifiers[Types.NORMAL] = normal;
            this.modifiers[Types.BURST] = burst;
            this.modifiers[Types.CHARGED] = charged;
            this.modifiers[Types.PLUNGE] = plunge;
            this.modifiers[Types.SKILL] = skill;
        }

        public List<WorldEvent> switchUnit(double tiemstamp)
        {
            return new List<WorldEvent>{
                new SwitchUnit(tiemstamp, this)
            };
        }

        protected Stats.Stats getStats(Types type)
        {
            if (snapshots[type].BaseHP != 0)
            {
                return snapshots[type];
            }

            Stats.Stats requested = new Stats.Stats();

            foreach (KeyValuePair<string, Stats.Stats> x in buffs[Types.EVERYTHING])
                requested += modifiers[type] + stats + x.Value;

            foreach(KeyValuePair<string, Stats.Stats> x in buffs[type])
            {
                requested += x.Value;
            }

            return requested;
        }

        public void Snapshot(Types type)
        {
            snapshots[type] = getStats(type);
        }

        public void UnSnapshot(Types type)
        {
            snapshots[type] = new Stats.Stats();
        }

        public void AddBuff(string name, Stats.Stats buff, Types type)
        {
            if (!buffs[type].ContainsKey(name))
            {
               buffs[type].Add(name, buff);
            }
        }

        public void RemoveBuff(string name, Types type)
        {
            if (buffs[type].ContainsKey(name))
            {
                buffs[type].Remove(name);
            }
        }
    }
}