using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class GhostEnemy : VisibleObject
    {
        //physics stuff
        World world;
        public BoxCollider collider;
        Vector2 colliderOffset;

        //drawing stuff
        public Texture texture;
        public MultiDrawable enemyDrawable;
        public AnimatedSprite sprite;

        //health stuff
        public int health;
        public bool invulnerable;

        // lights for eyes
        public Spotlight leftEye;
        public Spotlight rightEye;

        //shader
        public Shader enemyHit;
        public float shaderTween;
        public float hitCooldown;

        //animation
        public Animation placeholder;
		public Animation dead;

        //misc
        Player player;
        //bool isAlive = true;
        Camera camera;
        public EnemyManager enemyManager;
        List<Bullet> bullets = new List<Bullet>();
		bool hasBeenKnockedBack = false;

        //special AI
        TimeSpan timeSinceLastMove = new TimeSpan();
        TimeSpan specialCooldown = new TimeSpan(0, 0, 3);
        Random random = new Random();

        //callbacks
        public delegate void DestroyBullet(Bullet bullet);
        public delegate void DestroyEnemy();
        public delegate void MeleeDestoryEnemy();

        public GhostEnemy(string tag, Vector2 position, World world, Player player, Camera camera, EnemyManager manager) : base(tag)
        {
            this.position = position;

            //texture - using button as placeholder
            texture = new Texture("assets/ghost.png");
            sprite = new AnimatedSprite(texture, 64, 64);
            enemyDrawable = new MultiDrawable(sprite);

            //shader
            enemyHit = new Shader(null, "shaders/enemyHit.frag");
            enemyHit.SetCurrentTextureParameter("texture");

            //animation
            placeholder = new Animation("placeholder", 0, 16);
			dead = new Animation("ghostDead", 19, 1);

            sprite.AddAnimation(placeholder);
			sprite.AddAnimation(dead);
            sprite.animationController.SetActiveAnimation(placeholder);

            //sprite.AddAnimation();

            //physics
            this.world = world;
            colliderOffset = new Vector2(0, 0);
            this.collider = new BoxCollider(tag, new Vector2(64, 64), this.position + this.colliderOffset);
            this.collider.AddTagToIgnore("characterMelee");
            this.collider.AddTagToIgnore("characterWalk");
            this.collider.AddTagToIgnore("enemyBullet");
            this.collider.AddTagToIgnore("one");
            this.world.AddCollider(collider);

            //misc
            this.objectDrawable = enemyDrawable;
            this.collider.debug = true;
            this.enemyManager = manager;
            this.player = player;
            this.health = 3;
            this.camera = camera;

            //callbacks
            DestroyEnemy enemyCallback = DeleteEnemy;
            this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());
            MeleeDestoryEnemy meleeEnemyCallback = MeleeEnemy;
            this.collider.CreateOnCollisionEnter("characterMelee", () => meleeEnemyCallback());

            // eyes as candles
            this.leftEye = new Spotlight(4, new Color(1f, 1f, 1f), new Vector2(this.collider.position.X + 25, this.collider.position.Y + 18), 100, 0.5f, true);
            leftEye.shader.SetParameter("thisLightIntensity", leftEye.intensity);
            this.rightEye = new Spotlight(4, new Color(1f, 1f, 1f), new Vector2(this.collider.position.X + 34, this.collider.position.Y + 18), 100, 0.5f, true);
            rightEye.shader.SetParameter("thisLightIntensity", rightEye.intensity);
        }

        public override void Update(float deltaTime)
        {
            if (isAlive)
            {
                this.position = this.collider.position + this.colliderOffset;
                setEyeLightPosition(deltaTime);

                if (collider.debug)
                {
                    collider.UpdateVertices();
                }

                if (health > 0)
                {
                    CheckForRelocation(deltaTime);
					CheckForKnockback();
                }

                if (health == 0)
                {
                    //Console.WriteLine(shaderTween);
                    //if (shaderTween > 1)
                    //{
                    //Console.WriteLine(this.enemyManager.spawnFies);
                    if (this.sprite.animationController.hasReachedEnd)
                    {
                        Console.WriteLine("ghost dead!");
                        this.sprite.animationController.SetActiveAnimation(dead);
                        this.sprite.animationController.dontLoop = true;
                        isAlive = false;
                    }
                    //}
                }

                
                
            }

			if (invulnerable)
			{
				shaderTween += 0.05f;
				enemyHit.SetParameter("tweenValue", shaderTween);
			}
			
			if (shaderTween > 1)
			{
				invulnerable = false;
				shaderTween = 0;
				sprite.shader = null;
			}

            foreach (Bullet bullet in bullets)
            {
                bullet.Update(deltaTime);
            }

            CheckBulletScreenBounds();

            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(enemyDrawable);
            if (isAlive)
            {
                lightMap.Draw(this.leftEye);
                lightMap.Draw(this.rightEye);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(diffuseSurface, lightMap, deltaTime);
            }

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

        public void CheckForRelocation(float deltaTime)
        {
            if (timeSinceLastMove > specialCooldown)
            {
                // lets move
                this.collider.position = new Vector2(random.Next((int)camera.GetTopLeftScreenBounds().X, (int)camera.GetBottomRightScreenBounds().X - this.sprite.width), 
                                                     random.Next((int)camera.GetTopLeftScreenBounds().Y, (int)camera.GetBottomRightScreenBounds().Y - this.sprite.height));
                timeSinceLastMove = new TimeSpan();


                int points = 10;
                int radius = 5;
                double slice = 2 * Math.PI / points;
                for (int i = 0; i < points; i++)
                {
                    double angle = slice * i;
                    //Console.WriteLine("angle: " + angle);
                    float x = (float)((this.collider.position.X + 32) + radius * Math.Cos(angle));
                    float y = (float)((this.collider.position.Y + 32) + radius * Math.Sin(angle));
                    Vector2 target = new Vector2(x, y);
                    //Console.WriteLine(target);
                    Fire(target);
                }
            }

            timeSinceLastMove += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
        }

        public void Fire(Vector2 target)
        {
            Vector2 direction = target - (this.collider.position + new Vector2(32, 32));
            direction.Normalize();

            direction *= 300;

            Bullet newBullet = new Bullet("enemyBullet", (this.collider.position + new Vector2(32, 32)), this.world, direction);
            newBullet.collider.AddVelocity(direction);
            newBullet.collider.AddTagToIgnore(this.tag);
            newBullet.collider.AddTagToIgnore("littleEye");
            newBullet.collider.AddTagToIgnore("characterMelee");
            newBullet.collider.AddTagToIgnore("characterWalk");
            newBullet.collider.AddTagToIgnore("skelly");
			newBullet.collider.AddTagToIgnore("buttonOne");
			newBullet.collider.AddTagToIgnore("buttonTwo");
			newBullet.collider.AddTagToIgnore("buttonThree");
            DestroyBullet bulletCallback = DeleteBullet;

            //callbacks
            newBullet.collider.CreateOnCollisionEnter("box1", () => bulletCallback(newBullet));
            newBullet.collider.CreateOnCollisionEnter("one", () => bulletCallback(newBullet));
            //newBullet.collider.CreateOnCollisionEnter("littleEye", () => bulletCallback(newBullet));


            bullets.Add(newBullet);

        }

        public void CheckBulletScreenBounds()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {

                if (bullets[i].position.X < camera.GetTopLeftScreenBounds().X)
                {
                    DeleteBullet(bullets[i]);
                    return;
                }
                //Console.WriteLine(bullets[i].position.X + ", " + (bullets[i].position.X + bullets[i].width )+ ", " + bullets[i].collider.bottomRight.X);
                if (bullets[i].position.X + bullets[i].width > camera.GetBottomRightScreenBounds().X)
                {
                    DeleteBullet(bullets[i]);
                    return;
                }
                if (bullets[i].position.Y < camera.GetTopLeftScreenBounds().Y)
                {
                    DeleteBullet(bullets[i]);
                    return;
                }
                if (bullets[i].position.Y + bullets[i].height > camera.GetBottomRightScreenBounds().Y)
                {
                    DeleteBullet(bullets[i]);
                    return;
                }

            }
        }

        public void DeleteBullet(Bullet bullet)
        {
            //Console.WriteLine("removed");
            world.RemoveCollider(bullet.collider);
            bullets.Remove(bullet);
        }

        public void DeleteEnemy()
        {
            if (!invulnerable)
            {
				//we want our guy to move now.
				timeSinceLastMove += specialCooldown;

                invulnerable = true;
                health -= 1;
                sprite.AddShader(enemyHit);
                if (health == 0)
                {
                    Console.WriteLine("dying");
                    this.world.RemoveCollider(this.collider);
                    this.sprite.animationController.SetActiveAnimation(placeholder);
                    this.sprite.animationController.dontLoop = true;
                }

                Vector2 target = this.player.collider.position + this.player.collider.size / 2;
                Vector2 direction = target - this.collider.position;
                direction.Normalize();

                direction *= 300;

                //this.collider.AddVelocity(-direction);
            }
        }

        public void MeleeEnemy()
        {
            if (this.player.meleeButtonDown)
            {
                //Console.WriteLine("melee");
                DeleteEnemy();
            }
        }

        /// <summary>
        /// This is so ugly
        /// </summary>
        public void setEyeLightPosition(float deltaTime)
        {
            if (sprite.animationController.curFrame == 0 || sprite.animationController.curFrame == 8)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 25 + 3), (this.collider.position.Y + 18 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 34 + 3), (this.collider.position.Y + 18 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 1 || sprite.animationController.curFrame == 7)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 26 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 35 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 2 || sprite.animationController.curFrame == 6)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 26 + 3), (this.collider.position.Y + 16 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 35 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 3 || sprite.animationController.curFrame == 5)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 27 + 3), (this.collider.position.Y + 15 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 36 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 4)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 27 + 3), (this.collider.position.Y + 15 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 36 + 3), (this.collider.position.Y + 19 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 9 || sprite.animationController.curFrame == 15)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 24 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 33 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 10 || sprite.animationController.curFrame == 14)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 24 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 33 + 3), (this.collider.position.Y + 16 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 11 || sprite.animationController.curFrame == 13)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 23 + 3), (this.collider.position.Y + 17 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 32 + 3), (this.collider.position.Y + 15 + 3)), deltaTime);
            }
            else if (sprite.animationController.curFrame == 12)
            {
                leftEye.Update(new Vector2((this.collider.position.X + 23 + 3), (this.collider.position.Y + 19 + 3)), deltaTime);
                rightEye.Update(new Vector2((this.collider.position.X + 32 + 3), (this.collider.position.Y + 15 + 3)), deltaTime);
            }
        }
		public void CheckForKnockback()
		{
			if (hasBeenKnockedBack == false)
			{
				if (this.player.sheildOnCooldown)
				{
					Console.WriteLine ("knocking back");
					hasBeenKnockedBack = true;
					
					Vector2 target = this.player.collider.position + this.player.collider.size / 2;
					Vector2 direction = target - this.collider.position;
					direction.Normalize();
					
					direction *= 500;
					
					this.collider.AddVelocity(-direction);
				}
			}
			else
			{
				if (this.player.sheildOnCooldown == false)
				{
					hasBeenKnockedBack = false;
				}
			}
			
			
		}
    }
}
