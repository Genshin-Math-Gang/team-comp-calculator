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
        private Dictionary<Types, Stats.Stats> stats = new Dictionary<Types, Stats.Stats>(){
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
            {Types.BURST, new Dictionary<string, Stats.Stats>()} 
        };

        protected Unit(int constellationLevel, Stats.Stats burst, Stats.Stats skill, Stats.Stats normal, Stats.Stats charged, Stats.Stats plunge)
        {
            this.constellationLevel = constellationLevel;

            this.stats[Types.NORMAL] = normal;
            this.stats[Types.BURST] = burst;
            this.stats[Types.CHARGED] = charged;
            this.stats[Types.PLUNGE] = plunge;
            this.stats[Types.SKILL] = skill;
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

            Stats.Stats requested = stats[type];

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
            if (type == Types.EVERYTHING)
            {
                foreach(KeyValuePair<Types, Dictionary<string, Stats.Stats>> x in buffs)
                {
                    if (!x.Value.ContainsKey(name))
                        x.Value.Add(name, buff);
                }
                return;
            }

            if (!buffs[type].ContainsKey(name))
            {
               buffs[type].Add(name, buff);
            }
        }

        public void RemoveBuff(string name, Types type)
        {
            if (type == Types.EVERYTHING)
            {
                foreach(KeyValuePair<Types, Dictionary<string, Stats.Stats>> x in buffs)
                {
                    if (x.Value.ContainsKey(name))
                        x.Value.Remove(name);
                }
                return;
            }

            if (buffs[type].ContainsKey(name))
            {
                buffs[type].Remove(name);
            }
        }
    }
}