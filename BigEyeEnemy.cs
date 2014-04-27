using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class BigEyeEnemy : VisibleObject
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
        public bool hasBeenMeleeHit = false;

        //shader
        public Shader enemyHit;
        public float shaderTween;
        public float hitCooldown;

        //animation
        //public Animation placeholder;
        public Animation flyAnimation;
        public Animation dying;
        public Animation exploding;
        public Animation dead;

        //manager
        public EnemyManager enemyManager;

        //callbacks
        public delegate void DestroyEnemy();
        public delegate void MeleeDestoryEnemy();
        public delegate void DestroyBullet(Bullet bullet);

        //misc
        Player player;
        bool isAlive = true;
        Camera camera;

        //bullet logic
        bool canFire = true;
        TimeSpan fireCooldown = new TimeSpan(0, 0, 0, 4);
        TimeSpan timeSinceLastFire = new TimeSpan();
        List<Bullet> bullets = new List<Bullet>();


        public BigEyeEnemy(string tag, Vector2 position, World world, Player player, Camera camera, EnemyManager manager) : base(tag)
        {
            this.position = position;

            //texture - using button as placeholder
            texture = new Texture("assets/FatFly.png");
            sprite = new AnimatedSprite(texture, 64, 64);
            enemyDrawable = new MultiDrawable(sprite);

            //shader
            enemyHit = new Shader(null, "shaders/enemyHit.frag");
            enemyHit.SetCurrentTextureParameter("texture");

            //animation
            //placeholder = new Animation("placeHolder", 0, 1);
            flyAnimation = new Animation("bigEyeFlying", 0, 8);
            dying = new Animation("bigEyeDying", 9, 5);
            exploding = new Animation("bigEyeExploding", 14, 4);
            dead = new Animation("bigEyeDead", 8, 1);

            //sprite.AddAnimation(placeholder);
            sprite.AddAnimation(flyAnimation);
            sprite.AddAnimation(dying);
            sprite.AddAnimation(exploding);
            sprite.animationController.SetActiveAnimation(flyAnimation);

            //physics
            this.world = world;
            colliderOffset = new Vector2(0, 0);
            this.collider = new BoxCollider(tag, new Vector2(64, 64), this.position + this.colliderOffset);
            this.collider.AddTagToIgnore("characterMelee");
            this.collider.AddTagToIgnore("characterWalk");
            this.world.AddCollider(collider);

            //misc
            this.objectDrawable = enemyDrawable;
            this.collider.debug = true;
            this.enemyManager = manager;
            this.player = player;
            this.health = 10;
            this.camera = camera;

            //callbacks
            DestroyEnemy enemyCallback = DeleteEnemy;
            this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());
            MeleeDestoryEnemy meleeEnemyCallback = MeleeEnemy;
            this.collider.CreateOnCollisionEnter("characterMelee", () => meleeEnemyCallback());
        }

        public override void Update(float deltaTime)
        {
            if (isAlive)
            {
                this.position = this.collider.position + this.colliderOffset;
                //Console.WriteLine(this.player.position);
                //FollowPlayer();

                if (health > 0)
                {
                    CheckForFire(deltaTime);
                    FollowPlayer();
                }

                CheckBulletScreenBounds();

                foreach (Bullet bullet in bullets)
                {
                    bullet.Update(deltaTime);
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

                //Console.WriteLine(shaderTween);

                if (health == 0)
                {
                    //Console.WriteLine(shaderTween);
                    //if (shaderTween > 1)
                    //{
                    //    //Console.WriteLine(this.enemyManager.spawnFies);
                        
                    //}

                    if (this.enemyManager.spawnFies == false)
                    {
                        //if (this.sprite.animationController.hasReachedEnd)
                        //{
                        Console.WriteLine("exploding!!!");
                        this.sprite.animationController.SetActiveAnimation(exploding);
                        this.sprite.animationController.dontLoop = true;
                        isAlive = false;
                        //}
                    }
                }

                if (collider.debug)
                {
                    collider.UpdateVertices();
                }

                
            }

            if (this.sprite.animationController.GetActiveAnimationName() == "bigEyeExploding" && this.sprite.animationController.hasReachedEnd)
            {
                this.sprite.animationController.SetActiveAnimation(dead);
            }


            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(enemyDrawable);

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

        /// <summary>
        /// See if its time to fire. if so, call fire.
        /// </summary>
        /// <param name="deltaTime">float deltaTime</param>
        public void CheckForFire(float deltaTime)
        {
            //Console.WriteLine("checking fire");
            if (timeSinceLastFire > fireCooldown)
            {
                //Console.WriteLine("firing");
                // get the points around the circle.
                int points = 10;
                int radius = 5;
                double slice = 2 * Math.PI / points;
                for (int i = 0; i < points; i++)
                {
                    double angle = slice * i;
                    //Console.WriteLine("angle: " + angle);
                    float x = (float)((this.position.X + 32) + radius * Math.Cos(angle));
                    float y = (float)((this.position.Y + 32) + radius * Math.Sin(angle));
                    Vector2 target = new Vector2(x, y);
                    //Console.WriteLine(target);
                    Fire(target);
                }

                    
                timeSinceLastFire = new TimeSpan();
            }

            timeSinceLastFire += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
        }

        public void Fire(Vector2 target)
        {
            Vector2 direction = target - (this.collider.position + new Vector2(32, 32));
            direction.Normalize();

            direction *= 1000;

            Bullet newBullet = new Bullet("enemyBullet", (this.collider.position + new Vector2(32, 32)), this.world, direction);
            newBullet.collider.AddVelocity(direction);
            newBullet.collider.AddTagToIgnore(this.tag);
            newBullet.collider.AddTagToIgnore("littleEye");
            newBullet.collider.AddTagToIgnore("characterMelee");
            newBullet.collider.AddTagToIgnore("characterWalk");
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
                invulnerable = true;
                health -= 1;
                sprite.AddShader(enemyHit);
                if (health == 0)
                {
                    this.world.RemoveCollider(this.collider);
                    this.sprite.animationController.SetActiveAnimation(dying);
                    //this.sprite.animationController.dontLoop = true;
                    this.enemyManager.SpawnLittleEyes(this.position + (this.collider.size / 2));
                }

                /*Vector2 target = this.player.collider.position + this.player.collider.size / 2;
                Vector2 direction = target - this.collider.position;
                direction.Normalize();

                direction *= 10000;*/

                //this.collider.AddVelocity(-direction);
            }
        }

        public void MeleeEnemy()
        {
            if (this.player.meleeButtonDown)
            {
                /*
				Vector2 target = this.player.collider.position + this.player.collider.size / 2;
				Vector2 direction = target - this.collider.position;
				direction.Normalize();

				direction *= 6000;

				this.collider.AddVelocity(-direction);
                 */
                if (!hasBeenMeleeHit)
                {
                    DeleteEnemy();
                    this.player.canDealMeleeDamage = false;
                    hasBeenMeleeHit = true;
                }
            }
            else
            {
                hasBeenMeleeHit = false;
            }
        }

        public void FollowPlayer()
        {
            Vector2 target = this.player.collider.position + this.player.collider.size / 2;
            Vector2 direction = target - this.collider.position;
            direction.Normalize();

            direction *= 50;

            this.collider.AddVelocity(direction);

        }
    }
}
