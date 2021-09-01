using Tcc.Elements;

namespace Tcc.Stats
{
    public class GeneralStats
    {
        public NonKeyedStats NonKeyedStats { get; }

        // Elemental type
        public KeyedStatBonus<Element> ElementalBonus { get; }
        public KeyedPercentBonus<Element> ElementalResistance { get; }

        // Attributes not listed on stats page
        public KeyedPercentBonus<Reaction> ReactionBonus { get; }

        public GeneralStats(
            int level = 1,
            double baseAttack = 0, double flatAttack = 0, double attackPercent = 0,
            double baseDefence = 0, double flatDefence = 0, double defencePercent = 0,
            double elementalMastery = 0,
            double critRate = 0, double critDamage = 0,
            double healingBonus = 0, double incomingHealingBonus = 0,
            double energyRecharge = 0, double cdReduction = 0, double shieldStrength = 0,
            double damagePercent = 0, double independentMultiplier = 1,
            KeyedStatBonus<Element> elementalBonus = null,
            KeyedPercentBonus<Element> elementalResistance = null,
            KeyedPercentBonus<Reaction> reactionBonus = null
        ): this(
            nonKeyedStats: new NonKeyedStats(
                level: level,
                baseAttack: baseAttack,
                flatAttack: flatAttack,
                attackPercent: attackPercent,
                baseDefence: baseDefence,
                flatDefence: flatDefence,
                defencePercent: defencePercent,
                elementalMastery: elementalMastery,
                critRate: critRate,
                critDamage: critDamage,
                healingBonus: healingBonus,
                incomingHealingBonus: incomingHealingBonus,
                energyRecharge: energyRecharge,
                cdReduction: cdReduction,
                shieldStrength: shieldStrength,
                damagePercent: damagePercent,
                independentMultiplier: independentMultiplier
            ),
            elementalBonus: elementalBonus,
            elementalResistance: elementalResistance,
            reactionBonus: reactionBonus
        ) {
        }

        GeneralStats(
            NonKeyedStats nonKeyedStats,
            KeyedStatBonus<Element> elementalBonus, KeyedPercentBonus<Element> elementalResistance,
            KeyedPercentBonus<Reaction> reactionBonus
        ) {
            this.NonKeyedStats = nonKeyedStats;
            this.ElementalBonus = elementalBonus ?? new KeyedStatBonus<Element>();
            this.ElementalResistance = elementalResistance ?? new KeyedPercentBonus<Element>();
            this.ReactionBonus = reactionBonus ?? new KeyedPercentBonus<Reaction>();
        }

        public static GeneralStats operator +(GeneralStats first, GeneralStats second)
        {
            return new GeneralStats(
                nonKeyedStats: first.NonKeyedStats + second.NonKeyedStats,
                elementalBonus: first.ElementalBonus + second.ElementalBonus,
                elementalResistance: first.ElementalResistance + second.ElementalResistance,
                reactionBonus: first.ReactionBonus + second.ReactionBonus
            );
        }

        public int Level => NonKeyedStats.Level;
        public MultipliableStat Attack => NonKeyedStats.Attack;
        public MultipliableStat Defence => NonKeyedStats.Defence;
        public double ElementalMastery => NonKeyedStats.ElementalMastery;
        public double CritRate => NonKeyedStats.CritRate;
        public double CritDamage => NonKeyedStats.CritDamage;
        public double HealingBonus => NonKeyedStats.HealingBonus;
        public double IncomingHealingBonus => NonKeyedStats.IncomingHealingBonus;
        public double EnergyRecharge => NonKeyedStats.EnergyRecharge;
        public double CdReduction => NonKeyedStats.CdReduction;
        public double ShieldStrength => NonKeyedStats.ShieldStrength;
        public double DamagePercent => NonKeyedStats.DamagePercent;
        public double IndependentMultiplier => NonKeyedStats.IndependentMultiplier;

        public static implicit operator GeneralStats((Element element, double damagePercent) bonus)
            => new GeneralStats(elementalBonus: new KeyedStatBonus<Element>((bonus.element, new NonKeyedStats(damagePercent: bonus.damagePercent))));

        public static implicit operator GeneralStats(KeyedPercentBonus<Reaction> reactionBonus) => new GeneralStats(reactionBonus: reactionBonus);
    }
}
