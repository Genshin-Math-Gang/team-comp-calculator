  
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tcc.elements;
using Tcc.enemy;
using Tcc.events;
using Tcc.stats;
using Tcc.units;

namespace Tcc
{
    public class World
    {
        List<Unit> units;
        List<CharacterEvent> characterEvents;
        private WorldEventQueue queuedWorldEvents;
        List<Enemy> enemies;
        public double[] TotalDamage;

        public event EventHandler<(Timestamp timestamp, Unit attacker, Element element, Types attackType)>
            enemyHitHook;

        public event EventHandler<(Unit from, Unit to, Timestamp timestamp)> unitSwapped;

        public World(List<Enemy> enemies)
        {
            this.characterEvents = new List<CharacterEvent>();
            this.TotalDamage = new double[4];
            this.enemies = enemies;
        }

        public Unit OnFieldUnit { get; private set; }

        public void SetUnits(Unit onFieldUnit, Unit unit2, Unit unit3, Unit unit4)
        {
            this.OnFieldUnit = onFieldUnit;
            this.units = new List<Unit> {onFieldUnit, unit2, unit3, unit4};
        }

        public ReadOnlyCollection<Unit> GetUnits() => units.AsReadOnly();

        public void AddEnemy(Enemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public void AddCharacterEvent(Timestamp timestamp, Func<Timestamp, object[], List<WorldEvent>> characterAction,
            params object[] param)
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

        public List<Enemy> Enemies
        {
            get => enemies;
        }

        public void EnemyDeath(Timestamp timestamp, Enemy enemy)
        {
            enemies.Remove(enemy);
            Console.WriteLine($"Enemy {enemy} died at {timestamp}");
            foreach (var unit in units)
            {
                unit.EnemyDeath(timestamp);
            }
            if (enemies.Count == 0)
            {
                AddWorldEvent(new End(timestamp));
            }
        }

        public void ClearQueue()
        {
            queuedWorldEvents = new WorldEventQueue();
        }

        public void DealDamage(Timestamp timestamp, Element element, SecondPassStatsPage statsPage, Unit unit,
            Types type, Enemy enemy, int mvIndex, HitType hitType, string description = null)
        {

            var results = enemy.TakeDamage(timestamp, element, type, statsPage, unit, hitType,
                mvIndex);
            double final_damage = results.Item1;
            List<WorldEvent> events = results.Item2 ?? new List<WorldEvent>();
            // scuffed
            AddWorldEvents(events);
            TotalDamage[units.IndexOf(unit)] += final_damage;
            // hack to exit early 
            if (final_damage < 0)
            {
                return;
            }

            if (description != null)
            {
                Console.WriteLine(
                    $"Damage dealt by {description} at {timestamp} is {final_damage:N2} to {enemy.GetHashCode()}");
            }
            else
            {
                Console.WriteLine($"Damage dealt at {timestamp} is {final_damage:N2} to {enemy.GetHashCode()}");
            }
        }


        public void CalculateDamage(Timestamp timestamp, Element element, int mvIndex, SecondPassStatsPage statsPage,
            Unit unit, Types type, HitType hitType, string description = null)
        {

            // this code has a lot of room for optimization if needed, could be parallelized or something
            for (int i = 0; i < hitType.Bounces; i++)
            {
                // might be better to iterate over enemies randomly to be more realistic 
                if (hitType.IsAoe)
                {
                    foreach (Enemy enemy in enemies)
                    {
                        DealDamage(timestamp + i * hitType.Delay, element, statsPage, unit, type, enemy,
                            mvIndex, hitType, description);
                    }
                }
                else
                {
                    foreach (Enemy enemy in enemies)
                    {
                        if (i > hitType.Bounces) break;
                        DealDamage(timestamp + i * hitType.Delay, element, statsPage, unit, type, enemy,
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

        public void InfuseAbility(Timestamp startTime, Timestamp endTime, Anemo unit)
        {
            if (unit.Infusion != Element.PHYSICAL) { return;}
            
            Element element = Element.PHYSICAL;
            foreach (var enemy in enemies)
            {
                var temp = Converter.AuraToElement(enemy.GetAura());
                if ((int) temp < (int) element)
                {
                    element = temp;
                    if (element == Element.PYRO)
                    {
                        break;
                    }
                }
                
            }

            if (element == Element.PHYSICAL)
            {
                return;
            }
            unit.Infusion = element;
            Console.WriteLine($"{unit} had their ability infused with {unit.Infusion} at {startTime}");
            AddWorldEvent(new WorldEvent(endTime, _ => unit.Infusion=Element.PHYSICAL));
        }
        

        public void Simulate()
        {
            queuedWorldEvents = new WorldEventQueue(characterEvents);


            while (queuedWorldEvents.IsEmpty())
            {
                var nextEvent = queuedWorldEvents.DeQueue();
                nextEvent.Apply(this);
            }
        }

        /*public void AddWorldEvents(params WorldEvent[] events)
        {
            queuedWorldEvents.AddRange(events);

            queuedWorldEvents = queuedWorldEvents
                .OrderBy((worldEvent) => worldEvent.Timestamp)
                .ToList();
        }*/

        public void AddWorldEvents(List<WorldEvent> worldEvents)
        {
            foreach (var worldEvent in worldEvents)
            {
                queuedWorldEvents.Add(worldEvent);
            }
        }

        public void AddWorldEvent(WorldEvent worldEvent)
        {

            queuedWorldEvents.Add(worldEvent);
        }
    }
}