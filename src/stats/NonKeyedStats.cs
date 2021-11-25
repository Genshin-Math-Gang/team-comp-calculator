namespace Tcc.Stats
{
    public class NonKeyedStats
    {
        // "Base" stats
        public int Level { get; } // Inclusion debatable, might eventually be buffable
        public MultipliableStat Attack { get; }
        public MultipliableStat Defence { get; }
        public double ElementalMastery { get; }

        // Advanced stats
        public double CritRate { get; }
        public double CritDamage { get; }
        public double HealingBonus { get; }
        public double IncomingHealingBonus { get; }
        public double EnergyRecharge { get; }
        public double CdReduction { get; }
        public double ShieldStrength { get; }

        // Multipliers
        public double DamagePercent { get; }
        public double IndependentMultiplier { get; }

        public NonKeyedStats(
            int level = 1,
            double baseAttack = 0, double flatAttack = 0, double attackPercent = 0,
            double baseDefence = 0, double flatDefence = 0, double defencePercent = 0,
            double elementalMastery = 0,
            double critRate = 0, double critDamage = 0,
            double healingBonus = 0, double incomingHealingBonus = 0,
            double energyRecharge = 0, double cdReduction = 0, double shieldStrength = 0,
            double damagePercent = 0, double independentMultiplier = 1
        ): this(
            level: level,
            attack: new MultipliableStat(baseValue: baseAttack, flatBonus: flatAttack, percentBonus: attackPercent),
            defence: new MultipliableStat(baseValue: baseDefence, flatBonus: flatDefence, percentBonus: defencePercent),
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
        ) {
        }

        NonKeyedStats(
            int level,
            MultipliableStat attack, MultipliableStat defence, double elementalMastery,
            double critRate, double critDamage,
            double healingBonus, double incomingHealingBonus,
            double energyRecharge, double cdReduction, double shieldStrength,
            double damagePercent, double independentMultiplier
        ) {
            this.Level = level;
            this.Attack = attack;
            this.Defence = defence;
            this.ElementalMastery = elementalMastery;
            this.CritRate = critRate;
            this.CritDamage = critDamage;
            this.HealingBonus = healingBonus;
            this.IncomingHealingBonus = incomingHealingBonus;
            this.EnergyRecharge = energyRecharge;
            this.CdReduction = cdReduction;
            this.ShieldStrength = shieldStrength;
            this.DamagePercent = damagePercent;
            this.IndependentMultiplier = independentMultiplier;
        }

        public static NonKeyedStats operator +(NonKeyedStats first, NonKeyedStats second)
        {
            return new NonKeyedStats(
                level: first.Level + second.Level,
                attack: first.Attack + second.Attack,
                defence: first.Defence + second.Defence,
                elementalMastery: first.ElementalMastery + second.ElementalMastery,
                critRate: first.CritRate + second.CritRate,
                critDamage: first.CritDamage + second.CritDamage,
                healingBonus: first.HealingBonus + second.HealingBonus,
                incomingHealingBonus: first.IncomingHealingBonus + second.IncomingHealingBonus,
                energyRecharge: first.EnergyRecharge + second.EnergyRecharge,
                cdReduction: first.CdReduction + second.CdReduction,
                shieldStrength: first.ShieldStrength + second.ShieldStrength,
                damagePercent: first.DamagePercent + second.DamagePercent,
                independentMultiplier: first.IndependentMultiplier * second.IndependentMultiplier
            );
        }
        
        public static NonKeyedStats operator -(NonKeyedStats first, NonKeyedStats second)
        {
            return new NonKeyedStats(
                level: first.Level - second.Level,
                attack: first.Attack - second.Attack,
                defence: first.Defence - second.Defence,
                elementalMastery: first.ElementalMastery - second.ElementalMastery,
                critRate: first.CritRate - second.CritRate,
                critDamage: first.CritDamage - second.CritDamage,
                healingBonus: first.HealingBonus - second.HealingBonus,
                incomingHealingBonus: first.IncomingHealingBonus - second.IncomingHealingBonus,
                energyRecharge: first.EnergyRecharge - second.EnergyRecharge,
                cdReduction: first.CdReduction - second.CdReduction,
                shieldStrength: first.ShieldStrength - second.ShieldStrength,
                damagePercent: first.DamagePercent - second.DamagePercent,
                independentMultiplier: first.IndependentMultiplier / second.IndependentMultiplier
            );
        }
    }
}
