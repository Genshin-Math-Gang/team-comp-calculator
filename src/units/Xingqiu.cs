using Tcc.Elements;
using Tcc.Stats;
using Tcc.Weapons;

namespace Tcc.Units
{
    public class Xingqiu: Unit
    {
        private static readonly ICDCreator AutoICD = new ICDCreator("a987d543-8542-455d-8121-331cdabc43b5");
        private static readonly ICDCreator BurstICD = new ICDCreator("73afe8d9-055e-4f11-94d6-6764815573f0");
        
        public Xingqiu(int constellationLevel):
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
                    attackPercent: 0.466+0.12,
                    flatAttack: 311,
                    critRate: 0.5,
                    critDamage: 1
                ),
                normal: new AbilityStats(motionValues: new double[] {0.9214,0.9418,1.0795,1.11798,1.3209}),
                charged: new AbilityStats(motionValues: new double[] {1.105+1.202}),
                plunge: new AbilityStats(motionValues: new double[] {1.2638,2.527,3.1564}),
                skill: new AbilityStats(motionValues: new double[] {2.4768,1.512+1.656,1.584+1.728,2.376}, gaugeStrength:2),
                burst: new AbilityStats(motionValues: new double[] {4.1904,0.108,1.008}, gaugeStrength:2)) {}
    }
}