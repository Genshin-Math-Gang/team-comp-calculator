using System;
using System.Collections.Generic;

namespace Tcc.Elements
{
    public class Guage
    {
        private double[] guageValues;

        public Guage(double[] guage_values)
        {
            this.guageValues = guage_values;
            
            // possibly do some validation later
        }
        
        // change return type later
        public int[] ElementApplied(int element_type, double guage_strength)
        {
            switch (element_type)
            {
                case Element.PHYSICAL
            }
        }
    }
}