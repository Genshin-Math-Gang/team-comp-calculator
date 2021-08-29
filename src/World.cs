using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Buffs;
using Tcc.Events;
using Tcc.Units;
using Tcc.Stats;
using Tcc.Elements;

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

        public event EventHandler<(Unit from, Unit to, Timestamp timestamp)> unitSwapped;

        public World(List<Enemy.Enemy> enemies)
        {
            this.characterEvents = new List<CharacterEvent>();
            this.TotalDamage = new double[4];
            this.enemies = enemies;
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

        public void AddCharacterEvent(Timestamp timestamp, Func<Timestamp, List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction));
        }

        public void SwitchUnit(Timestamp timestamp, Units.Unit unit)
        {
            unitSwapped?.Invoke(this, (onFieldUnit, unit, timestamp));
            this.onFieldUnit = unit;
            Console.WriteLine($"Switched to {unit} at {timestamp}");
        }

        public void Hit(Timestamp timestamp, Elements.Element element, int mvIndex, Stats.Stats stats, Units.Unit unit, Types type, double reaction = Reaction.NONE, bool isHeavy = false, bool applyElement = true, bool isAoe = true, int bounces = 1, string description = "")
        {
            double result = Reaction.NONE;
            double final_damage = 0;
            for (int i = 1; i <= bounces; i++)
            {
                if (isAoe)
                {
                foreach (Enemy.Enemy enemy in enemies)
                    {
                        if (applyElement)
                        {
                            result = enemy.gauge.ElementApplied(timestamp, element, unit, type, isHeavy);
                        }
                        final_damage += enemy.takeDamage(timestamp, element, type, stats, unit, mvIndex, reaction, isHeavy) * (result > 0 ? result : 1);
                    }
                }
                else
                {
                foreach (Enemy.Enemy enemy in enemies)
                    {
                        if (i > bounces) break;
                        if (applyElement)
                        {
                            result = enemy.gauge.ElementApplied(timestamp, element, unit, type, isHeavy);
                        }
                        final_damage += enemy.takeDamage(timestamp, element, type, stats, unit, mvIndex, reaction, isHeavy) * (result > 0 ? result : 1);
                        i++;
                    }
                }
            }

            this.TotalDamage[units.IndexOf(unit)] += final_damage;
            if (description == "")
                Console.WriteLine($"Damage dealt at {timestamp} is {final_damage}");
            else
                Console.WriteLine($"Damage dealt by {description} at {timestamp} is {final_damage}");

            switch (result)
            {
                case Reaction.OVERLOADED : {AddWorldEvents(new Overload(timestamp, stats, unit)); break;}
            }
        }

        public void AddBuff(Timestamp timestamp, Units.Unit unit, BuffFromUnit buff, string description)
        {
            unit.AddBuff(buff);

            if (description == "") Console.WriteLine($"Buff added to {unit} at {timestamp}");
            else Console.WriteLine($"Buff added by {description} to {unit} at {timestamp}");
        }

        public void AddBuffOnField(Timestamp timestamp, BuffFromUnit buff, string description)
        {
            this.onFieldUnit.AddBuff(buff);

            if (description == "") Console.WriteLine($"Buff added to {this.onFieldUnit} at {timestamp}");
            else Console.WriteLine($"Buff added by {description} to {this.onFieldUnit} at {timestamp}");
        }

        public void AddBuffGlobal(Timestamp timestamp, BuffFromUnit buff, string description)
        {
            foreach(Unit x in units)
            {
                if (x != null) x.AddBuff(buff);
            }

            if (description == "") Console.WriteLine($"Buff added at {timestamp}");
            else Console.WriteLine($"Buff added by {description} at {timestamp}");
        }

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