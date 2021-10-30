using System;
using System.Collections.Generic;
using Tcc;
using Tcc.Elements;
using Tcc.Events;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Xingqiu: Unit
    {
        private static readonly ICDCreator AutoICD = new ICDCreator("a987d543-8542-455d-8121-331cdabc43b5");
        private static readonly ICDCreator BurstICD = new ICDCreator("73afe8d9-055e-4f11-94d6-6764815573f0");
        private Timestamp lastBurstWave = new Timestamp(-1);
        private bool ultActive = false;
        private int burstWaveCount = 0;
        private int[] burstWaveSwordCount;
        
        public Xingqiu(int constellationLevel) :
            base(constellationLevel: constellationLevel,
                element: Element.HYDRO,
                burstEnergyCost: 80,
                weaponType: WeaponType.SWORD,
                capacityStats: new CapacityStats(
                    baseHp: 10222,
                    energy: 80
                ),
                generalStats: new GeneralStats(
                    baseAttack: 202,
                    baseDefence: 758,
                    attackPercent: 0.466 + 0.12,
                    flatAttack: 311,
                    critRate: 0.5,
                    critDamage: 1
                ),
                normal: new AbilityStats(motionValues: new double[] {0.9214, 0.9418, 1.0795, 1.11798, 1.3209}),
                // TODO: i really don't like having multiple hits being added together for MV, need to change at some point
                charged: new AbilityStats(motionValues: new double[] {.935 + 1.1101}),
                plunge: new AbilityStats(motionValues: new double[] {1.2638, 2.527, 3.1564}),
                skill: new AbilityStats(motionValues: new double[] {3.024, 3.4416, 3.024 * 1.5, 3.4416 * 1.5}),
                burst: new AbilityStats(motionValues: new double[] {.9769}))
        {
            burstWaveSwordCount = constellationLevel != 6 ? new[] {2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3} 
                : new[] {2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5};
        }
        
        
        public List<WorldEvent> NormalAttack(Timestamp timestamp, params object[] param)
        {
            var normalIndex = (int) param[1];
            return new List<WorldEvent>()
            {
                new Hit(timestamp, Element.PHYSICAL, normalIndex, GetStatsPage, this, Types.NORMAL, 
                    new HitType(false, 1, false, icd: AutoICD), $"xq normal {normalIndex}"),
                // TODO: find frame data for auto attacks
                NormalAttackGeneralUsed(timestamp, new Timestamp(0.2))

            };
        }
        
        // TODO: frame data for xq e was done at 30 fps apparently since my pc sucks, need to cross check results
        public List<WorldEvent> Skill(Timestamp timestamp)
        {

            //double multiplier = (constellationLevel >= 4 && ultActive)? 1.5 : 1;
            int indexShift = (constellationLevel >= 4 && ultActive) ? 2 : 1;
            return new List<WorldEvent>
            {
                SkillActivated(timestamp),
                new Hit(timestamp + 24.0/30, element, indexShift, GetStatsPage, this, Types.SKILL, 
                    new HitType(true), "Fatal Rainscreen first hit"),
                new Hit(timestamp + 40.0/30, element, 1 + indexShift, GetStatsPage, this, Types.SKILL, 
                    new HitType(true), "Fatal Rainscreen second hit")
            };
        }

        public List<WorldEvent> Burst(Timestamp timestamp)
        {

            burstWaveCount = 0;
            int duration = constellationLevel >= 2 ? 18 : 15;

            return new List<WorldEvent>
            {
                BurstActivated(timestamp),
                new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.normalAttackHook += RainSword;
                    }
                }),
                new WorldEvent(timestamp + duration, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.normalAttackHook -= RainSword;
                    }
                })
                // add events which add and remove an event listener for auto attacks
            };
        }
        
    
        // TODO: add logic so that if the animation overlaps with the icd then it triggers asap
        public void RainSword(object? sender, NormalAttackArgs e)
        {
            Timestamp timestamp = e.Timestamp;
            Timestamp duration = e.Duration;
            if (timestamp - lastBurstWave <= 0.99)
            {
                return;
            }

            int bounces = burstWaveSwordCount[burstWaveCount];
            burstWaveCount += 1;
            lastBurstWave = timestamp;
            e.World.AddWorldEvent(new Hit(timestamp, element, 0, GetStatsPage, this, Types.BURST,
                new HitType(bounces: bounces, delay:new Timestamp(0)), "Rain Sword"));
        }

    }
    
    
}