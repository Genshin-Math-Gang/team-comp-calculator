using System;
using System.Collections.Generic;

namespace Tcc.Elements
{
    class TransformativeScaling
    {
        public static double ReactionMultiplier (Reaction reaction)
        {
            switch (reaction)
            {
                case Reaction.BURNING: return 0.25;
                case Reaction.ELECTROCHARGED: return 1.2;
                case Reaction.SUPERCONDUCT: return 0.5;
                case Reaction.SHATTERED: return 1.5;
                case Reaction.OVERLOADED: return 2;
                case Reaction.SWIRL_CRYO: return 0.6;
                case Reaction.SWIRL_PYRO: return 0.6;
                case Reaction.SWIRL_HYDRO: return 0.6;
                case Reaction.SWIRL_ELECTRO: return 0.6;
            }

            return 0;
        }

        public static double EmScaling(double em)
        {
            return 1 + 16 * em / (2000 + em);
        }

        
        
        public static Dictionary<int, double> damage = new Dictionary<int, double>() {
        {1, 17.2}, 
        {2, 18.5}, 
        {3, 19.9}, 
        {4, 21.3}, 
        {5, 22.6}, 
        {6, 24.6}, 
        {7, 26.6}, 
        {8, 28.9}, 
        {9, 31.4}, 
        {10, 34.1}, 
        {11, 37.2}, 
        {12, 40.7}, 
        {13, 44.4}, 
        {14, 48.6}, 
        {15, 53.7}, 
        {16, 59.1}, 
        {17, 64.4}, 
        {18, 69.7}, 
        {19, 75.1}, 
        {20, 80.6}, 
        {21, 86.1}, 
        {22, 91.7}, 
        {23, 97.2}, 
        {24, 102.8}, 
        {25, 108.4}, 
        {26, 113.2}, 
        {27, 118.1}, 
        {28, 123.0}, 
        {29, 129.7}, 
        {30, 136.3}, 
        {31, 142.7}, 
        {32, 149.0}, 
        {33, 155.4}, 
        {34, 161.8}, 
        {35, 169.1}, 
        {36, 176.5}, 
        {37, 184.1}, 
        {38, 191.7}, 
        {39, 199.6}, 
        {40, 207.4}, 
        {41, 215.4}, 
        {42, 224.2}, 
        {43, 233.5}, 
        {44, 243.4}, 
        {45, 256.1}, 
        {46, 268.5}, 
        {47, 281.5}, 
        {48, 295.0}, 
        {49, 309.1}, 
        {50, 323.6}, 
        {51, 336.8}, 
        {52, 350.5}, 
        {53, 364.5}, 
        {54, 378.6}, 
        {55, 398.6}, 
        {56, 416.4}, 
        {57, 434.4}, 
        {58, 453.0}, 
        {59, 472.6}, 
        {60, 492.9}, 
        {61, 513.6}, 
        {62, 539.1}, 
        {63, 565.5}, 
        {64, 592.5}, 
        {65, 624.4}, 
        {66, 651.5}, 
        {67, 679.5}, 
        {68, 707.8}, 
        {69, 736.7}, 
        {70, 765.6}, 
        {71, 794.8}, 
        {72, 824.7}, 
        {73, 851.2}, 
        {74, 877.7}, 
        {75, 914.2}, 
        {76, 946.7}, 
        {77, 979.4}, 
        {78, 1011.2}, 
        {79, 1044.8}, 
        {80, 1077.4}, 
        {81, 1110.0}, 
        {82, 1143.0}, 
        {83, 1176.4}, 
        {84, 1210.2}, 
        {85, 1253.8}, 
        {86, 1289.0}, 
        {87, 1325.5}, 
        {88, 1363.5}, 
        {89, 1405.1}, 
        {90, 1446.9}, 
        {91, 1488.2}, 
        {92, 1528.4}, 
        {93, 1580.4}, 
        {94, 1630.8}, 
        {95, 1711.2}, 
        {96, 1780.5}, 
        {97, 1847.3}, 
        {98, 1911.5}, 
        {99, 1972.9}, 
        {100, 2030.1}, 
        };
    }
    
}