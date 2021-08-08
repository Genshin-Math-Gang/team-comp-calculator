using System.Collections.Generic;
using Tcc.Elements;

namespace Tcc.Events
{
    public class ElementalAttack: Action
    {
        private readonly double damage;
        private readonly Element element;
        private readonly int elementalUnits;

        public ElementalAttack(double damage, Element element, int elementalUnits)
        {
            this.damage = damage;
            this.element = element;
            this.elementalUnits = elementalUnits;
        }

        // public override List<ActionComponent> Components { get; }
    }
}
