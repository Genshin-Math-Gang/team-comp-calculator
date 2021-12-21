using System;
using Tcc.elements;

namespace Tcc.units
{
    public sealed class InfusionRef
    {
        private Func<Element> Getter;
        private Action<Element> Setter;

        public InfusionRef(Func<Element> getter, Action<Element> setter)
        {
            Getter = getter;
            Setter = setter;
        }
        public Element Value
        {
            get { return Getter(); }
            set { Setter(value); }
        }
    }
}