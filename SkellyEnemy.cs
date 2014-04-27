using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class SkellyEnemy : VisibleObject
    {
        //physics stuff
        World world;
        public BoxCollider collider;
        Vector2 colliderOffset;
		public BoxCollider meleeCollider;
		Vector2 meleeColliderOffset;

        //drawing stuff
        public Texture texture;
        public MultiDrawable enemyDrawable;
        public AnimatedSprite sprite;

        //health stuff
        public int health;
        public bool invulnerable;

        //shader
        public Shader enemyHit;
        public float shaderTween;
        public float hitCooldown;

        //animation
        //public Animation placeholder;
		public Animation hover;
		public Animation attackRight;
		public Animation attackLeft;
		public Animation dying;
		public Animation dead;

        //misc
        Player player;
        //bool isAlive = true;
        //Camera camera;
        public EnemyManager enemyManager;
		bool hasDealtDamage = false;
		bool hasBeenKnockedBack = false;

        //special move
        bool hasDashed = false;
        TimeSpan dashTime = new TimeSpan();
        TimeSpan dashDuration = new TimeSpan(0, 0, 0, 0, 200);
        TimeSpan dashCooldown = new TimeSpan(0, 0, 2);
        TimeSpan timeOnCooldown = new TimeSpan();

        //callbacks
        public delegate void DestroyEnemy();
        public delegate void MeleeDestoryEnemy();

		public delegate void CheckPlayerAndSwipe();

        public SkellyEnemy(string tag, Vector2 position, World world, Player player, Camera camera, EnemyManager manager) : base(tag)
        {
            this.position = position;

            //texture - using button as placeholder
            texture = new Texture("assets/skellyghost.png");
            sprite = new AnimatedSprite(texture, 64, 64);
            enemyDrawable = new MultiDrawable(sprite);

            //shader
            enemyHit = new Shader(null, "shaders/enemyHit.frag");
            enemyHit.SetCurrentTextureParameter("texture");

            //animation
            //placeholder = new Animation("placeholder", 0, 16);
			hover = new Animation("skellyHover", 0, 16);
			attackRight = new Animation("skellyAttackRight", 16, 10);
			attackLeft = new Animation("skellyAttackLeft", 16, 10, true);
			dying = new Animation("skellyDying", 31, 8);
			dead = new Animation("skellyDead", 38, 1);

            //sprite.AddAnimation(placeholder);
			sprite.AddAnimation(hover);
			sprite.AddAnimation(attackRight);
			sprite.AddAnimation(attackLeft);
			sprite.AddAnimation(dying);
			sprite.AddAnimation(dead);
            sprite.animationController.SetActiveAnimation(hover);

            //sprite.AddAnimation();

            //physics
            this.world = world;
            colliderOffset = new Vector2(-13, -8);
            this.collider = new BoxCollider(tag, new Vector2(35, 50), this.position + this.colliderOffset);
			//this.collider.mass = 10000000;
            this.collider.AddTagToIgnore("characterMelee");
            this.collider.AddTagToIgnore("characterWalk");
            this.collider.AddTagToIgnore("enemyBullet");
			this.collider.AddTagToIgnore("buttonOne");
			this.collider.AddTagToIgnore("buttonTwo");
			this.collider.AddTagToIgnore("buttonThree");
            this.collider.AddTagToIgnore("one");
			this.collider.AddTagToIgnore(tag + "Melee");
            this.world.AddCollider(collider);

			meleeColliderOffset = new Vector2(0, 0);
			this.meleeCollider = new BoxCollider(tag + "Melee", new Vector2(64, 64), this.position + this.meleeColliderOffset);
			this.meleeCollider.AddTagToIgnore("characterMelee");
			this.meleeCollider.AddTagToIgnore("characterWalk");
			this.meleeCollider.AddTagToIgnore("enemyBullet");
			this.meleeCollider.AddTagToIgnore("buttonOne");
			this.meleeCollider.AddTagToIgnore("buttonTwo");
			this.meleeCollider.AddTagToIgnore("buttonThree");
			this.meleeCollider.AddTagToIgnore("one");
			this.meleeCollider.AddTagToIgnore("bullet");
			this.meleeCollider.AddTagToIgnore(tag);
			this.meleeCollider.debug = true;
			this.world.AddCollider(meleeCollider);

            //misc
            this.objectDrawable = enemyDrawable;
            this.collider.debug = true;
            this.enemyManager = manager;
            this.player = player;
            this.health = 10;
            //this.camera = camera;

            //callbacks
            DestroyEnemy enemyCallback = DeleteEnemy;
            this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());
            MeleeDestoryEnemy meleeEnemyCallback = MeleeEnemy;
            this.collider.CreateOnCollisionEnter("characterMelee", () => meleeEnemyCallback());
			CheckPlayerAndSwipe playerCallback = CheckDamage;
			this.meleeCollider.CreateOnCollisionEnter("one", () => playerCallback());
        }

        public override void Update(float deltaTime)
        {
            if (isAlive)
            {
                this.position = this.collider.position + this.colliderOffset;
				this.meleeCollider.position = this.position;

                if (health > 0)
                {
                    //CheckForFire(deltaTime);
                    //FollowPlayer();
					CheckForKnockback();
					DashAtPlayer(deltaTime);
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
                        //Console.WriteLine(this.enemyManager.spawnFies);
                    if (this.sprite.animationController.hasReachedEnd)
                    {
                        Console.WriteLine("skelly dead!");
                        this.sprite.animationController.SetActiveAnimation(dead);
                        this.sprite.animationController.dontLoop = true;
                        isAlive = false;
                    }
                    //}
                }

                if (collider.debug)
                {
                    collider.UpdateVertices();
                }
				if (meleeCollider.debug)
				{
					meleeCollider.UpdateVertices();
				}

                
            }

            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(enemyDrawable);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

			if (meleeCollider.debug)
			{
				meleeCollider.DrawDebugBox(diffuseSurface, deltaTime);
			}

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

        public void DashAtPlayer(float deltaTime)
        {
            if (hasDashed == false)
            {
                if (dashDuration > dashTime)
                {
                    //Console.WriteLine("dashing");
                    // we want to get a point way past teh player so we will move at almost the same speed everytime.
                    Vector2 direction = this.player.position - this.position;
                    direction.Normalize();

                    direction *= 1000;
                    //Console.WriteLine(this.position + ", " + (this.position + direction));

                    //Console.WriteLine((float)dashTime.Milliseconds / (float)dashDuration.Milliseconds);

                    //this.collider.position = Util.Lerp(this.collider.position, direction, (float)dashTime.Milliseconds / (float)dashDuration.Milliseconds);
                    this.collider.position = Util.Lerp(this.collider.position, this.player.position + direction, 0.01f);
                }
                else
                {
                    dashTime = new TimeSpan();
                    hasDashed = true;
					// dash has ended play swipe.

					// determine which direction player is to decide which animation to use.
					if (player.position.X > this.position.X)
					{
						this.sprite.animationController.SetActiveAnimation(attackRight);
						this.sprite.animationController.dontLoop = true;
					}
					else
					{
						this.sprite.animationController.SetActiveAnimation(attackLeft);
						this.sprite.animationController.dontLoop = true;
					}
                }
                dashTime += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
            }
            else
            {
                if (dashCooldown < timeOnCooldown)
                {
                    //Console.WriteLine("dash reset");
                    hasDashed = false;
                    timeOnCooldown = new TimeSpan();
                }

                timeOnCooldown += new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));
            }

			if (this.sprite.animationController.GetActiveAnimationName() == "skellyAttackRight" ||
			    this.sprite.animationController.GetActiveAnimationName() == "skellyAttackLeft")
			{
				if (this.sprite.animationController.hasReachedEnd)
				{
					this.sprite.animationController.SetActiveAnimation(hover);
					this.sprite.animationController.dontLoop = false;
					hasDealtDamage = false;
				}
			}


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
                    this.sprite.animationController.dontLoop = true;
                }

                Vector2 target = this.player.collider.position + this.player.collider.size / 2;
                Vector2 direction = target - this.collider.position;
                direction.Normalize();

                direction *= 30;

                this.collider.AddVelocity(-direction);
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

		public void CheckDamage()
		{

			if (this.sprite.animationController.GetActiveAnimationName() == "skellyAttackRight" ||
			 this.sprite.animationController.GetActiveAnimationName() == "skellyAttackLeft")
			{
				if (hasDealtDamage == false)
				{
					//Console.WriteLine ("we are inside the player.");
					this.player.TakeDamage();
					hasDealtDamage = true;
				}
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
