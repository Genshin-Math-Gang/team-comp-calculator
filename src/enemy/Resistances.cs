using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Tcc.Elements;

namespace Tcc.Enemy
{
    public class Resistances
    {
        private Dictionary<Element, double> resistanceDict;

        public Resistances()
        {
            resistanceDict = new Dictionary<Element, double>()
            {
                [Element.ANEMO] = 0.1,
                [Element.GEO] = 0.1,
                [Element.PYRO] = 0.1,
                [Element.HYDRO] = 0.1,
                [Element.ELECTRO] = 0.1,
                [Element.CRYO] = 0.1,
                [Element.DENDRO] = 0.1,
            };
        }
        
        public Resistances(Dictionary<Element, double> resist)
        {
            resistanceDict = resist;
        }
        
        public Resistances(Element element, double value)
        {
            resistanceDict = new Dictionary<Element, double>();
            resistanceDict[element] = value;    
        }

        public double this[Element element]
        {
            get { return resistanceDict[element]; }
            set { resistanceDict[element] = value; }
        }

        public static Resistances operator *(Resistances r1, Resistances r2)
        {
            var result = new Resistances();
            foreach (var (element, res) in r1.resistanceDict)
            {
                result[element] = res + r2[element];
            }

            return result;
        }
    }
}