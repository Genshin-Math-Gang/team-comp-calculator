using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Events;
using Tcc.Units;
using Tcc.Stats;
using Tcc.Enemy;

namespace Tcc
{
    public class World
    {
        Unit onFieldUnit;
        List<Unit> units;
        List<CharacterEvent> characterEvents;
        List<Enemy.Enemy> enemies;
        public double[] TotalDamage;

        public World()
        {
            this.characterEvents = new List<CharacterEvent>();
            this.TotalDamage = new double[4];
            this.enemies = new List<Enemy.Enemy>();
        }

        public void SetUnits(Unit onFieldUnit, Unit unit2, Unit unit3, Unit unit4)
        {
            this.onFieldUnit = onFieldUnit;
            this.units = new List<Unit> { onFieldUnit, unit2, unit3, unit4 };
        }

        public void AddEnemy(Enemy.Enemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public void AddCharacterEvent(double timestamp, Func<double, List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction));
        }

        public void SwitchUnit(double timestamp, Units.Unit unit)
        {
            this.onFieldUnit = unit;
            Console.WriteLine($"Switched to {unit} at {timestamp}");
        }

        public void Hit(double timestamp, double damage, Units.Unit unit, string description)
        {
            this.TotalDamage[units.IndexOf(unit)] += damage;
            if (description == "")
                Console.WriteLine($"Damage dealt at {timestamp} is {damage}");
            else
                Console.WriteLine($"Damage dealt by {description} at {timestamp} is {damage}");
        }

        public void Snapshot(double timestamp, Unit unit, Types type, string description)
        {
            unit.Snapshot(type);
            if (description == "")
                Console.WriteLine($"{unit} snapshotted at {timestamp}");
            else
                Console.WriteLine($"{unit} snapshotted {description} at {timestamp}");
        }

        public void UnSnapshot(double timestamp, Unit unit, Types type, string description)
        {
            unit.UnSnapshot(type);
            if (description == "")
                Console.WriteLine($"{unit} un-snapshotted at {timestamp}");
            else
                Console.WriteLine($"{unit} un-snapshotted {description} at {timestamp}");
        }

        public void AddBuff(double timestamp, Stats.Stats stats, Units.Unit unit, string name, Types type, string description)
        {
            unit.AddBuff(name, stats, type);
            if (description == "")
                Console.WriteLine($"Buff added to {unit} at {timestamp}");
            else
                Console.WriteLine($"Buff added by {description} to {unit} at {timestamp}");
        }

        public void AddBuffOnField(double timestamp, Stats.Stats stats, string name, Types type, string description)
        {
            this.onFieldUnit.AddBuff(name, stats, type);
            if (description == "")
                Console.WriteLine($"Buff added to {this.onFieldUnit} at {timestamp}");
            else
                Console.WriteLine($"Buff added by {description} to {this.onFieldUnit} at {timestamp}");
        }

        public void RemoveBuff(double timestamp, Units.Unit unit, string name, Types type, string description)
        {
            unit.RemoveBuff(name, type);
            if (description == "")
                Console.WriteLine($"Buff expired to {unit} at {timestamp}");
            else
                Console.WriteLine($"Buff expired by {description} to {unit} at {timestamp}");
        }

        public void RemoveBuffGlobal(double timestamp, string name, Types type, string description)
        {
            foreach(Unit x in units)
            {
                if (x != null)
                    x.RemoveBuff(name, type);
            }
            if (description == "")
                Console.WriteLine($"Buff expired at {timestamp}");
            else
                Console.WriteLine($"Buff expired by {description} at {timestamp}");
            
        }

        public void AddBuffGlobal(double timestamp, Stats.Stats stats, string name, Types type, string description)
        {
            foreach(Unit x in units)
            {
                if (x != null)
                    x.AddBuff(name, stats, type);
            }
            if (description == "")
                Console.WriteLine($"Buff added at {timestamp}");
            else
                Console.WriteLine($"Buff added by {description} at {timestamp}");
        }

        public void Simulate()
        {
            List<WorldEvent> worldEvents = characterEvents
                .SelectMany((characterEvent) => characterEvent.GetWorldEvents())
                .OrderBy((worldEvent) => worldEvent.Timestamp)
                .ToList();

            foreach(var worldEvent in worldEvents) worldEvent.Apply(this);
        }
    }
}