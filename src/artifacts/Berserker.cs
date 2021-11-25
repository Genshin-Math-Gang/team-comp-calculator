using System;
using Tcc.buffs;
using Tcc.stats;
using Tcc.units;

namespace Tcc.artifacts
{
    public class Berserker: ArtifactSet
    {
        static readonly Guid ID_2PC = new Guid("70f619bf-a99d-4797-9bee-555e503102d7");
        static readonly Guid ID_4PC = new Guid("b62bfb7b-a12e-42f2-9771-df4d54da8752");

        private static readonly FirstPassModifier MODIFIER_2PC = _ => new StatsPage(Stats.CritRate, 0.12);
        // TODO: this hp stuff seems sus and it seems like current hp should rely on time and not max
        static readonly FirstPassModifier MODIFIER_4PC = data => data.st.CurrentHp / data.st.GetStatsPage(data.timestamp).Hp < 0.7 ? 
            new StatsPage(Stats.CritRate, 0.24) : new StatsPage();

        public override void Add2pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, MODIFIER_2PC));
        public override void Add4pc(World world, Unit unit) => unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_4PC, MODIFIER_4PC));
    }
}
