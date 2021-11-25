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
        
        public Xingqiu(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("bennett", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.HYDRO, WeaponType.SWORD)
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
            int indexShift = (ConstellationLevel >= 4 && ultActive) ? 2 : 1;
            return new List<WorldEvent>
            {
                SkillActivated(timestamp),
                new Hit(timestamp + 24.0/30, Element, indexShift, GetStatsPage, this, Types.SKILL, 
                    new HitType(true), "Fatal Rainscreen first hit"),
                new Hit(timestamp + 40.0/30, Element, 1 + indexShift, GetStatsPage, this, Types.SKILL, 
                    new HitType(true), "Fatal Rainscreen second hit")
            };
        }

        public List<WorldEvent> Burst(Timestamp timestamp)
        {

            burstWaveCount = 0;
            int duration = ConstellationLevel >= 2 ? 18 : 15;

            return new List<WorldEvent>
            {
                BurstActivated(timestamp),
                new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.NormalAttackHook += RainSword;
                    }
                }),
                new WorldEvent(timestamp + duration, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.NormalAttackHook -= RainSword;
                    }
                })
                // add events which add and remove an event listener for auto attacks
            };
        }
        
        
        // TODO: add delay between auto being used and swords hitting
        // need to do recording to see how long it is
        public void RainSword(object? sender, NormalAttackArgs e)
        {
            Timestamp timestamp = e.Timestamp;
            Timestamp duration = e.Duration;
            // kinda scuffed but when it was 1 and not 0.99 it didn't work how i wanted
            if (timestamp + duration - lastBurstWave <= 0.99)
            {
                return;
            }

            int bounces = burstWaveSwordCount[burstWaveCount];
            burstWaveCount += 1;
            lastBurstWave = timestamp;
            e.World.AddWorldEvent(new Hit(Timestamp.Max(timestamp, lastBurstWave+1), Element, 0, GetStatsPage, 
                this, Types.BURST, new HitType(bounces: bounces, delay:new Timestamp(0)), "Rain Sword"));
        }

    }
    
    
}