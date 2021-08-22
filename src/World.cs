using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Units;

namespace Tcc
{
    public class World
    {
        Unit onFieldUnit;
        List<Unit> units;
        List<CharacterEvent> characterEvents;
        List<WorldEvent> queuedWorldEvents;
        List<Enemy.Enemy> enemies;
        public double[] TotalDamage;

        public event EventHandler<(Timestamp timestamp, Unit attacker, Element element, Stats.Types attackType)> enemyHitHook; // TODO Not fired by anything

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

        public ReadOnlyCollection<Unit> GetUnits() => units.AsReadOnly();

        public void AddEnemy(Enemy.Enemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public void AddCharacterEvent(Timestamp timestamp, Func<Timestamp, List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction));
        }

        public void SwitchUnit(Timestamp timestamp, Units.Unit unit)
        {
            this.onFieldUnit = unit;
            Console.WriteLine($"Switched to {unit} at {timestamp}");
        }

        public void Hit(Timestamp timestamp, double damage, Units.Unit unit, string description)
        {
            this.TotalDamage[units.IndexOf(unit)] += damage;
            if (description == "")
                Console.WriteLine($"Damage dealt at {timestamp} is {damage}");
            else
                Console.WriteLine($"Damage dealt by {description} at {timestamp} is {damage}");
        }

        // public void Snapshot(Timestamp timestamp, Unit unit, Types type, string description)
        // {
        //     unit.Snapshot(type);
        //     if (description == "")
        //         Console.WriteLine($"{unit} snapshotted at {timestamp}");
        //     else
        //         Console.WriteLine($"{unit} snapshotted {description} at {timestamp}");
        // }

        // public void UnSnapshot(Timestamp timestamp, Unit unit, Types type, string description)
        // {
        //     unit.UnSnapshot(type);
        //     if (description == "")
        //         Console.WriteLine($"{unit} un-snapshotted at {timestamp}");
        //     else
        //         Console.WriteLine($"{unit} un-snapshotted {description} at {timestamp}");
        // }

        public void AddBuff(Timestamp timestamp, Units.Unit unit, UnconditionalBuff buff, string description)
        {
            unit.AddBuff(buff);

            if (description == "") Console.WriteLine($"Buff added to {unit} at {timestamp}");
            else Console.WriteLine($"Buff added by {description} to {unit} at {timestamp}");
        }

        public void AddBuffOnField(Timestamp timestamp, UnconditionalBuff buff, string description)
        {
            this.onFieldUnit.AddBuff(buff);

            if (description == "") Console.WriteLine($"Buff added to {this.onFieldUnit} at {timestamp}");
            else Console.WriteLine($"Buff added by {description} to {this.onFieldUnit} at {timestamp}");
        }

        public void AddBuffGlobal(Timestamp timestamp, UnconditionalBuff buff, string description)
        {
            foreach(Unit x in units)
            {
                if (x != null) x.AddBuff(buff);
            }

            if (description == "") Console.WriteLine($"Buff added at {timestamp}");
            else Console.WriteLine($"Buff added by {description} at {timestamp}");
        }

        // public void RemoveBuff(Timestamp timestamp, Units.Unit unit, string name, Types type, string description)
        // {
        //     unit.RemoveBuff(name, type);
        //     if (description == "")
        //         Console.WriteLine($"Buff expired to {unit} at {timestamp}");
        //     else
        //         Console.WriteLine($"Buff expired by {description} to {unit} at {timestamp}");
        // }

        // public void RemoveBuffGlobal(Timestamp timestamp, string name, Types type, string description)
        // {
        //     foreach(Unit x in units)
        //     {
        //         if (x != null)
        //             x.RemoveBuff(name, type);
        //     }
        //     if (description == "")
        //         Console.WriteLine($"Buff expired at {timestamp}");
        //     else
        //         Console.WriteLine($"Buff expired by {description} at {timestamp}");
            
        // }

        public void Simulate()
        {
            queuedWorldEvents = characterEvents
                .SelectMany((characterEvent) => characterEvent.GetWorldEvents())
                .OrderBy((worldEvent) => worldEvent.Timestamp)
                .ToList();

            while(queuedWorldEvents.Any())
            {
                var nextEvent = queuedWorldEvents[0];

                queuedWorldEvents.RemoveAt(0);
                nextEvent.Apply(this);
            }
        }

        public void AddWorldEvents(params WorldEvent[] events)
        {
            queuedWorldEvents.AddRange(events);

            queuedWorldEvents = queuedWorldEvents
                .OrderBy((worldEvent) => worldEvent.Timestamp)
                .ToList();
        }
    }
}