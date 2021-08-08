using System;
using System.Collections.Generic;
using System.Linq;
using Tcc.Events;
using Tcc.Units;

namespace Tcc
{
    public class World
    {
        Unit onFieldUnit;
        List<Unit> units;
        List<CharacterEvent> characterEvents;

        public World()
        {
            this.characterEvents = new List<CharacterEvent>();
            this.TotalDamage = 0;
        }

        public double TotalDamage { get; private set; }

        public void SetUnits(Unit onFieldUnit, Unit unit2, Unit unit3, Unit unit4)
        {
            this.onFieldUnit = onFieldUnit;
            this.units = new List<Unit> { onFieldUnit, unit2, unit3, unit4 };
        }

        public void AddCharacterEvent(double timestamp, Func<double, List<WorldEvent>> characterAction)
        {
            characterEvents.Add(new CharacterEvent(timestamp, characterAction));
        }

        public void DealDamage(double damage)
        {
            this.TotalDamage += damage;
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