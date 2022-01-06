#nullable enable
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using Tcc.buffs;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Xingqiu: Unit
    {
        private double lastBurstWave = -1;
        private bool ultActive = false;
        private int burstWaveCount = 0;
        private int[] burstWaveSwordCount;
        private readonly HitType SkillHitType;

        private static Guid c2guid = Guid.NewGuid();



        public Xingqiu(int constellationLevel=0, string level="90", int autoLevel=6, int skillLevel=6, int burstLevel=6): 
            base("xingqiu", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.HYDRO, WeaponType.SWORD)
        {
            burstWaveSwordCount = constellationLevel != 6 ? new[] {2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3} 
                : new[] {2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5, 2, 3, 5};
            AutoAttackFrameData = new[] {9, 34, 59, 78, 116, 160, 195, 63};
            BurstICD = new();
            SkillHitType = new HitType(Element.HYDRO);
            AutoAttackHits = new[] {1, 1, 2, 1, 2};
        }

        public override void Reset()
        {
            ultActive = false;
            lastBurstWave = -1;
            base.Reset();
        }


        // TODO: frame data for xq e was done at 30 fps apparently since my pc sucks, need to cross check results
        public override List<WorldEvent> Skill(double timestamp, params object[] p)
        {
            // TODO: make c4 work
            //double multiplier = (constellationLevel >= 4 && ultActive)? 1.5 : 1;
            int indexShift = (ConstellationLevel >= 4 && ultActive) ? 2 : 1;
            return new List<WorldEvent>
            {
                SkillActivated(timestamp),
                new Hit(timestamp + 24.0/30, indexShift, GetStatsPage, this, Types.SKILL, 
                    SkillHitType, "Fatal Rainscreen first hit"),
                new Hit(timestamp + 40.0/30, 1 + indexShift, GetStatsPage, this, Types.SKILL, 
                    SkillHitType, "Fatal Rainscreen second hit")
            };
        }

        public override List<WorldEvent> Burst(double timestamp)
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
            double timestamp = e.Timestamp;
            double duration = e.Duration;
            // kinda scuffed but when it was 1 and not 0.99 it didn't work how i wanted
            if (timestamp + duration - lastBurstWave <= 0.99)
            {
                return;
            }

            int bounces = burstWaveSwordCount[burstWaveCount];
            burstWaveCount += 1;
            double newWave = Math.Max(timestamp, lastBurstWave + 1);
            lastBurstWave = newWave;
            // check how many frames swords take to hit, 29 frames is filler
            // TODO: maybe make HitType for this but i would need 3 or to rewrite the class
            switch (ConstellationLevel)
            {
                case < 2:
                    e.World.AddWorldEvent(new Hit(newWave + 29/60.0, 0, GetStatsPage, this, Types.BURST, 
                        new HitType(Element.HYDRO, bounces: bounces, delay: 0), "Rain Sword"));
                    break;
                default:
                    e.World.AddWorldEvent(new Hit(newWave + 29/60.0, 0, GetStatsPage, this, Types.BURST, 
                        new HitType(Element.HYDRO, bounces: bounces, delay: 0), "Rain Sword", 
                        debuff: new RefreshableBuff<FirstPassModifier>(c2guid, newWave + 269/60.0, 
                            _ => (Stats.HydroResistance, -0.15), 1)));
                    break;
            }

        }

    }
    
    
}