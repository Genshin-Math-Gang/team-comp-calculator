using System;
using System.Collections.Generic;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class ThunderingFury: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("c8e4425b-699e-4bda-be53-cdaa6559d0f0");
        static readonly Guid ID_4PC = new Guid("50643482-51e8-414e-85b2-04da0f4e65ce");

        static readonly FirstPassModifier MODIFIER_2PC = (_) => (Stats.ElectroDamageBonus, 0.15);

        static readonly FirstPassModifier MODIFIER_4PC = (_) => new StatsPage(new Dictionary<Stats, double>
        {
            {Stats.OverloadBonus, 0.4},
            {Stats.SuperconductBonus, 0.4},
            {Stats.ElectrochargedBonus, 0.4}
        });

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC, MODIFIER_4PC));
    }
}
