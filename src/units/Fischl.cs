using System;
using System.Collections.Generic;
using Tcc.elements;
using Tcc.events;
using Tcc.stats;
using Tcc.weapons;

namespace Tcc.units
{
    public class Fischl: Unit
    {
        private SnapshottedStats skillSnapshot;
        private readonly HitType skillType, summoningType, a4Type;
        private Timestamp lastA4 = -1;
        public Fischl(int constellationLevel = 0, string level = "90", int autoLevel = 6, int skillLevel = 6,
            int burstLevel = 6) :
            base("fischl", level, constellationLevel, autoLevel, skillLevel, burstLevel, Element.ELECTRO,
                WeaponType.BOW)
        {
            SkillICD = new ICDCreator(5, 4);
            summoningType = new HitType(Element.ELECTRO, icd: new ICDCreator(0,0));
            a4Type = new HitType(Element.ELECTRO, false, icd: new ICDCreator(0,0));
            skillType = new HitType(Element.ELECTRO, false, icd: SkillICD);
        }

        public override List<WorldEvent> Skill(Timestamp timestamp, params object[] p)
        {
            List<WorldEvent> events = new List<WorldEvent> {SkillActivated(timestamp)};
            int hits = ConstellationLevel < 6 ? 10 : 12;
            // add some delay here
            events.Add(new Hit(timestamp, 0, skillSnapshot.GetStats, this, Types.SKILL,
                summoningType, "Oz summon"));
            
            for (int i = 0; i < hits; i++)
            {
                events.Add(new Hit(timestamp+ i, 1, skillSnapshot.GetStats, this, Types.SKILL,
                    skillType, "Oz"));
            }
            // scuffed, need to account for ascension 
            if (Level > 60)
            {
                events.Add(new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.TriggeredReactionHook += A4;
                    }
                }));
                events.Add(new WorldEvent(timestamp + hits, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.TriggeredReactionHook += A4;
                    }
                }));
            }

            if (ConstellationLevel == 6)
            {
                events.Add(new WorldEvent(timestamp, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.NormalAttackHook += C6;
                    }
                }));
                events.Add(new WorldEvent(timestamp + hits, world =>
                {
                    foreach (var unit in world.GetUnits())
                    {
                        if (unit is not null) unit.NormalAttackHook += C6;
                    }
                }));
            }
            return events;
        }

        public override List<WorldEvent> Burst(Timestamp timestamp)
        {
            throw new NotImplementedException();
        }

        public void A4(object? sender, (Timestamp timestamp, Reaction reaction, World world) tuple)
        {
            if (tuple.timestamp - lastA4 > 0.5)
            {
                return;
            }
            lastA4 = tuple.timestamp;
            tuple.world.AddWorldEvent(new Hit(tuple.timestamp, 1,skillSnapshot.GetStats, this, Types.SKILL, 
                a4Type, "Fischl a4"));
        }

        public void C6(object? sender, NormalAttackArgs e)
        {
            // TODO: only hits once on normal attacks with multiple hits, like xiangling
            Timestamp timestamp = e.Timestamp;
            e.World.AddWorldEvent(new Hit(timestamp, 2, skillSnapshot.GetStats, this, Types.SKILL, 
                skillType, "Fischl c6"));
        }
    }
}