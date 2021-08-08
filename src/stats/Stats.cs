using System;

namespace Tcc.Stats
{
    public class Stats
    {
        // Implemented all the stats
        public double[] MV { get; } //Motion Value
        public double BaseHP { get; } //Base HP
        public double HPP { get; } //HP percantege
        public double HPF { get; } //HP flat
        public double TotalDMG { get; } //Total DMG%
        public double BaseAttack { get; } //Base Attack
        public double AttackP { get; } //Attack percantage, should be between 0 and 1.
        public double AttackF { get; } //Attack flat
        public double CR { get; } //Crit rate
        public double CD { get; } //Crit DMG

        public Stats (double[] mv, double base_hp, double hp_p, double hp_f, double total_DMG, double base_attack, double attack_p, double attack_f, double crit_rate, double crit_dmg)
        {
            this.MV = mv;
            this.BaseHP = base_hp;
            this.HPP = hp_p;
            this.HPF = hp_f;
            this.TotalDMG = total_DMG;
            this.BaseAttack = base_attack;
            this.AttackP = attack_p;
            this.AttackF = attack_f;
            this.CR = crit_rate;
            this.CD = crit_dmg;
        }

        public Stats ()
        {
            this.MV = new double[] {0.0};
            this.BaseHP = 0.0;
            this.HPP = 0.0;
            this.HPF = 0.0;
            this.TotalDMG = 0.0;
            this.BaseAttack = 0.0;
            this.AttackP = 0.0;
            this.AttackF = 0.0;
            this.CR = 0.0;
            this.CD = 0.0;
        }

        public double CalculateHitDamage(int index) {
            return this.MV[index] * (this.BaseAttack*(1 + AttackP) + AttackF) * (1 + this.TotalDMG) * (1 + this.CR * this.CD);
        }

        public static Stats operator +(Stats a, Stats b)
        {
            double[] temp = new double[a.MV.Length];

            if (b.MV[0] != -1)
            {
                if (a.MV.Length != b.MV.Length)
                throw new Exception("Cannot add different motion values together");

                temp = new double[a.MV.Length];

                for(int i=0; i < a.MV.Length; i++)
                {
                    temp[i] = a.MV[i] + b.MV[i];
                }
            }
            else
                a.MV.CopyTo(temp, 0);

            return new Stats(
                mv:temp,
                base_hp: a.BaseHP + b.BaseHP,
                hp_p: a.HPP + b.HPP,
                hp_f: a.HPF + b.HPF,
                total_DMG: a.TotalDMG + b.TotalDMG,
                base_attack: a.BaseAttack + b.BaseAttack,
                attack_p: a.AttackP + b.AttackP,
                attack_f: a.AttackF + b.AttackF,
                crit_rate: a.CR + b.CR,
                crit_dmg: a.CD + b.CD);
        }
        public static Stats operator -(Stats a)
        {
            return new Stats(
                mv:a.MV,
                base_hp: -a.BaseHP,
                hp_p: -a.HPP,
                hp_f: -a.HPF,
                total_DMG: -a.TotalDMG,
                base_attack: -a.BaseAttack,
                attack_p: -a.AttackP,
                attack_f: -a.AttackF,
                crit_rate: -a.CR,
                crit_dmg: -a.CD);
        }
    }
}
