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

        //local
        public bool spawnFies;
        private Vector2 spawnPosition;
        private TimeSpan duration = new TimeSpan(0, 0, 0, 0, 500);
        private TimeSpan accum = new TimeSpan();
        private TimeSpan overAllDuration = new TimeSpan(0, 0, 10);
        private TimeSpan overAllAccum = new TimeSpan();

        public EnemyManager(string tag, Level level) : base(tag)
        {
            this.level = level;
        }

        public override void Update(float deltaTime)
        {
            //base.Update(deltaTime);

            if (spawnFies)
            {
                //Console.WriteLine("checking flies");
                PerformSpawn(deltaTime);
            }
        }

        public void PerformSpawn(float deltaTime)
        {
            //Console.WriteLine(overAllDuration + ", " + overAllAccum);
            if (overAllDuration > overAllAccum)
            {
                //Console.WriteLine("checking spawn time");
                if (duration < accum)
                {
                    //Console.WriteLine("spawning fly");
                    
                    // time to spawn. only want to do one. that why it is here
                    Enemy newEnemy = new Enemy("littleEye", this.spawnPosition, this.level.world, FindPlayer());
                    level.AddObject(newEnemy);

                    //reset
                    accum = new TimeSpan();
                }
                else
                {
                    // its not time to spawn yet.
                    accum += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
                }
            }
            else
            {
                Console.WriteLine("ending little dudes");
                // we have reached end of duration
                spawnFies = false;
            }

            overAllAccum += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
        }


        public void SpawnLittleEyes(Vector2 incPosition)
        {
            Console.WriteLine("spawning little dudes");
            this.spawnPosition = incPosition;
            this.spawnFies = true;
            overAllAccum = new TimeSpan();

            //for (int i = 0; i < 50; i++)
            //{
            //    Enemy newEnemy = new Enemy("littleEye", incPosition, this.level.world, FindPlayer());
            //    level.AddObject(newEnemy);
            //}
        }

        public Player FindPlayer()
        {
            //Console.WriteLine(level.objects.Count);
            foreach (BaseObject obj in level.objects)
            {
                if (obj.tag == "one")
                {
                    this.player = (Player)obj;
                    return (Player)obj;
                }
            }

            return null;
        }
    }
}
