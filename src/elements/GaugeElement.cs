using System;

namespace Tcc.elements
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
            GaugeValue = Math.Max(GaugeValue - time / DecayRate, 0);
        }

        public void UpdateGauge(double newStrength)
        {
            GaugeValue = Math.Max(newStrength, GaugeValue);
        }

        public override string ToString()
        {
            return Element + " " + GaugeValue;
        }
    }
}