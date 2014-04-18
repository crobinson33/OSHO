using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Enemy : BaseObject
    {

        World world;
        public BoxCollider collider;
        public Texture texture;
        public MultiDrawable enemyDrawable;
        public AnimatedSprite sprite;

        public Animation black;
        public Animation red;

        // need to know where the player is.
        public Player player;
		public bool isAlive = true;

		public delegate void DestroyEnemy();

        public Enemy(string tag, Vector2 position, World world, Player player) : base(tag)
        {
            this.position = position;

            texture = new Texture("assets/enemy.png");
            sprite = new AnimatedSprite(texture, 32, 32);
            enemyDrawable = new MultiDrawable(sprite);
            //enemyDrawable.AddDrawable(sprite);

            black = new Animation("blackEnemy", 0, 1);
            red = new Animation("redEnemy", 1, 1);

            sprite.AddAnimation(black);
            sprite.AddAnimation(red);

            sprite.animationController.SetActiveAnimation(black);

            this.world = world;
            this.collider = new BoxCollider(tag, new Vector2(32, 32), this.position);
            this.world.AddCollider(collider);

			DestroyEnemy enemyCallback = DeleteEnemy;
			this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());

            this.player = player;
        }

        public override void Update(float deltaTime)
        {
            this.position = this.collider.position;
            //Console.WriteLine(this.player.position);
            FollowPlayer();

            base.Update(deltaTime);
        }

		public void DeleteEnemy()
		{
			Console.WriteLine ("deleted enemy");
			this.world.RemoveCollider(this.collider);
		}

        public override void Draw(Surface surface, float deltaTime)
        {
            surface.Draw(enemyDrawable, this.position, deltaTime);
            base.Draw(surface, deltaTime);
        }

        public void FollowPlayer()
        {
            Vector2 target = this.player.position;
            Vector2 direction = target - this.position;
            direction.Normalize();

            direction *= 500;

            this.collider.AddVelocity(direction);
            
        }
    }
}
