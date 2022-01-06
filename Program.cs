using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using Tcc.artifacts;
using Tcc.buffs;
using Tcc.enemy;
using Tcc.events;
using Tcc.stats;
using Tcc.units;
using static System.Threading.Tasks.Parallel;

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

            //System.Console.WriteLine("\n\nSimulating Xiangling's elemental burst (Pyronado) and Shogun's elemental skill buff from her elemental skill (Eye of Stormy Judgement) and Bennett's global buff from his elemental burst (Fantastic Voyage)");
            //Simulate_Xiangling_burst_with_Shogun_And_Bennett_Snapshot();
            
            //Ganyu_Test();
            
            //Sucrose_Test();
            //Noblesse_Test();
            Xingqiu_Test();
        }

        
        static void Xingqiu_Test()
        {
            /*var watch = new Stopwatch();
            watch.Start();
            int limit = (int) Math.Pow(10, 5);
            double[] totalDamage = [limit];
            Bennett benentt = new Bennett();
            Xingqiu xq = new Xingqiu();
            Parallel.For(0, limit, i =>
            {
                Bennett benentt = new Bennett();
                Xingqiu xq = new Xingqiu();
                xq.ArtifactStats = new ArtifactStats(new Flower(), new Feather(),
                    new Sands(Stats.AtkPercent), new Goblet(Stats.HydroDamageBonus), new Circlet(Stats.CritRate));
                List<Enemy> enemies = new List<Enemy>();
                enemies.Add(new Enemy());
                World world = new World(enemies);
                world.SetUnits(xq, benentt, null, null);

                world.AddCharacterEvent((0), xq.Burst);
                world.AddCharacterEvent((1), time => xq.AutoAttack(time, AutoString.N1));
                world.AddCharacterEvent((1.5), benentt.SwitchUnit);
                world.AddCharacterEvent((2), time => benentt.AutoAttack(time, AutoString.N1));
                world.AddCharacterEvent((2.5), time => benentt.AutoAttack(time, AutoString.N1));
                world.AddCharacterEvent((3), time => benentt.AutoAttack(time, AutoString.N1));
                world.AddCharacterEvent((14), time => benentt.AutoAttack(time, AutoString.N1));
                world.AddCharacterEvent((16), time => benentt.AutoAttack(time, AutoString.N1));
                world.Simulate();
                totalDamage[i] = world.TotalDamage[0] + world.TotalDamage[1];
            }); 
            Console.WriteLine(totalDamage.Average());
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);*/
            var watch = new Stopwatch();
            int limit = (int) Math.Pow(10, 3);
            double[] totalDamage = new double[limit];
            ActionList actionList = new ActionList(
                new (Character.Xingqiu, cons:2),
                Character.Bennett,
                Character.Xiangling,
                Character.Sucrose, new List<Action>
                {
                    new (0, 0, ActionType.Burst),
                    new (0, 1, ActionType.Normal, new object[]{AutoString.N1}),
                    new (1, 1.5, ActionType.Swap),
                    new (1, 2, ActionType.Normal, new object[]{AutoString.N1}),
                    new (1, 2.5, ActionType.Normal, new object[]{AutoString.N1}),
                    new (1, 3, ActionType.Normal, new object[]{AutoString.N1}),
                    new (1, 13, ActionType.Normal, new object[]{AutoString.N1}),
                    new (1, 14, ActionType.Normal, new object[]{AutoString.N1})
                });
            watch.Start();
            For<World>(0, limit, () => new World(actionList), (i, loop, world) =>
                {
                    world.Simulate();
                    totalDamage[i] = world.TotalDamage[0] + world.TotalDamage[1];
                    world.Reset();
                    return world;
                },
                (x) => {}
                );
            Console.WriteLine(totalDamage.Average());
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        static void Sucrose_Test()
        {
            Sucrose sucrose = new Sucrose();
            Bennett benentt = new Bennett(0);
            
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            enemies.Add(new Enemy());
            //enemies.Add(new Enemy.Enemy());

            World world = new World(enemies);
            world.SetUnits(sucrose, benentt, null, null);

            var vv = new ViridescentVenerer();
            vv.Add2pc(world, sucrose);
            vv.Add4pc(world, sucrose);
            
            world.AddCharacterEvent((0), benentt.Burst);
            world.AddCharacterEvent((1), sucrose.Burst);
            world.AddCharacterEvent((1.5), sucrose.Skill);

            world.Simulate();
            
            Console.WriteLine($"{sucrose} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{benentt} total DMG: {world.TotalDamage[1]}");


        }


        static void Noblesse_Test()
        {
            Sucrose sucrose = new Sucrose(0);
            Bennett bennett = new Bennett(0);
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            World world = new World(enemies);
            world.SetUnits(sucrose, bennett, null, null);
            var noblesse = new NoblessOblige();
            noblesse.Add4pc(world, sucrose);
            
            world.AddCharacterEvent((.5), sucrose.Skill);
            world.AddCharacterEvent((1), sucrose.Burst);
            world.AddCharacterEvent((1.5), sucrose.Skill);
            
            world.Simulate();
        }
        static void Ganyu_Test()
        {
            Ganyu ganyu = new Ganyu(0);
            Bennett benentt = new Bennett(0);
            
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            
            World world = new World(enemies);
            world.SetUnits(ganyu, benentt, null, null);

            var wanderer = new WanderersTroupe();
            var shime = new ShimenawasReminiscence();
            //wanderer.Add2pc(world, ganyu);
            //wanderer.Add4pc(world, ganyu);
            shime.Add2pc(world, ganyu);
            shime.Add4pc(world, ganyu);
            
            world.AddCharacterEvent((0), benentt.Burst);
            
            world.AddCharacterEvent((1), ganyu.SwitchUnit);
            
            world.AddCharacterEvent((1.5), ganyu.Skill);
            world.AddCharacterEvent((2), ganyu.ChargedAttack, 3);
            world.AddCharacterEvent((19), ganyu.Skill);
            world.AddCharacterEvent((20), ganyu.ChargedAttack, 3);
            world.AddCharacterEvent((30), ganyu.ChargedAttack, 3);
            
            world.AddCharacterEvent((41), benentt.SwitchUnit);
            
            world.AddCharacterEvent((42), benentt.Burst);
            
            world.AddCharacterEvent((43), ganyu.SwitchUnit);
            
            world.AddCharacterEvent((44), ganyu.Burst);

            world.Simulate();
            
            Console.WriteLine($"{ganyu} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{benentt} total DMG: {world.TotalDamage[1]}");
            
        }
        static void Simulate_Xiangling_Burst_With_Shogun_Snapshot()
        {
            Raiden shogun = new Raiden();
            Xiangling xiangling = new Xiangling(0);
            
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());

            World world = new World(enemies);
            world.SetUnits(shogun, xiangling, null, null);

            var crimsonWitch = new CrimsonWitch();
            crimsonWitch.Add2pc(world, xiangling);
            crimsonWitch.Add4pc(world, xiangling);

            //Use Shogun skill
            world.AddCharacterEvent((0), shogun.Skill);

            //Switch to Xiangling cast burst
            world.AddCharacterEvent((1), xiangling.SwitchUnit);
            world.AddCharacterEvent((1.5), xiangling.Burst);
            world.AddCharacterEvent((3), xiangling.BurstHit);

            //Switch off Xiangling
            world.AddCharacterEvent((4), shogun.SwitchUnit);
    
            //Buff Shogun
            world.AddCharacterEvent((4), (timestamp) => new List<WorldEvent> {
                new WorldEvent(timestamp, (world) => shogun.AddBuff(new PermanentBuff<FirstPassModifier>(
                    new Guid("e6a06a2f-7d42-437a-b330-fb08b79d5045"), (_) => (Stats.EnergyRecharge, 0.5))))
            });

            //Xiangling burst hit while off-field and post shogun buff
            world.AddCharacterEvent((4.5), xiangling.BurstHit);


            //Return to Xiangling and cast burst
            world.AddCharacterEvent((11), xiangling.SwitchUnit);
            world.AddCharacterEvent((11.5), xiangling.Burst);

            //Cast burst again after Shogun buff expired;
            world.AddCharacterEvent((501), xiangling.Burst);
            world.AddCharacterEvent((503), xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[1]}");
            Console.WriteLine($"{shogun} total DMG: {world.TotalDamage[0]}");
        }

        static void Simulate_Xiangling_Guoba_With_Bennett_Snapshot()
        {
            Xiangling xiangling = new Xiangling(0);
            Bennett bennett = new Bennett(0);

            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());

            World world = new World(enemies);
            world.SetUnits(xiangling, bennett, null, null);

            world.AddCharacterEvent((1), bennett.SwitchUnit);
            world.AddCharacterEvent((1.2), bennett.Burst);

            world.AddCharacterEvent((1.7), xiangling.SwitchUnit);

            world.AddCharacterEvent((3.8), xiangling.Skill);

            world.AddCharacterEvent((5.5), bennett.SwitchUnit);

            world.AddCharacterEvent((16), xiangling.SwitchUnit);

            world.AddCharacterEvent((17), xiangling.Skill);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{bennett} total DMG: {world.TotalDamage[1]}");
        }

        static void Simulate_Xiangling_burst_with_Shogun_And_Bennett_Snapshot()
        {
            Xiangling xiangling = new Xiangling(0);
            Bennett bennett = new Bennett(0);
            Raiden shogun = new Raiden(0);

            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy());

            World world = new World(enemies);
            world.SetUnits(xiangling, bennett, shogun, null);

            //Switch to shogun and cast skill
            world.AddCharacterEvent((1), shogun.SwitchUnit);
            world.AddCharacterEvent((1.5), shogun.Skill);

            //Switch to Xiangling and cast burst
            world.AddCharacterEvent( (1.8), xiangling.SwitchUnit);
            world.AddCharacterEvent((2), xiangling.Burst);

            //Switch to Bennett and cast burst
            world.AddCharacterEvent((4), bennett.SwitchUnit);
            world.AddCharacterEvent((4.5), bennett.Burst);

            //Xiangling burst hit off-field
            world.AddCharacterEvent((5), xiangling.BurstHit);

            //Switch to Xiangling and hit burst
            world.AddCharacterEvent((5.4), xiangling.SwitchUnit);
            world.AddCharacterEvent((5.5), xiangling.BurstHit);

            //Cast Bennett burst a lot later after all buffs expired
            world.AddCharacterEvent((399), bennett.SwitchUnit);
            world.AddCharacterEvent((400), bennett.Burst);

            //Cast shogun E
            world.AddCharacterEvent((401), shogun.SwitchUnit);
            world.AddCharacterEvent((402), shogun.Skill);

            //Cast xiangling nado
            world.AddCharacterEvent((403), xiangling.SwitchUnit);
            world.AddCharacterEvent((404), xiangling.Burst);
            world.AddCharacterEvent((407), xiangling.BurstHit);

            //Switch off Xiangling and her nado hits off-field
            world.AddCharacterEvent((408), bennett.SwitchUnit);
            world.AddCharacterEvent((409), xiangling.BurstHit);

            world.Simulate();

            Console.WriteLine($"{xiangling} total DMG: {world.TotalDamage[0]}");
            Console.WriteLine($"{bennett} total DMG: {world.TotalDamage[1]}");
            Console.WriteLine($"{shogun} total DMG: {world.TotalDamage[2]}");
        }
    }
}
