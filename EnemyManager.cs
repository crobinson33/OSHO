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

        public bool buttonOneDown = false;
        public bool buttonOneOver = false;
        public bool buttonTwoDown = false;
        public bool buttonTwoOver = false;
        public bool buttonThreeDown = false;
        public bool buttonThreeOver = false;
        public bool buttonFourDown = false;

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
                //PerformSpawn(deltaTime);
            }

			if (buttonOneDown && buttonOneOver == false)
			{
				if (CheckIfEnemiesStillAlive() == false)
				{
					FindObject("buttonOne").isFinished = true;
				}

				// if all enemies are dead.
				if (FindObject("buttonOne").isFinished)
				{
					Console.WriteLine ("all enemies dead, adding next button");
					Item buttonTwo = new Item("buttonTwo", "assets/Button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(1354, 1112), level.world, new Vector2(0, -32));
					buttonTwo.CreateAnimation("button", 0, 1);
					//buttonTwo.CreateAnimation("buttonDown", 1, 2);
					buttonTwo.collider.isStatic = true;
					buttonTwo.collider.mass = 100000000;
					buttonTwo.collider.AddTagToIgnore("one");
					buttonTwo.collider.AddTagToIgnore("bullet");
					level.AddObject(buttonTwo);
					level.RemoveObject(FindObject("buttonOne"));
					buttonOneOver = true;
				}
			}

			if (buttonTwoDown && buttonTwoOver == false)
			{
				if (CheckIfEnemiesStillAlive() == false)
				{
					FindObject("buttonTwo").isFinished = true;
				}

				if (FindObject("buttonTwo").isFinished)
				{
					Console.WriteLine ("all enemies dead, adding next button");
					Item buttonThree = new Item("buttonThree", "assets/Button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(1354, 1112), level.world, new Vector2(0, -32));
					buttonThree.CreateAnimation("button", 0, 1);
					//buttonThree.CreateAnimation("buttonDown", 1, 2);
					buttonThree.collider.isStatic = true;
					buttonThree.collider.mass = 100000000;
					buttonThree.collider.AddTagToIgnore("one");
					buttonThree.collider.AddTagToIgnore("bullet");
					level.AddObject(buttonThree);
					//level.RemoveObject(FindObject("buttonTwo"));
					buttonTwoOver = true;
				}
			}

			if (buttonThreeDown && buttonThreeOver == false)
			{
				if (CheckIfEnemiesStillAlive() == false)
				{
					FindObject("buttonThree").isFinished = true;
				}
				
				if (FindObject("buttonThree").isFinished)
				{
					Console.WriteLine ("all enemies dead, adding next button");
					Item buttonFour = new Item("buttonFour", "assets/Button.png", new Vector2(64, 64), new Vector2(64, 32), new Vector2(1354, 1112), level.world, new Vector2(0, -32));
					buttonFour.CreateAnimation("button", 0, 1);
					//buttonFour.CreateAnimation("buttonDown", 1, 2);
					buttonFour.collider.isStatic = true;
					buttonFour.collider.mass = 100000000;
					buttonFour.collider.AddTagToIgnore("one");
					buttonFour.collider.AddTagToIgnore("bullet");
					level.AddObject(buttonFour);
					//level.RemoveObject(FindObject("buttonTwo"));
					buttonThreeOver = true;
				}
			}
        }

		public Item FindObject(string tag)
		{
			foreach (VisibleObject obj in level.objects)
			{
				//Console.WriteLine (obj.tag);
				if (obj.tag == tag)
				{
					return (Item)obj;
				}
			}
			return null;
		}

		public bool CheckIfEnemiesStillAlive()
		{
			int counter = 0;
			foreach (VisibleObject obj in level.objects)
			{
				if (obj.isAlive)
				{
					if (obj.tag != "one")
					{
						//Console.WriteLine (obj.tag);
						counter++;
					}
				}
			}

			Console.WriteLine ("counter: " + counter);
			if (counter > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//spawn event
        public void ButtonOneDown()
        {
            if (buttonOneDown != true)
            {
                FindPlayer();
                /*Console.WriteLine("added eye");
                BigEyeEnemy eyeEnemy = new BigEyeEnemy("bigEye", this.player.position, level.world, this.player, level.camera, this);
                this.level.AddObject(eyeEnemy);*/
                buttonOneDown = true;

				FindObject("buttonOne").CreateAnimation("button", 1, 2);


				int enemies = 3;
				int radius = 400;
				double slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);

					BigEyeEnemy eyeEnemy = new BigEyeEnemy("bigEye", target, level.world, this.player, level.camera, this);
					this.level.AddObject(eyeEnemy);
				}

				enemies = 25;
				radius = 300;
				slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					Enemy newEnemy = new Enemy("littleEye", target, this.level.world, FindPlayer());
					level.AddObject(newEnemy);
				}
            }
        }

		//spawn event
        public void ButtonTwoDown()
        {
            if (buttonTwoDown != true)
            {
                FindPlayer();
                Console.WriteLine("added skelly");
                //SkellyEnemy skellyEnemy = new SkellyEnemy("skelly", this.player.position, level.world, player, level.camera, this);
                //this.level.AddObject(skellyEnemy);
                buttonTwoDown = true;
				FindObject("buttonTwo").CreateAnimation("button", 1, 2);

				int enemies = 2;
				int radius = 400;
				double slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					SkellyEnemy skellyEnemy = new SkellyEnemy("skelly", target, level.world, player, level.camera, this);
					this.level.AddObject(skellyEnemy);
				}
            }
        }

		//spawn event
        public void ButtonThreeDown()
        {
            if (buttonThreeDown != true)
            {
                FindPlayer();
                Console.WriteLine("added ghost");
                //GhostEnemy ghostEnemy = new GhostEnemy("ghost", this.player.position, level.world, player, level.camera, this);
                //this.level.AddObject(ghostEnemy);
                buttonThreeDown = true;
				FindObject("buttonThree").CreateAnimation("button", 1, 2);

				int enemies = 3;
				int radius = 400;
				double slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					GhostEnemy ghostEnemy = new GhostEnemy("ghost", target, level.world, player, level.camera, this);
					this.level.AddObject(ghostEnemy);
				}
            }
        }

		//spawn event
		public void ButtonFourDown()
		{
			if (buttonFourDown != true)
			{
				FindPlayer();
				Console.WriteLine("added ghost");
				//GhostEnemy ghostEnemy = new GhostEnemy("ghost", this.player.position, level.world, player, level.camera, this);
				//this.level.AddObject(ghostEnemy);
				buttonFourDown = true;
				FindObject("buttonFour").CreateAnimation("button", 1, 2);

				int enemies = 3;
				int radius = 400;
				double slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					BigEyeEnemy eyeEnemy = new BigEyeEnemy("bigEye", target, level.world, this.player, level.camera, this);
					this.level.AddObject(eyeEnemy);
				}
				
				enemies = 25;
				radius = 300;
				slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					Enemy newEnemy = new Enemy("littleEye", target, this.level.world, FindPlayer());
					level.AddObject(newEnemy);
				}

				enemies = 2;
				radius = 400;
				slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					SkellyEnemy skellyEnemy = new SkellyEnemy("skelly", target, level.world, player, level.camera, this);
					this.level.AddObject(skellyEnemy);
				}
				
				enemies = 3;
				radius = 400;
				slice = 2 * Math.PI / enemies;
				for (int i = 0; i < enemies; i++)
				{
					double angle = slice * i;
					//Console.WriteLine("angle: " + angle);
					float x = (float)((this.player.position.X) + radius * Math.Cos(angle));
					float y = (float)((this.player.position.Y) + radius * Math.Sin(angle));
					Vector2 target = new Vector2(x, y);
					//Console.WriteLine(target);
					//Fire(target);
					
					GhostEnemy ghostEnemy = new GhostEnemy("ghost", target, level.world, player, level.camera, this);
					this.level.AddObject(ghostEnemy);
				}
			}
		}

		public BigEyeEnemy FindBigEye(BigEyeEnemy enemy)
		{
			foreach (VisibleObject obj in level.objects)
			{
				if (enemy == obj)
				{
					return (BigEyeEnemy)obj;
				}
			}
			return null;
		}

        /*public void PerformSpawn(float deltaTime)
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
        }*/


        public void SpawnLittleEyes(BigEyeEnemy enemy)
        {
            Console.WriteLine("spawning little dudes");
            //this.spawnPosition = incPosition;
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
