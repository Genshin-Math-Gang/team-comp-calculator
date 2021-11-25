using System;
using System.Collections.Generic;
using Tcc.elements;

namespace Tcc.stats
{
    public class StatsPage
    {

        private double[] StatValues = new double[(int) Stats.IndependentMultiplier + 1];
        
        public StatsPage(Dictionary<Stats, double> stats)
        {
            
            foreach (var (key, value) in stats)
            {
                StatValues[(int) key] = value;
            }
        }

        public StatsPage(Stats stat, double value)
        {
            StatValues[(int) stat] = value;
        }
        public StatsPage() { }

        public double this[Stats stat] => StatValues[(int) stat];

        public double Hp => this[Stats.HpBase] * (1 + this[Stats.HpPercent]) + this[Stats.HpFlat];

        public double Atk => this[Stats.AtkBase] * (1 + this[Stats.AtkPercent]) + this[Stats.AtkFlat];

        public double CritMultiplier => 1 + this[Stats.CritDamage] * this[Stats.CritRate];

        public double DamageMultiplier(Element element) =>
            1 + this[Stats.DamagePercent] + this[Converter.ElementToBonus(element)];


        public double ReactionBonus(Reaction reaction)
        {
            switch (reaction)
            {
                case Reaction.UNKNOWN or Reaction.NONE or Reaction.FREEZE or Reaction.CRYSTALLIZE_PYRO or 
                    Reaction.CRYSTALLIZE_HYDRO or Reaction.CRYSTALLIZE_ELECTRO or Reaction.CRYSTALLIZE_CRYO:
                    return 0;
                case Reaction.OVERLOADED:
                    return this[Stats.OverloadBonus];
                case Reaction.ELECTROCHARGED:
                    return this[Stats.ElectrochargedBonus];
                case Reaction.SUPERCONDUCT:
                    return this[Stats.SuperconductBonus];
                case Reaction.BURNING:
                    return this[Stats.BurningBonus];
                case Reaction.SHATTERED:
                    return this[Stats.ShatteredBonus];
                case Reaction.SWIRL_PYRO or Reaction.SWIRL_HYDRO or Reaction.SWIRL_ELECTRO or Reaction.SWIRL_CRYO:
                    return this[Stats.SwirlBonus];
                case Reaction.MELT:
                    return this[Stats.MeltBonus];
                case Reaction.VAPORIZE:
                    return this[Stats.VaporizeBonus];
                default:
                    throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null);
            }
        }
        
        
        public static StatsPage operator +(StatsPage page, StatsPage page2)
        {
            StatsPage statsPage = new StatsPage();
            for (int i=0; i < page.StatValues.Length; i++)
            {
                statsPage.StatValues[i] = page.StatValues[i] + page2.StatValues[i];
            }

            return statsPage;
        }
        
        public static StatsPage operator -(StatsPage page, StatsPage page2)
        {
            StatsPage statsPage = new StatsPage();
            for (int i=0; i < page.StatValues.Length; i++)
            {
                statsPage.StatValues[i] = page.StatValues[i] - page2.StatValues[i];
            }

            return statsPage;
        }
        
        
        public static implicit operator StatsPage((Stats, double) data) => new (data.Item1, data.Item2);
        
    }
    // old version
    /*public class StatsPage
    {
        public readonly CapacityStats capacityStats;
        public readonly GeneralStats generalStats;

        public StatsPage(CapacityStats capacityStats, GeneralStats generalStats)
        {
            this.capacityStats = capacityStats;
            this.generalStats = generalStats;
        }

        public MultipliableStat MaxHp => capacityStats.Hp;
        public int MaxEnergy => capacityStats.Energy;

        public int Level => generalStats.Level;
        public MultipliableStat Attack => generalStats.Attack;
        public MultipliableStat Defence => generalStats.Defence;
        public double ElementalMastery => generalStats.ElementalMastery;
        public double CritRate => generalStats.CritRate;
        public double CritDamage => generalStats.CritDamage;
        public double HealingBonus => generalStats.HealingBonus;
        public double IncomingHealingBonus => generalStats.IncomingHealingBonus;
        public double EnergyRecharge => generalStats.EnergyRecharge;
        public double CdReduction => generalStats.CdReduction;
        public double ShieldStrength => generalStats.ShieldStrength;
        public double DamagePercent => generalStats.DamagePercent;
        public double IndependentMultiplier => generalStats.IndependentMultiplier;
        public KeyedPercentBonus<Element> ElementalBonus => generalStats.ElementalBonus;
        public KeyedPercentBonus<Element> ElementalResistance => generalStats.ElementalResistance;

        public KeyedPercentBonus<Reaction> ReactionBonus => generalStats.ReactionBonus;

        public static StatsPage operator +(StatsPage page, GeneralStats generalStats)
        {
            return new StatsPage(
                capacityStats: page.capacityStats,
                generalStats: page.generalStats + generalStats
            );
        }
    }*/
}