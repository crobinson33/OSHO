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

        //public Texture playerWeapon;
        public AnimatedSprite playerWeaponSprite;
        public AnimatedSprite playerArm;

        public Animation idleDownAnimation;
        public Animation idleUpAnimation;
        public Animation idleLeftAnimation;
        public Animation idleRightAnimation;
        public Animation downRunAnimation;

        public Animation weaponDown;
        public Animation weaponUp;
        public Animation weaponLeft;
        public Animation weaponRight;
        public Animation weaponBlank;

        public Animation runDownAnimation;
        public Animation runUpAnimation;
        public Animation runLeftAnimation;
        public Animation runRightAnimation;

        public Animation playerArmDown;
        public Animation playerArmUp;
        public Animation playerArmLeft;
        public Animation playerArmRight;

        public Shader test;

        public CharacterCollider collider;

        List<Bullet> bullets = new List<Bullet>();
        World world;

        Mouse mouse;

        public delegate void DestroyBullet(Bullet bullet);

        public Player(string tag, Vector2 position, World world, Mouse mouse) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;


            atexture = new Texture("assets/HunchSpriteFinal.png");
            asprite = new AnimatedSprite(atexture, 64, 64);
            playerWeaponSprite = new AnimatedSprite(atexture, 64, 64);
            playerArm = new AnimatedSprite(atexture, 64, 64);

            test = new Shader(null, "assets/test.frag");

            
            // Create animations
            downRunAnimation = new Animation("downRun", 0, 10);
            idleDownAnimation = new Animation("idleDown", 10, 7);
            idleUpAnimation = new Animation("idleUp", 20, 7);
            idleLeftAnimation = new Animation("idleLeft", 30, 7, true);
            idleRightAnimation = new Animation("idleRight", 30, 7);

            //runs.
            runDownAnimation = new Animation("runDown", 40, 10);
            runUpAnimation = new Animation("runUp", 50, 10);
            runRightAnimation = new Animation("runRight", 60, 10);
            runLeftAnimation = new Animation("runLeft", 70, 10);

            weaponDown = new Animation("weaponDown", 0, 1);
            weaponUp = new Animation("weaponUp", 1, 1);
            weaponLeft = new Animation("weaponLeft", 2, 1);
            weaponRight = new Animation("weaponRight", 3, 1);

            playerArmDown = new Animation("armDown", 4, 1);
            playerArmUp = new Animation("armUp", 5, 1);
            playerArmLeft = new Animation("armLeft", 6, 1);
            playerArmRight = new Animation("armRight", 7, 1);

            // Add animations
            asprite.AddAnimation(idleDownAnimation);
            asprite.AddAnimation(downRunAnimation);
            asprite.AddAnimation(runDownAnimation);
            asprite.AddAnimation(runUpAnimation);
            asprite.AddAnimation(runRightAnimation);
            asprite.AddAnimation(runLeftAnimation);

            playerWeaponSprite.AddAnimation(weaponDown);
            playerWeaponSprite.AddAnimation(weaponUp);
            playerWeaponSprite.AddAnimation(weaponLeft);
            playerWeaponSprite.AddAnimation(weaponRight);

            playerArm.AddAnimation(playerArmDown);
            playerArm.AddAnimation(playerArmUp);
            playerArm.AddAnimation(playerArmLeft);
            playerArm.AddAnimation(playerArmRight);


            // Add shader
            test.SetCurrentTextureParameter("texture");
            //asprite.addShader(test);

            // Test animation
            asprite.animationController.SetActiveAnimation(idleRightAnimation);
            playerWeaponSprite.animationController.SetActiveAnimation(weaponRight);
            playerArm.animationController.SetActiveAnimation(playerArmRight);

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
            //CheckForIdle();

            base.Update(deltaTime);
        }

        public override void Draw(Surface surface, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(asprite, this.position, deltaTime);
            surface.Draw(playerArm, this.position, deltaTime);
            surface.Draw(playerWeaponSprite, this.position, deltaTime);
            
                        

            foreach(Bullet bullet in bullets)
            {
                bullet.Draw(surface, deltaTime);
            }


            base.Draw(surface, deltaTime);
        }

        public void CheckForIdle()
        {
            if (Math.Abs(this.collider.velocity.X) < 1f && Math.Abs(this.collider.velocity.Y) < 1f)
            {
                this.asprite.animationController.SetActiveAnimation(idleDownAnimation);
            }
        }

        public void CheckBulletScreenBounds()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                
                if (bullets[i].position.X < 0)
                {
                    //Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                //Console.WriteLine(bullets[i].position.X + ", " + (bullets[i].position.X + bullets[i].width )+ ", " + bullets[i].collider.bottomRight.X);
                if (bullets[i].position.X + bullets[i].width > 800)
                {
                    //Console.WriteLine("removed");
                    //bullets[i].Dispose();
                    //bullets[i] = null;
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                if (bullets[i].position.Y < 0)
                {
                    //Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
                if (bullets[i].position.Y + bullets[i].height > 600)
                {
                    //Console.WriteLine("removed");
                    world.RemoveCollider(bullets[i].collider);
                    bullets.RemoveAt(i);
                    return;
                }
            }
        }

        public void DeleteBullet(Bullet bullet)
        {
            Console.WriteLine("got here");
            world.RemoveCollider(bullet.collider);
            bullets.Remove(bullet);
        }

        public void HandleInput()
        {
            Keyboard keyboard = new Keyboard();
            float vel = 15;

            //up
            if (keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(0, -vel));
                this.asprite.animationController.SetActiveAnimation(runUpAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmUp);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponUp);
            }

            //down
            if (keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(0, vel));
                this.asprite.animationController.SetActiveAnimation(runDownAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmDown);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);
            }

            //left
            if (keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(-vel, 0));
                this.asprite.animationController.SetActiveAnimation(runLeftAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmLeft);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponLeft);
            }

            //right
            if (keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(vel, 0));
                this.asprite.animationController.SetActiveAnimation(runRightAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmRight);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponRight);
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
                DestroyBullet bulletCallback = DeleteBullet;
                newBullet.collider.CreateOnCollisionEnter("box1", () => bulletCallback(newBullet));
                bullets.Add(newBullet);
            }

            //Console.WriteLine(this.position + ", " + this.collider.position + ", " + this.collider.velocity);
        }
    }
}
