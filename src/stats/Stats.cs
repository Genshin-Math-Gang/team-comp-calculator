using System;
using Tcc.Elements;
using System.Collections.Generic;

namespace Tcc.Stats
{
    public class Stats
    {
        public double[] MotionValues { get; }
        public MultipliableStat MaxHp { get; }
        public MultipliableStat Attack { get; }
        public MultipliableStat Defence { get; }
        public double ElementalMastery { get; }
        public double CritRate { get; }
        public double CritDamage { get; }
        public double HealingBonus { get; }
        public double IncomingHealingBonus { get; }
        public double EnergyRecharge { get; }
        public double CdReduction { get; } //This seems useless?
        public double ShieldStrength { get; }
        public double DamagePercent { get; }
        public KeyedPercentBonus<Element> ElementalBonus { get; }
        public KeyedPercentBonus<double> ReactionBonus { get; }
        public KeyedPercentBonus<Element> ElementalResistance { get; }
        public int Energy { get; }
        public int Level { get; }
        public double DEFReduction { get; }
        public double GaugeStrength { get; }
        public Timestamp ICD { get; }
        public int HitPity { get; }

        public Stats (
            double[] motionValues = null,
            double baseHp = 0, double flatHp = 0, double hpPercent = 0,
            double baseAttack = 0, double flatAttack = 0, double attackPercent = 0,
            double baseDefence = 0, double flatDefence = 0, double defencePercent = 0,
            double elementalMastery = 0, double critRate = 0, double critDamage = 0,
            double healingBonus = 0, double incomingHealingBonus = 0, double energyRecharge = 0,
            double cdReduction = 0, double shieldStrength = 0, double damagePercent = 0,
            KeyedPercentBonus<Element> elementalBonus = null, KeyedPercentBonus<double> reactionBonus = null,
            KeyedPercentBonus<Element> elementalResistance = null, int energy = 0, int level = 1, double defReduction = 0,
            double gaugeStrength = 1, Timestamp icd = null, int hitPity = 3
        ) {
            this.MotionValues = motionValues;
            this.MaxHp = new MultipliableStat(baseValue: baseHp, flatBonus: flatHp, percentBonus: hpPercent);
            this.Attack = new MultipliableStat(baseValue: baseAttack, flatBonus: flatAttack, percentBonus: attackPercent);
            this.Defence = new MultipliableStat(baseValue: baseDefence, flatBonus: flatDefence, percentBonus: defencePercent);
            this.ElementalMastery = elementalMastery;
            this.CritRate = critRate;
            this.CritDamage = critDamage;
            this.HealingBonus = healingBonus;
            this.IncomingHealingBonus = incomingHealingBonus;
            this.EnergyRecharge = energyRecharge;
            this.CdReduction = cdReduction;
            this.ShieldStrength = shieldStrength;
            this.DamagePercent = damagePercent;
            this.ElementalBonus = elementalBonus ?? new KeyedPercentBonus<Element>();
            this.ReactionBonus = reactionBonus ?? new KeyedPercentBonus<double>(new Dictionary<double, double>() {{Reaction.BURNING, 0}, {Reaction.ELECTROCHARGED, 0}, {Reaction.MELT, 0}, {Reaction.OVERLOADED, 0}, {Reaction.SUPERCONDUCT, 0}, {Reaction.SWIRL_CRYO, 0}, {Reaction.SWIRL_ELECTRO, 0}, {Reaction.SWIRL_HYDRO, 0}, {Reaction.SWIRL_PYRO, 0}, {Reaction.VAPORIZE, 0}});
            this.ElementalResistance = elementalResistance ?? new KeyedPercentBonus<Element>(new Dictionary<Element, double>() {{Element.ANEMO, 0.1}, {Element.CRYO, 0.1}, {Element.DENDRO, 0.1}, {Element.ELECTRO, 0.1}, {Element.GEO, 0.1}, {Element.HYDRO, 0.1}, {Element.PHYSICAL, 0.1}, {Element.PYRO, 0.1}});
            this.Energy = energy;
            this.Level = level;
            this.DEFReduction = defReduction;
            this.GaugeStrength = gaugeStrength;
            this.HitPity = hitPity;
            this.ICD = icd ?? new Timestamp(2.5);
        }

        Stats(
            double[] motionValues, MultipliableStat maxHp, MultipliableStat attack, MultipliableStat defence,
            double elementalMastery, double critRate, double critDamage,
            double healingBonus, double incomingHealingBonus, double energyRecharge,
            double cdReduction, double shieldStrength, double damagePercent,
            KeyedPercentBonus<Element> elementalBonus, KeyedPercentBonus<double> reactionBonus,
            KeyedPercentBonus<Element> elementalResistance, int energy, int level, double defReduction,
            double gaugeStrength, Timestamp icd, int hitPity
        ) {
            this.MotionValues = motionValues;
            this.MaxHp = maxHp;
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
            this.ElementalBonus = elementalBonus ?? new KeyedPercentBonus<Element>();
            this.ReactionBonus = reactionBonus ?? new KeyedPercentBonus<double>();
            this.ElementalResistance = elementalResistance ?? new KeyedPercentBonus<Element>();
            this.Energy = energy;
            this.Level = Level;
            this.DEFReduction = defReduction;
            this.GaugeStrength = gaugeStrength;
            this.HitPity = hitPity;
            this.ICD = icd;
        }

        public static Stats operator +(Stats first, Stats second)
        {
            double[] motionValues = null;

            if(first.MotionValues != null)
            {
                if(second.MotionValues != null)
                {
                    if(first.MotionValues.Length != second.MotionValues.Length) throw new Exception("Cannot add different motion values together");

                    motionValues = new double[first.MotionValues.Length];

                    for(int i = 0; i < motionValues.Length; i++)
                    {
                        motionValues[i] = first.MotionValues[i] + second.MotionValues[i];
                    }
                }
                else
                {
                    motionValues = (double[])first.MotionValues.Clone();
                }
            }
            else if(second.MotionValues != null)
            {
                motionValues = (double[])second.MotionValues.Clone();
            }

            return new Stats(
                motionValues: motionValues,
                maxHp: first.MaxHp + second.MaxHp,
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
                elementalBonus: first.ElementalBonus + second.ElementalBonus,
                reactionBonus: first.ReactionBonus + second.ReactionBonus,
                elementalResistance: first.ElementalResistance + second.ElementalResistance,
                energy: first.Energy + second.Energy,
                level: first.Level + second.Level,
                defReduction: first.DEFReduction + second.DEFReduction,
                gaugeStrength: first.GaugeStrength + second.GaugeStrength,
                icd: first.ICD + second.ICD,
                hitPity: first.HitPity + second.HitPity
            );
        }
        public static implicit operator Stats(KeyedPercentBonus<Element> bonus) => new Stats(elementalBonus: bonus);
        public static implicit operator Stats(KeyedPercentBonus<double> bonus) => new Stats(reactionBonus: bonus);
    }
}
