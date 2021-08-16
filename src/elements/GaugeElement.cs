using System;
using System.Collections.Generic;

namespace Tcc.Elements
{
    public class GaugeElement
    {
        public double GaugeValue { get; set; }
        public double DecayRate { get; }
        public Element Element { get; }

        public GaugeElement(Element element, double strength)
        {
            this.Element = element;
            this.GaugeValue = strength;
            this.DecayRate = 2.5 + 7 / strength;
        }

        public void TimeDecay(double time)
        {
            this.GaugeValue -= time / DecayRate;
        }


    }
}