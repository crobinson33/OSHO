using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Player : BaseObject
    {
        public Texture atexture;
        public AnimatedSprite asprite;
        public Animation idleDownAnimation;
        public Animation idleUpAnimation;
        public Animation idleLeftAnimation;
        public Animation idleRightAnimation;
        public Animation downRunAnimation;

        public CharacterCollider collider;

        List<Bullet> bullets = new List<Bullet>();
        World world;

        Mouse mouse;

        public Player(string tag, Vector2 position, World world, Mouse mouse) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;


            atexture = new Texture("HunchSprite.png");

            asprite = new AnimatedSprite(atexture, 64, 64);

            
            // Create animations
            downRunAnimation = new Animation("downRun", 0, 10);
            idleDownAnimation = new Animation("idleDown", 10, 7);
            idleUpAnimation = new Animation("idleUp", 20, 7);
            idleLeftAnimation = new Animation("idleLeft", 30, 7, true);
            idleRightAnimation = new Animation("idleRight", 30, 7);

            // Add animations
            asprite.AddAnimation(idleDownAnimation);
            asprite.AddAnimation(downRunAnimation);

            // Test animation
            asprite.animationController.SetActiveAnimation(idleRightAnimation);

            this.world = world;
            collider = new CharacterCollider("player", new Vector2(64, 64), this.position);
            collider.AddTagToIgnore("bullet");
            this.world.AddCollider(collider);

            this.mouse = mouse;

            Console.WriteLine("player end...");
        }

        public override void Update(float deltaTime)
        {
            this.collider.CalculatePoints();
            this.position = this.collider.position;
            //asprite.Update(this.position);
            HandleInput();
            //Console.WriteLine("getting called...");

            //Console.WriteLine(bullets.Count);
            foreach(Bullet bullet in bullets)
            {
                bullet.Update(deltaTime);
            }

            //remove bullets off screen
            CheckBulletScreenBounds();

            base.Update(deltaTime);
        }

        public override void Draw(Surface surface, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(asprite, this.position, deltaTime);

            foreach(Bullet bullet in bullets)
            {
                bullet.Draw(surface, deltaTime);
            }


            base.Draw(surface, deltaTime);
        }

        public void CheckBulletScreenBounds()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                
                if (bullets[i].position.X < 0)
                {
                    Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                //Console.WriteLine(bullets[i].position.X + ", " + (bullets[i].position.X + bullets[i].width )+ ", " + bullets[i].collider.bottomRight.X);
                if (bullets[i].position.X + bullets[i].width > 800)
                {
                    Console.WriteLine("removed");
                    //bullets[i].Dispose();
                    //bullets[i] = null;
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                if (bullets[i].position.Y < 0)
                {
                    Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                if (bullets[i].position.Y + bullets[i].height > 600)
                {
                    Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
            }
        }

        public void HandleInput()
        {
            Keyboard keyboard = new Keyboard();
            float vel = 20;

            //up
            if (keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(0, -vel));
                this.asprite.animationController.SetActiveAnimation(idleUpAnimation);
            }

            //down
            if (keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(0, vel));
                this.asprite.animationController.SetActiveAnimation(idleDownAnimation);
            }

            //left
            if (keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(-vel, 0));
                this.asprite.animationController.SetActiveAnimation(idleLeftAnimation);
            }

            //right
            if (keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(vel, 0));
                this.asprite.animationController.SetActiveAnimation(idleRightAnimation);
            }

            //space
            if (keyboard.IsKeyDown(Key.KeyCode.Space))
            {
                Vector2 target = mouse.GetMousePosition();

                Vector2 direction = target - this.position;
                direction.Normalize();

                //Console.WriteLine(target);
                //Console.WriteLine(direction);
                float velocity = 500000;
                //Console.WriteLine(direction * velocity);

                Bullet newBullet = new Bullet("bullet", this.position, this.world, direction * velocity);
                newBullet.collider.AddVelocity(direction * (velocity));
                bullets.Add(newBullet);
            }

            //Console.WriteLine(this.position + ", " + this.collider.position + ", " + this.collider.velocity);
        }
    }
}
