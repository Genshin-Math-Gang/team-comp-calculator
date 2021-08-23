using System;
using Tcc.Elements;

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
        public double CdReduction { get; }
        public double ShieldStrength { get; }
        public double DamagePercent { get; }
        public KeyedPercentBonus<Element> ElementalBonus { get; }
        public KeyedPercentBonus<Reaction> ReactionBonus { get; }
        public KeyedPercentBonus<Element> ElementalResistance { get; }

        public Stats (
            double[] motionValues = null,
            double baseHp = 0, double flatHp = 0, double hpPercent = 0,
            double baseAttack = 0, double flatAttack = 0, double attackPercent = 0,
            double baseDefence = 0, double flatDefence = 0, double defencePercent = 0,
            double elementalMastery = 0, double critRate = 0, double critDamage = 0,
            double healingBonus = 0, double incomingHealingBonus = 0, double energyRecharge = 0,
            double cdReduction = 0, double shieldStrength = 0, double damagePercent = 0,
            KeyedPercentBonus<Element> elementalBonus = null, KeyedPercentBonus<Reaction> reactionBonus = null,
            KeyedPercentBonus<Element> elementalResistance = null
        ): this(
            motionValues: motionValues,
            maxHp: new MultipliableStat(baseValue: baseHp, flatBonus: flatHp, percentBonus: hpPercent),
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
            elementalBonus: elementalBonus,
            reactionBonus: reactionBonus,
            elementalResistance: elementalResistance
        ) {
        }

        Stats(
            double[] motionValues, MultipliableStat maxHp, MultipliableStat attack, MultipliableStat defence,
            double elementalMastery, double critRate, double critDamage,
            double healingBonus, double incomingHealingBonus, double energyRecharge,
            double cdReduction, double shieldStrength, double damagePercent,
            KeyedPercentBonus<Element> elementalBonus, KeyedPercentBonus<Reaction> reactionBonus,
            KeyedPercentBonus<Element> elementalResistance
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
            this.ReactionBonus = reactionBonus ?? new KeyedPercentBonus<Reaction>();
            this.ElementalResistance = elementalResistance ?? new KeyedPercentBonus<Element>();
        }

        public double CalculateHitDamage(int mvIndex, Element element, Reaction? reaction = null)
        {
            var totalDamagePercent = 1 + DamagePercent;

            totalDamagePercent += ElementalBonus.GetDamagePercentBonus(element);
            if(reaction.HasValue) totalDamagePercent += ReactionBonus.GetDamagePercentBonus(reaction.Value);

            return MotionValues[mvIndex] * Attack * totalDamagePercent * (1 + CritRate * CritDamage);
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
                elementalResistance: first.ElementalResistance + second.ElementalResistance
            );
        }

        public static implicit operator Stats(KeyedPercentBonus<Element> bonus) => new Stats(elementalBonus: bonus);
        public static implicit operator Stats(KeyedPercentBonus<Reaction> bonus) => new Stats(reactionBonus: bonus);
    }
}
