using System.Collections.Generic;
using System;
using Tcc.Buffs.Artifacts;
using Tcc.Buffs;
using Tcc.Events;
using Tcc.Units;
using Tcc.Weapons;

namespace Tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            // SimulateXianglingBurst();
/*          SimulateXianglingBuffDuringBurst();
            SimulateXianglingBuffDuringBurstStartup();
            SimulateAyakaWithoutBuff();
            SimulateAyakaWithBuffDuringBurst();
            SimulateAyakaWithBuff(); */
            SimulateShogunNonsense();
        }

        static void SimulateShogunNonsense()
        {
            Shogun shogun = new Shogun(0);
            Xiangling xiangling = new Xiangling(0);

            World world = new World();
            world.SetUnits(shogun, xiangling, null, null);

            world.AddCharacterEvent(new Timestamp(0), shogun.Skill);

            world.AddCharacterEvent(new Timestamp(1), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(1.5), xiangling.InitialBurst);

            world.AddCharacterEvent(new Timestamp(3), xiangling.BurstHit);

            world.AddCharacterEvent(new Timestamp(4), shogun.SwitchUnit);
    
            world.AddCharacterEvent(new Timestamp(4), (timestamp) => new List<WorldEvent> {
                new WorldEvent(timestamp, (world) => world.AddBuff(timestamp, shogun, new BasicBuffFromUnit(new Guid("e6a06a2f-7d42-437a-b330-fb08b79d5045"), new Stats.Stats(energyRecharge: 0.5)), "test buff"))
            });

            world.AddCharacterEvent(new Timestamp(4.5), xiangling.BurstHit);

            // world.AddCharacterEvent(new Timestamp(10.5), shogun.Skill);

            world.AddCharacterEvent(new Timestamp(11), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(11.5), xiangling.InitialBurst);

            world.AddCharacterEvent(new Timestamp(26), xiangling.InitialBurst);

            world.AddCharacterEvent(new Timestamp(35.5), xiangling.InitialBurst);

            // world.AddCharacterEvent(new Timestamp(500), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(501), xiangling.InitialBurst);
            world.AddCharacterEvent(new Timestamp(503), xiangling.BurstHit);

            world.Simulate();
        }

        static void SimulateXianglingBurst()
        {
            Xiangling xiangling = new Xiangling(0);
            Bennett bennett = new Bennett(0);

            World world = new World();
            world.SetUnits(xiangling, bennett, null, null);

            xiangling.Weapon = new DummyWeapon(WeaponType.POLEARM);
            bennett.Weapon = new DummyWeapon(WeaponType.SWORD);

            var crimsonWitch = new CrimsonWitch();
            crimsonWitch.Add2pc(world, xiangling);
            crimsonWitch.Add4pc(world, xiangling);

            world.AddCharacterEvent(new Timestamp(1), bennett.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(1.2), bennett.Burst);

            world.AddCharacterEvent(new Timestamp(1.7), xiangling.SwitchUnit);

            world.AddCharacterEvent(new Timestamp(3.8), xiangling.Skill);

            world.AddCharacterEvent(new Timestamp(5.5), bennett.SwitchUnit);

            world.AddCharacterEvent(new Timestamp(16), xiangling.SwitchUnit);

            world.AddCharacterEvent(new Timestamp(17), xiangling.Skill);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{bennett} total DMG: {world.TotalDamage[1]}");
        }

/*         static void SimulateXianglingBuffDuringBurst()
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
        } */

/*         static void SimulateXianglingBuffDuringBurstStartup()
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
        } */

/*         static void SimulateAyakaWithoutBuff()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(1, ayaka.Burst);

            world.Simulate();

            Console.WriteLine($"Ayaka without buff total damage: {world.TotalDamage}");
        } */

/*         static void SimulateAyakaWithBuff()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(0, hackBennettBuff(ayaka));
            world.AddCharacterEvent(1, ayaka.Burst);

            world.Simulate();

            Console.WriteLine($"Ayaka with buff throughout total damage: {world.TotalDamage}");
        } */

/*         static void SimulateAyakaWithBuffDuringBurst()
        {
            Ayaka ayaka = new Ayaka(0);

            World world = new World();
            world.SetUnits(ayaka, null, null, null);

            world.AddCharacterEvent(1, ayaka.Burst);
            world.AddCharacterEvent(1.1, hackBennettBuff(ayaka));

            world.Simulate();

            Console.WriteLine($"Ayaka with buff during burst total damage: {world.TotalDamage}");
        } */
    }
}
