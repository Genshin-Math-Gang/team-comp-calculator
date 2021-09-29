using System;

namespace Tcc.Stats
{
    public class MultipliableStat
    {

        private double current;
        public MultipliableStat(double flatBonus = 0, double percentBonus = 0, double baseValue = 0)
        {
            this.Base = baseValue;
            this.FlatBonus = flatBonus;
            this.PercentBonus = percentBonus;
            this.current = baseValue * (1+percentBonus) + flatBonus;
        }

        public double Base { get; }
        public double FlatBonus { get; }
        public double PercentBonus { get; }

        public double Current => current;

        public double Effective => Base * (1 + PercentBonus) + FlatBonus;

        public static MultipliableStat operator +(MultipliableStat first, MultipliableStat second)
        {
            return new MultipliableStat(
                flatBonus: first.FlatBonus + second.FlatBonus,
                percentBonus: first.PercentBonus + second.PercentBonus,
                baseValue: first.Base + second.Base
            );
        }

        public bool ReduceValue(double value)
        {
            current = Math.Max(Current - value, 0);
            return current <= 0;
        }

        public static implicit operator double(MultipliableStat stat) => stat.Effective;
    }
}