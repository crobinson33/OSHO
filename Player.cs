﻿using System;
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
        public MultiDrawable playerDrawable;

        //public Texture playerWeapon;
        public AnimatedSprite baseSprite;
        public AnimatedSprite playerWeaponSprite;
        public AnimatedSprite playerArm;

        public Animation idleDownAnimation;
        public Animation idleUpAnimation;
        public Animation idleLeftAnimation;
        public Animation idleRightAnimation;

        public Animation weaponDown;
        public Animation weaponUp;
        public Animation weaponLeft;
        public Animation weaponRight;
        public Animation weaponBlank;

        public Animation weaponDownFire;
        public Animation weaponUpFire;
        public Animation weaponLeftFire;
        public Animation weaponRightFire;
        public Animation weaponClear;

        public Animation runDownAnimation;
        public Animation runUpAnimation;
        public Animation runLeftAnimation;
        public Animation runRightAnimation;

        public Animation playerArmDown;
        public Animation playerArmUp;
        public Animation playerArmLeft;
        public Animation playerArmRight;
        public Animation playerArmClear;

        public Shader test;

        public Camera camera;

        public CharacterCollider collider;

        List<Bullet> bullets = new List<Bullet>();
        World world;

        Mouse mouse;

        public delegate void DestroyBullet(Bullet bullet);


        // for fire rate.
        //int fireRateSeconds = 1;
        TimeSpan timeSinceLastFire = new TimeSpan(0, 0, 0); // can't pass a variable in here so make it the same as firerate
        TimeSpan timeToFire = new TimeSpan(0, 0, 0, 0, 300);
        //bool hasFired = false;

        public Player(string tag, Vector2 position, World world, Mouse mouse, Camera camera) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;


            atexture = new Texture("assets/HunchSprite.png");

            baseSprite = new AnimatedSprite(atexture, 64, 64);
            playerWeaponSprite = new AnimatedSprite(atexture, 64, 64);
            playerArm = new AnimatedSprite(atexture, 64, 64);

            playerDrawable = new MultiDrawable(baseSprite);
            playerDrawable.AddDrawable(playerArm);
            playerDrawable.AddDrawable(playerWeaponSprite);

            this.camera = camera;
            
            // Create animations
            idleDownAnimation = new Animation("idleDown", 20, 7);
            idleUpAnimation = new Animation("idleUp", 30, 7);
            idleRightAnimation = new Animation("idleRight", 40, 7);
            idleLeftAnimation = new Animation("idleLeft", 40, 7, true);

            //runs.
            runDownAnimation = new Animation("runDown", 50, 10);
            runUpAnimation = new Animation("runUp", 60, 10);
            runRightAnimation = new Animation("runRight", 70, 10);
            runLeftAnimation = new Animation("runLeft", 80, 10);

            weaponDown = new Animation("weaponDown", 10, 1);
            weaponUp = new Animation("weaponUp", 11, 1);
            weaponLeft = new Animation("weaponLeft", 12, 1);
            weaponRight = new Animation("weaponRight", 13, 1);
            weaponClear = new Animation("weaponClear", 29, 1);
            

            weaponDownFire = new Animation("weaponDownFire", 0, 1);
            weaponUpFire = new Animation("weaponUpFire", 1, 1);
            weaponLeftFire = new Animation("weaponLeftFire", 2, 1);
            weaponRightFire = new Animation("weaponRightFire", 3, 1);

            playerArmDown = new Animation("armDown", 14, 1);
            playerArmUp = new Animation("armUp", 15, 1);
            playerArmLeft = new Animation("armLeft", 16, 1);
            playerArmRight = new Animation("armRight", 17, 1);
            playerArmClear = new Animation("armClear", 29, 1);

            // Add animations
            baseSprite.AddAnimation(idleDownAnimation);
            baseSprite.AddAnimation(idleUpAnimation);
            baseSprite.AddAnimation(idleRightAnimation);
            baseSprite.AddAnimation(idleLeftAnimation);
            baseSprite.AddAnimation(runDownAnimation);
            baseSprite.AddAnimation(runUpAnimation);
            baseSprite.AddAnimation(runRightAnimation);
            baseSprite.AddAnimation(runLeftAnimation);

            playerWeaponSprite.AddAnimation(weaponDown);
            playerWeaponSprite.AddAnimation(weaponUp);
            playerWeaponSprite.AddAnimation(weaponLeft);
            playerWeaponSprite.AddAnimation(weaponRight);
            playerWeaponSprite.AddAnimation(weaponClear);

            playerArm.AddAnimation(playerArmDown);
            playerArm.AddAnimation(playerArmUp);
            playerArm.AddAnimation(playerArmLeft);
            playerArm.AddAnimation(playerArmRight);
            playerArm.AddAnimation(playerArmClear);


            // Add shader
            //test.SetCurrentTextureParameter("texture");
            //baseSprite.AddShader(test);

            // Test animation
            baseSprite.animationController.SetActiveAnimation(idleRightAnimation);
            playerDrawable.drawPartsInFront = false;
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
            HandleInput(deltaTime);
            //Console.WriteLine("getting called...");

            //Console.WriteLine(bullets.Count);
            foreach(Bullet bullet in bullets)
            {
                bullet.Update(deltaTime);
            }

            //remove bullets off screen
            CheckBulletScreenBounds();
            CheckForIdle();

            base.Update(deltaTime);
            camera.SetCenterPosition(this.position);
        }

        public override void Draw(Surface surface, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(playerDrawable, this.position, deltaTime);

            foreach(Bullet bullet in bullets)
            {
                bullet.Draw(surface, deltaTime);
            }

            base.Draw(surface, deltaTime);
        }

        public void CheckForIdle()
        {
            if (Math.Abs(this.collider.velocity.X) < 2f && Math.Abs(this.collider.velocity.Y) < 2f)
            {
                this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                this.playerArm.animationController.SetActiveAnimation(playerArmClear);
            }
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
            Console.WriteLine("removed");
            world.RemoveCollider(bullet.collider);
            bullets.Remove(bullet);
        }

        public void HandleInput(float deltaTime)
        {
            Keyboard keyboard = new Keyboard();
            float vel = 5;
            //Console.WriteLine(camera.GetTopLeftScreenBounds() + ", " + camera.GetBottomRightScreenBounds());

            //up keydown
            if (keyboard.IsKeyDown(Key.KeyCode.W) 
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(0, -vel));
                this.baseSprite.animationController.SetActiveAnimation(runUpAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmUp);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponUp);
                this.playerDrawable.drawPartsInFront = false;
            }

            //down keydown
            if (keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(0, vel));
                this.baseSprite.animationController.SetActiveAnimation(runDownAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmDown);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);
                this.playerDrawable.drawPartsInFront = true;
            }

            //left keydown
            if (keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(-vel, 0));
                this.baseSprite.animationController.SetActiveAnimation(runLeftAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmLeft);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponLeft);
                this.playerDrawable.drawPartsInFront = true;
            }

            //right keydown
            if (keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(vel, 0));
                this.baseSprite.animationController.SetActiveAnimation(runRightAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmRight);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponRight);
                this.playerDrawable.drawPartsInFront = false;
            }

            //up keyup
            if (keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(0, -vel));
                this.baseSprite.animationController.SetActiveAnimation(runUpAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmUp);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponUp);
                this.playerDrawable.drawPartsInFront = false;
            }

            //down keyup
            if (keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(0, vel));
                this.baseSprite.animationController.SetActiveAnimation(runDownAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmDown);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);
                this.playerDrawable.drawPartsInFront = true;
            }

            //left keyup
            if (keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(-vel, 0));
                this.baseSprite.animationController.SetActiveAnimation(runLeftAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmLeft);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponLeft);
                this.playerDrawable.drawPartsInFront = true;
            }

            //right keyup
            if (keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(vel, 0));
                this.baseSprite.animationController.SetActiveAnimation(runRightAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmRight);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponRight);
                this.playerDrawable.drawPartsInFront = false;
            }

            // now we need horizontal directions

            //w & a
            if (keyboard.IsKeyDown(Key.KeyCode.W) && keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(-vel, -vel));
            }

            //a & s
            if (keyboard.IsKeyDown(Key.KeyCode.A) && keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(-vel, vel));
            }

            //s & d
            if (keyboard.IsKeyDown(Key.KeyCode.S) && keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(vel, vel));
            }

            //d & w
            if (keyboard.IsKeyDown(Key.KeyCode.D) && keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(vel, -vel));
            }

            //w & s
            if (keyboard.IsKeyDown(Key.KeyCode.W) && keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(0, 0));
            }

            //a & d
            if (keyboard.IsKeyDown(Key.KeyCode.A) && keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(0, 0));
            }

            //Console.WriteLine(mouse.GetMouseWorldPosition());
            //space
            if (keyboard.IsKeyDown(Key.KeyCode.Space))
            {
                //Console.WriteLine(timeSinceLastFire + ", " + timeToFire);
                if (timeSinceLastFire > timeToFire)
                {
                    Vector2 target = mouse.GetMouseWorldPosition();

                    Vector2 direction = target - this.position;
                    direction.Normalize();

                    //Console.WriteLine(target);
                    //Console.WriteLine(direction);
                    float velocity = 50000;
                    //Console.WriteLine(direction * velocity);

                    Bullet newBullet = new Bullet("bullet", new Vector2(this.position.X + 32, this.position.Y + 32), this.world, direction * velocity);
                    newBullet.collider.AddVelocity(direction * (velocity));
                    DestroyBullet bulletCallback = DeleteBullet;
                    newBullet.collider.CreateOnCollisionEnter("box1", () => bulletCallback(newBullet));
                    bullets.Add(newBullet);

                    //reset fire.
                    //timeSinceLastFire = new TimeSpan(0, 0, fireRateSeconds);

                    timeSinceLastFire = new TimeSpan();
                }
            }
            //Console.WriteLine((int)(deltaTime * 1000));
            timeSinceLastFire += new TimeSpan(0, 0, 0, 0,(int)(deltaTime * 1000));
            //timeSinceLastFire.Milliseconds += (int)(deltaTime * 1000);

            //Console.WriteLine(this.position + ", " + mouse.GetMouseWorldPosition() + ", " + this.collider.velocity);
        }
    }
}
