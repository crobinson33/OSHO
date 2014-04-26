using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    /// <summary>
    /// This will take objects in and be used to trigger events.
    /// </summary>
    public class EnemyManager : Manager
    {
        // we hold the level so we can add objects and remove objects dynamically.
        public Level level;

        public Player player;


        public EnemyManager(string tag, Level level) : base(tag)
        {
            this.level = level;
            this.objectDrawable = null;

        }

        public override void Update(float deltaTime)
        {
            //base.Update(deltaTime);
        }


        public void SpawnLittleEyes(Vector2 incPosition)
        {
            Console.WriteLine("spawning little dudes");

            Enemy newEnemy = new Enemy("littleEye", incPosition, this.level.world, FindPlayer());
            level.AddObject(newEnemy);
        }

        public Player FindPlayer()
        {
            //Console.WriteLine(level.objects.Count);
            foreach (BaseObject obj in level.objects)
            {
                if (obj.tag == "one")
                {
                    return (Player)obj;
                }
            }

            return null;
        }
    }
}
