  
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

        public void AddCharacterEvent(Timestamp timestamp, Func<Timestamp, object[], List<WorldEvent>> characterAction, params object[] param)
        {
            object[] p = {this, null};
            param.CopyTo(p, 1);
            characterEvents.Add(new CharacterEvent(timestamp, characterAction, p));
        }
        public void AddCharacterEvent(Timestamp timestamp, Func<Timestamp, object[], List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction, this));
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
        
        public List<Enemy.Enemy> Enemies
        {
            get => enemies;
        }

        public void DealDamage(Timestamp timestamp, Element element, SecondPassStatsPage statsPage, Unit unit, 
            Types type, Enemy.Enemy enemy, int mvIndex, HitType hitType, string description = null)
        {

            var results = enemy.TakeDamage(timestamp, element, type, statsPage, unit, hitType, 
                mvIndex);
            double final_damage = results.Item1;
            List<WorldEvent> events = results.Item2 ?? new List<WorldEvent>();
            // scuffed
            foreach (var e in events)
            {
                AddWorldEvents(e);
            }
            TotalDamage[units.IndexOf(unit)] += final_damage;
            // hack to exit early 
            if (final_damage < 0) { return;}
            if (description != null)
            {
                Console.WriteLine($"Damage dealt by {description} at {timestamp} is {final_damage} to {enemy}");
            }
            else
            {
                Console.WriteLine($"Damage dealt at {timestamp} is {final_damage} to {enemy}");
            }
        }

        
        public void CalculateDamage(Timestamp timestamp, Element element, int mvIndex, SecondPassStatsPage statsPage, 
            Unit unit, Types type, HitType hitType, string description = null)
        {
            
            for (int i = 0; i < hitType.Bounces; i++)
            {
                // might be better to iterate over enemies randomly to be more realistic 
                if (hitType.IsAoe)
                {
                    foreach(Enemy.Enemy enemy in enemies) 
                    {
                        DealDamage(timestamp+i*hitType.Delay, element, statsPage, unit, type, enemy, 
                            mvIndex, hitType, description);
                    }
                }
                else
                {
                    foreach(Enemy.Enemy enemy in enemies)
                    {
                        if (i > hitType.Bounces) break;
                        DealDamage(timestamp+i*hitType.Delay, element, statsPage, unit, type, enemy, 
                            mvIndex, hitType, description);
                        i++;
                    }
                    
                    if (!hitType.SelfBounce)
                    {
                        break;
                    }
                }
            }
        }
        public void Simulate()
        {
            // minor order changes needed to make this work properly
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