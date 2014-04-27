using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Enemy : VisibleObject
    {

        World world;
        public BoxCollider collider;
		Vector2 colliderOffset;
        public Texture texture;
        public MultiDrawable enemyDrawable;
        public AnimatedSprite sprite;
        public int health;
        public bool invulnerable;
        public Shader enemyHit;
        public float shaderTween;
        public float hitCooldown;

        public Animation black;
        public Animation dying;
        public Animation dead;

        public bool hasBeenMeleeHit = false;

        // need to know where the player is.
        public Player player;
		public bool isAlive = true;

		public delegate void DestroyEnemy();
		public delegate void MeleeDestoryEnemy();

        public Enemy(string tag, Vector2 position, World world, Player player) : base(tag)
        {
            this.position = position;
            invulnerable = false;
            health = 4;
            shaderTween = 0;
            hitCooldown = 2;

            texture = new Texture("assets/Eyefly.png");
            sprite = new AnimatedSprite(texture, 32, 32);
            enemyDrawable = new MultiDrawable(sprite);

            enemyHit = new Shader(null, "shaders/enemyHit.frag");
            enemyHit.SetCurrentTextureParameter("texture");

            black = new Animation("blackEnemy", 0, 8);
            dying = new Animation("dyingEnemy", 8, 6);
            dead = new Animation("deadEnemy", 13, 1);

            sprite.AddAnimation(black);
            sprite.AddAnimation(dying);
            sprite.AddAnimation(dead);

            sprite.animationController.SetActiveAnimation(black);

            this.world = world;
			colliderOffset = new Vector2(-5, -5);
            this.collider = new BoxCollider(tag, new Vector2(20, 20), this.position + this.colliderOffset);
			this.collider.AddTagToIgnore("characterMelee");
            this.collider.AddTagToIgnore("characterWalk");
            //this.collider.AddTagToIgnore(tag);
            this.world.AddCollider(collider);

			DestroyEnemy enemyCallback = DeleteEnemy;
			this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());
			MeleeDestoryEnemy meleeEnemyCallback = MeleeEnemy;
			this.collider.CreateOnCollisionEnter("characterMelee", () => meleeEnemyCallback());

            this.player = player;

            this.objectDrawable = enemyDrawable;

            this.collider.debug = true;
        }

        public override void Update(float deltaTime)
        {
            if (isAlive)
            {
                this.position = this.collider.position + this.colliderOffset;
                //Console.WriteLine(this.player.position);
                FollowPlayer();

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
                    if (this.sprite.animationController.hasReachedEnd)
                    {
                        this.sprite.animationController.SetActiveAnimation(dead);
                        isAlive = false;
                    }
                }

                if (collider.debug)
                {
                    collider.UpdateVertices();
                }

                base.Update(deltaTime);
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

                direction *= 10000;

                this.collider.AddVelocity(-direction);
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


        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(enemyDrawable);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

        public void FollowPlayer()
        {
			Vector2 target = this.player.collider.position + this.player.collider.size / 2;
            Vector2 direction = target - this.collider.position;
            direction.Normalize();

            direction *= 500;

            this.collider.AddVelocity(direction);
            
        }
    }
}
