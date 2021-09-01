  
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Buffs;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc
{
    public class World
    {
        List<Unit> units;
        List<CharacterEvent> characterEvents;
        List<WorldEvent> queuedWorldEvents;
        List<Enemy.Enemy> enemies;
        public double[] TotalDamage;

        public event EventHandler<(Timestamp timestamp, Unit attacker, Element element, Stats.Types attackType)> enemyHitHook;
        public event EventHandler<(Unit from, Unit to, Timestamp timestamp)> unitSwapped;

        public World(List<Enemy.Enemy> enemies)
        {
            this.characterEvents = new List<CharacterEvent>();
            this.TotalDamage = new double[4];
            this.enemies = enemies;
        }

        public Unit OnFieldUnit { get; private set; }

        public void SetUnits(Unit onFieldUnit, Unit unit2, Unit unit3, Unit unit4)
        {
            this.OnFieldUnit = onFieldUnit;
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

        public void SwitchUnit(Timestamp timestamp, Unit unit)
        {
            unitSwapped?.Invoke(this, (OnFieldUnit, unit, timestamp));
            OnFieldUnit = unit;
            Console.WriteLine($"Switched to {unit} at {timestamp}");
        }

        public void DealDamage(Timestamp timestamp, Element element, SecondPassStatsPage statsPage, Unit unit, 
            Types type, Enemy.Enemy enemy, int mvIndex, Reaction reaction,
            bool isHeavy, int icdOveride, string description = null)
        {
            double final_damage;
            double result = enemy.gauge.ElementApplied(timestamp, element, this, 
                unit.GetAbilityStats(statsPage, type, enemy, timestamp).GaugeStrength, 
                unit, statsPage, type, isHeavy, icdOveride);
            if (result > 0) final_damage = enemy.TakeDamage(timestamp, element, type, statsPage, 
                unit, mvIndex, reaction, isHeavy) * result;
            else final_damage = enemy.TakeDamage(timestamp, element, type, statsPage, unit, mvIndex, reaction, isHeavy);

            this.TotalDamage[units.IndexOf(unit)] += final_damage;
            if (description != null)
            {
                Console.WriteLine($"Damage dealt by {description} at {timestamp} is {final_damage}");
            }
            else
            {
                Console.WriteLine($"Damage dealt at {timestamp} is {final_damage}");
            }
        }

        public void CalculateDamage(Timestamp timestamp, Element element, int mvIndex, SecondPassStatsPage statsPage, 
            Units.Unit unit, Types type, Reaction reaction = Reaction.NONE, bool isHeavy = false, bool isAoe = true, 
            int bounces = 1, int icdOveride = 0, string description = null)
        {
            for (int i = 0; i < bounces; i++)
            {
                if (isAoe)
                {
                    foreach(Enemy.Enemy enemy in enemies) 
                    {
                        DealDamage(timestamp, element, statsPage, unit, type, enemy, 
                            mvIndex, reaction, isAoe, icdOveride, description);
                    }
                }
                else
                {
                    foreach(Enemy.Enemy enemy in enemies)
                    {
                        if (i > bounces) break;
                        DealDamage(timestamp, element, statsPage, unit, type, enemy, 
                            mvIndex, reaction, isAoe, icdOveride, description);
                        i++;
                    }
                }
            }
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