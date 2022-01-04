using System;
using System.Collections.Generic;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class TenacityOfTheMillelith: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("12f19925-c85e-4f15-857f-7d305dfb179f");
        static readonly Guid ID_4PC = new Guid("1c5c261f-ab76-4b38-8f59-fcbf829dc58f");

        static readonly FirstPassModifier MODIFIER_2PC = _ => (Stats.HpPercent, 0.2);
        static readonly FirstPassModifier MODIFIER_4PC = _ =>new StatsPage(new Dictionary<Stats, double>
        {
            {Stats.AtkPercent, 0.2},
            {Stats.ShieldStrength, 0.3}
        });

        double cooldown4pcUntil;

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));

        public override void Add4pc(World world, Unit unit)
        {
            world.enemyHitHook += (_, data) =>
            {
                if(!data.attackType.IsType(Types.SKILL)) return;

                if(cooldown4pcUntil != null && data.timestamp < cooldown4pcUntil) return;
                else cooldown4pcUntil = data.timestamp + 0.5;

                foreach(var unitInParty in world.GetUnits()) unitInParty.AddBuff(new RefreshableBuff<FirstPassModifier>(ID_4PC, data.timestamp + 3, MODIFIER_4PC));
            };
        }
    }
}
