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

        bool buttonOneDown = false;
        bool buttonTwoDown = false;
        bool buttonThreeDown = false;

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

        public void ButtonOneDown()
        {
            if (buttonOneDown != true)
            {
                FindPlayer();
                Console.WriteLine("added eye");
                BigEyeEnemy eyeEnemy = new BigEyeEnemy("bigEye", new Vector2(100, 100), level.world, this.player, level.camera, this);
                this.level.AddObject(eyeEnemy);
                buttonOneDown = true;
            }
        }

        public void ButtonTwoDown()
        {
            if (buttonTwoDown != true)
            {
                FindPlayer();
                Console.WriteLine("added skelly");
                SkellyEnemy skellyEnemy = new SkellyEnemy("skelly", new Vector2(200, 100), level.world, player, level.camera, this);
                this.level.AddObject(skellyEnemy);
                buttonTwoDown = true;
            }
        }

        public void ButtonThreeDown()
        {
            if (buttonThreeDown != true)
            {
                FindPlayer();
                Console.WriteLine("added ghost");
                GhostEnemy ghostEnemy = new GhostEnemy("ghost", new Vector2(300, 100), level.world, player, level.camera, this);
                this.level.AddObject(ghostEnemy);
                buttonThreeDown = true;
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
