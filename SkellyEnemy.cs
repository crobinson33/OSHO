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

        //misc
        Player player;
        bool isAlive = true;
        Camera camera;
        public EnemyManager enemyManager;

        //special move
        bool hasDashed = false;
        TimeSpan dashTime = new TimeSpan();
        TimeSpan dashDuration = new TimeSpan(0, 0, 0, 0, 200);
        TimeSpan dashCooldown = new TimeSpan(0, 0, 2);
        TimeSpan timeOnCooldown = new TimeSpan();

        //callbacks
        public delegate void DestroyEnemy();
        public delegate void MeleeDestoryEnemy();

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
            placeholder = new Animation("placeholder", 0, 16);

            sprite.AddAnimation(placeholder);
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

                if (health > 0)
                {
                    //CheckForFire(deltaTime);
                    //FollowPlayer();
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
                        this.sprite.animationController.SetActiveAnimation(placeholder);
                        this.sprite.animationController.dontLoop = true;
                        isAlive = false;
                    }
                    //}
                }

                if (collider.debug)
                {
                    collider.UpdateVertices();
                }

                DashAtPlayer(deltaTime);
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
                    this.sprite.animationController.SetActiveAnimation(placeholder);
                    this.sprite.animationController.dontLoop = true;
                }

                Vector2 target = this.player.collider.position + this.player.collider.size / 2;
                Vector2 direction = target - this.collider.position;
                direction.Normalize();

                direction *= 300;

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
    }
}
