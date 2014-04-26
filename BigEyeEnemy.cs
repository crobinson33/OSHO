using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class BigEyeEnemy : BaseObject
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

        //shader
        public Shader enemyHit;
        public float shaderTween;
        public float hitCooldown;

        //animation
        public Animation placeholder;

        //manager
        public EnemyManager enemyManager;

        //callbacks
        public delegate void DestroyEnemy();
        public delegate void MeleeDestoryEnemy();

        public BigEyeEnemy(string tag, Vector2 position, World world, Player player, EnemyManager manager) : base(tag)
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
            placeholder = new Animation("placeHolder", 0, 1);
            sprite.AddAnimation(placeholder);
            sprite.animationController.SetActiveAnimation(placeholder);

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

            //callbacks
            DestroyEnemy enemyCallback = DeleteEnemy;
            this.collider.CreateOnCollisionEnter("bullet", () => enemyCallback());
            MeleeDestoryEnemy meleeEnemyCallback = MeleeEnemy;
            this.collider.CreateOnCollisionEnter("characterMelee", () => meleeEnemyCallback());
        }

        public override void Update(float deltaTime)
        {
            this.position = this.collider.position + this.colliderOffset;

            if (collider.debug)
            {
                collider.UpdateVertices();
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

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

        public void DeleteEnemy()
        {
            this.enemyManager.SpawnLittleEyes(this.position);
        }

        public void MeleeEnemy()
        {
            DeleteEnemy();
        }
    }
}
