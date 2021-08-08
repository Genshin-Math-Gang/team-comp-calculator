using System.Collections.Generic;
using System;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulateXianglingBuffDuringBurst();
            SimulateXianglingBuffDuringBurstStartup();
            SimulateAyakaWithoutBuff();
            SimulateAyakaWithBuffDuringBurst();
            SimulateAyakaWithBuff();
        }

        static void SimulateXianglingBuffDuringBurst()
        {
            Xiangling xiangling = new Xiangling(0);

            World world = new World();
            world.SetUnits(xiangling, null, null, null);

            world.AddCharacterEvent(1, xiangling.InitialBurst);
            world.AddCharacterEvent(2, xiangling.BurstHit);
            world.AddCharacterEvent(2.5, hackBennettBuff(xiangling));
            world.AddCharacterEvent(3.2, xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"Buff during burst total damage: {world.TotalDamage}");
        }

        static void SimulateXianglingBuffDuringBurstStartup()
        {
            Xiangling xiangling = new Xiangling(0);

            World world = new World();
            world.SetUnits(xiangling, null, null, null);

            world.AddCharacterEvent(1, xiangling.InitialBurst);
            world.AddCharacterEvent(1.5, hackBennettBuff(xiangling));
            world.AddCharacterEvent(2, xiangling.BurstHit);
            world.AddCharacterEvent(3.2, xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"Buff during startup total damage: {world.TotalDamage}");
        }

        static void SimulateAyakaWithoutBuff()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(1, ayaka.Burst);

            world.Simulate();

            Console.WriteLine($"Ayaka without buff total damage: {world.TotalDamage}");
        }

        static void SimulateAyakaWithBuff()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(0, hackBennettBuff(ayaka));
            world.AddCharacterEvent(1, ayaka.Burst);

            world.Simulate();

            Console.WriteLine($"Ayaka with buff throughout total damage: {world.TotalDamage}");
        }

        static void SimulateAyakaWithBuffDuringBurst()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(1, ayaka.Burst);
            world.AddCharacterEvent(1.1, hackBennettBuff(ayaka));

            world.Simulate();

            Console.WriteLine($"Ayaka with buff during burst total damage: {world.TotalDamage}");
        }

        static Func<double, List<WorldEvent>> hackBennettBuff(Unit unitToBuff)
        {
            return (timestamp) => new List<Events.WorldEvent> {
                new WorldEvent(timestamp, (_) => unitToBuff.AddBuff("Bennett Burst", new StaticUnitStats(50)))
            };
        }
    }
}
