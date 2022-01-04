  
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection;
using Tcc.elements;
using Tcc.enemy;
using Tcc.events;
using Tcc.stats;
using Tcc.units;

namespace Tcc
{
    public class World
    {
        Unit[] units;
        List<CharacterEvent> characterEvents;
        private WorldEventQueue queuedWorldEvents;
        List<Enemy> enemies;
        public double[] TotalDamage;

        private Random r = new ();
        private bool isDeterministic = false;
        private int simulationCount = 2;
        public bool Verbose;




        public event EventHandler<(double timestamp, Unit attacker, Element element, Types attackType)>
            enemyHitHook;

        public event EventHandler<(Unit from, Unit to, double timestamp)> unitSwapped;

        public World(List<Enemy> enemies)
        {
            characterEvents = new List<CharacterEvent>();
            TotalDamage = new double[4];
            this.enemies = enemies;
        }
        
        public World(List<Enemy> enemies, ActionList actionList): this(actionList)
        {
            this.enemies = enemies;
        }

        public World(ActionList actionList)
        {
            enemies = new List<Enemy> {new Enemy()}; 
            TotalDamage = new double[4];
            units = new Unit[4];
            for (int i = 0; i < 4; i++)
            {
                units[i] = Unit.UnitCreator(actionList.characters[i]);
            }
            OnFieldUnit = units[0];

            List<Action> actions = actionList.eventList;
            characterEvents = new List<CharacterEvent>();
            for (int i = 0; i < actions.Count; i++)
            {
                Unit u = units[actions[i].Character];
                double t = actions[i].Timestamp;
                object[] p = actions[i].param;
                // IF YOU GET WEIRD BUGS FROM SOMETHING CALLED AnonymousTypes,
                // TRY CHANGING CODE FROM new {...} to new object[]{...}
                switch (actions[i].ActionType)
                {
                    case ActionType.Normal:
                        AddCharacterEvent(t, time => u.AutoAttack(time, (AutoString) p[0]));
                        break;
                    case ActionType.Charge:
                        throw new NotImplementedException("charged attacks broken, please fix mathboi");
                        break;
                    case ActionType.Plunge:
                        throw new NotImplementedException("plunge attacks broken, please fix mathboi");
                        break;
                    case ActionType.Skill:
                        AddCharacterEvent(t, time => u.Skill(time, p));
                        break;
                    case ActionType.Burst:
                        AddCharacterEvent(t, u.Burst);
                        break;
                    case ActionType.Swap:
                        AddCharacterEvent(t, u.SwitchUnit);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Reset()
        {
            OnFieldUnit = units[0];
            // TODO: change the enemy thing later
            
            // this code seems like it should be faster but isn't
            /*foreach (var enemy in enemies)
            {
                enemy.Reset();
            }*/
            enemies = new List<Enemy>{new Enemy()};
            TotalDamage = new double[4];
            foreach (var u in units)
            {
                u.Reset();
            }
        }

        public Unit OnFieldUnit { get; private set; }

        public void SetUnits(Unit onFieldUnit, Unit unit2, Unit unit3, Unit unit4)
        {
            OnFieldUnit = onFieldUnit;
            units = new Unit[] {onFieldUnit, unit2, unit3, unit4};
        }

        // how important is it for this to be readonly
        public Unit[] GetUnits() => units;

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void AddCharacterEvent(double timestamp, Func<double, object[], List<WorldEvent>> characterAction,
            params object[] param)
        {
            object[] p = {this, null};
            param.CopyTo(p, 1);
            characterEvents.Add(new CharacterEvent(timestamp, characterAction, p));
        }

        public void AddCharacterEvent(double timestamp, Func<double, object[], List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction, this));
        }

        public void AddCharacterEvent(double timestamp, Func<double, List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction));
        }

        public void SwitchUnit(double timestamp, Unit unit)
        {
            unitSwapped?.Invoke(this, (OnFieldUnit, unit, timestamp));
            OnFieldUnit = unit;
            if (Verbose)
            {
                Console.WriteLine($"Switched to {unit} at {timestamp}");
            }
            
        }

        public List<Enemy> Enemies
        {
            get => enemies;
        }

        public void EnemyDeath(double timestamp, Enemy enemy)
        {
            enemies.Remove(enemy);
            if (Verbose) Console.WriteLine($"Enemy {enemy} died at {timestamp}");
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

        public void DealDamage(double timestamp, SecondPassStatsPage statsPage, Unit unit,
            Types type, Enemy enemy, double mvs, HitType hitType, string description = null)
        {
            
            var results = enemy.TakeDamage(timestamp, type, statsPage, unit, hitType,
                mvs, r, isDeterministic);
            double final_damage = results.Item1;
            List<WorldEvent> events = results.Item2 ?? new List<WorldEvent>();
            // scuffed
            AddWorldEvents(events);
            foreach (var u in GetUnits())
            {
                if (u is not null)  AddWorldEvent(u.DealtDamage(timestamp, unit));
            }
            TotalDamage[Array.IndexOf(units, unit)] += final_damage;
            // hack to exit early 
            if (final_damage < 0)
            {
                return;
            }

            if (Verbose) Console.WriteLine(
                description != null
                    ? $"Damage dealt by {description} at {timestamp} is {final_damage:N2} to {enemy.GetHashCode()}"
                    : $"Damage dealt at {timestamp} is {final_damage:N2} to {enemy.GetHashCode()}");
        }


        public void CalculateDamage(double timestamp, double mvs, SecondPassStatsPage statsPage,
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
                        DealDamage(timestamp + i * hitType.Delay, statsPage, unit, type, enemy,
                            mvs, hitType, description);
                    }
                }
                else
                {
                    foreach (Enemy enemy in enemies)
                    {
                        if (i > hitType.Bounces) break;
                        DealDamage(timestamp + i * hitType.Delay, statsPage, unit, type, enemy,
                            mvs, hitType, description);
                        i++;
                    }

                    if (!hitType.SelfBounce)
                    {
                        break;
                    }
                }
            }
        }

        public void InfuseAbility(double startTime, double endTime, Anemo unit)
        {
            if (unit.GetInfusion() != Element.PHYSICAL) { return;}
            
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
            unit.SetInfusion(element);
            if (Verbose) Console.WriteLine($"{unit} had their ability infused with {unit.GetInfusion()} at {startTime}");
            AddWorldEvent(new WorldEvent(endTime, _ => unit.SetInfusion(Element.PHYSICAL)));
        }
        

        public void Simulate(bool verbose=false)
        {
            queuedWorldEvents = new WorldEventQueue(characterEvents);

            Verbose = verbose;

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
                .OrderBy((worldEvent) => worldEvent.double)
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