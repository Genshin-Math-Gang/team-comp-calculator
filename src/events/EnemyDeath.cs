using Tcc.enemy;

namespace Tcc.events
{
    public class EnemyDeath: WorldEvent
    {
        public EnemyDeath(Enemy enemy, Timestamp timestamp) : 
            base(timestamp, world => world.EnemyDeath(timestamp, enemy), priority: 0)
        {
            // this should work since it uses object equality 
            // remove might be slow since its linear but it shouldn't matter
        }
    }
}