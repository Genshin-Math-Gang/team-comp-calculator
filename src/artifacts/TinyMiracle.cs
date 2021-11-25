namespace Tcc.artifacts
{
    /*public class TinyMiracle: ArtifactSet
    {
        // i do not care about this
        /*
        static readonly Guid ID_2PC = new Guid("6a9457be-248c-41d3-b59f-f6b77d78a5d5");

        public override void Add2pc(World world, Unit unit)
        {
            var stats = new GeneralStats(elementalResistance: new KeyedPercentBonus<Element>(
                Enum.GetValues<Element>()
                    .Where((element) => element != Element.PHYSICAL)
                    .Select((element) => (element, 0.2))
                    .ToArray()
            ));

            unit.AddBuff(new PermanentBuff<FirstPassModifier>(ID_2PC, (_) => stats));
        }

        public override void Add4pc(World world, Unit unit) => throw new NotImplementedException();
    }*/
}
