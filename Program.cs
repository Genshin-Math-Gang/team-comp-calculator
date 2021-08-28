using System.Collections.Generic;
using System;
using Tcc.Buffs;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Units;

namespace Tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine("\n\nSimulating Xiangling's elmental skill (Guoba) and Bennett's global buff from his elemental burst (Fantastic Voyage)");
            //Simulate_Xiangling_Guoba_With_Bennett_Snapshot();

            //System.Console.WriteLine("\n\nSimulating Xiangling's elemental burst (Pyronado) and Shogun's elemental burst buff from her elemental skill (Eye of Stormy Judgement)");
            //Simulate_Xiangling_Burst_With_Shogun_Snapshot();

            System.Console.WriteLine("\n\nSimulating Xiangling's elemental burst (Pyronado) and Shogun's elemental skill buff from her lemental skill (Eye of Stormy Judgement) and Bennett's global buff from his elemental burst (Fantastic Voyage)");
            Simulate_Xiangling_burst_with_Shogun_And_Bennett_Snapshot();

            //Shogun_Double_Buff();
        }

        static void Simulate_Xiangling_Burst_With_Shogun_Snapshot()
        {
            Shogun shogun = new Shogun(0);
            Xiangling xiangling = new Xiangling(0);

            World world = new World();
            world.SetUnits(shogun, xiangling, null, null);

            //Use Shogun skill
            world.AddCharacterEvent(new Timestamp(0), shogun.Skill);

            //Switch to Xiangling cast burst
            world.AddCharacterEvent(new Timestamp(1), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(1.5), xiangling.InitialBurst);
            world.AddCharacterEvent(new Timestamp(3), xiangling.BurstHit);

            //Switch off Xiangling
            world.AddCharacterEvent(new Timestamp(4), shogun.SwitchUnit);
    
            //Buff Shogun
            world.AddCharacterEvent(new Timestamp(4), (timestamp) => new List<WorldEvent> {
                new WorldEvent(timestamp, (world) => world.AddBuff(timestamp, shogun, new BasicBuffFromUnit(new Guid("e6a06a2f-7d42-437a-b330-fb08b79d5045"), new Stats.Stats(energyRecharge: 0.5)), "test buff"))
            });

            //Xiangling burst hit while off-field and post shogun buff
            world.AddCharacterEvent(new Timestamp(4.5), xiangling.BurstHit);


            //Return to Xiangling and cast burst
            world.AddCharacterEvent(new Timestamp(11), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(11.5), xiangling.InitialBurst);

            //Cast burst again after Shogun buff expired;
            world.AddCharacterEvent(new Timestamp(501), xiangling.InitialBurst);
            world.AddCharacterEvent(new Timestamp(503), xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[1]}");
            Console.WriteLine($"{shogun} total DMG: {world.TotalDamage[0]}");
        }

        static void Simulate_Xiangling_Guoba_With_Bennett_Snapshot()
        {
            Xiangling xiangling = new Xiangling(0);
            Bennett bennett = new Bennett(0);

            World world = new World();
            world.SetUnits(xiangling, bennett, null, null);

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

        static void Simulate_Xiangling_burst_with_Shogun_And_Bennett_Snapshot()
        {
            Xiangling xiangling = new Xiangling(0);
            Bennett bennett = new Bennett(0);
            Shogun shogun = new Shogun(0);

            World world = new World();
            world.SetUnits(xiangling, bennett, shogun, null);

            //Switch to shogun and cast skill
            world.AddCharacterEvent(new Timestamp(1), shogun.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(1.5), shogun.Skill);

            //Switch to Xiangling and cast burst
            world.AddCharacterEvent(new Timestamp (1.8), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(2), xiangling.InitialBurst);

            //Switch to Bennett and cast burst
            world.AddCharacterEvent(new Timestamp(4), bennett.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(4.5), bennett.Burst);

            //Xiangling burst hit off-field
            world.AddCharacterEvent(new Timestamp(5), xiangling.BurstHit);

            //Switch to Xiangling and hit burst
            world.AddCharacterEvent(new Timestamp(5.4), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(5.5), xiangling.BurstHit);

            //Cast Bennett burst a lot later after all buffs expired
            world.AddCharacterEvent(new Timestamp(399), bennett.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(400), bennett.Burst);

            //Cast shogun E
            world.AddCharacterEvent(new Timestamp(401), shogun.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(402), shogun.Skill);

            //Cast xiangling nado
            world.AddCharacterEvent(new Timestamp(403), xiangling.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(404), xiangling.InitialBurst);
            world.AddCharacterEvent(new Timestamp(407), xiangling.BurstHit);

            //Switch off Xiangling and her nado hits off-field
            world.AddCharacterEvent(new Timestamp(408), bennett.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(409), xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{bennett} total DMG: {world.TotalDamage[1]}");
            Console.WriteLine($"{shogun} total DMG: {world.TotalDamage[2]}");
        }

        static void Shogun_Double_Buff()
        {
            Xiangling xiangling = new Xiangling(0);
            Shogun shogun = new Shogun(0);

            World world = new World();
            world.SetUnits(xiangling, shogun, null, null);

            world.AddCharacterEvent(new Timestamp(0), shogun.Skill);
            world.AddCharacterEvent(new Timestamp(1), xiangling.SwitchUnit);

            world.AddCharacterEvent(new Timestamp(1.5), xiangling.InitialBurst);
            world.AddCharacterEvent(new Timestamp(2), shogun.SwitchUnit);
            world.AddCharacterEvent(new Timestamp(2.5), xiangling.SwitchUnit);

            world.AddCharacterEvent(new Timestamp(7), xiangling.InitialBurst);

            world.Simulate();
        }
    }
}
