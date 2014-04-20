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
        public Animation idleDownFire;
        //public Animation idleUpAnimation;
        //public Animation idleLeftAnimation;
        //public Animation idleRightAnimation;


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

        public Animation playerStartShell;
        public Animation playerEndShell;
        public Animation shellShine;
        public Animation shellEndWarning;

        public Shader test;

        public Camera camera;

        public CharacterCollider collider;
		Vector2 colliderOffset;
		public BoxCollider meleeCollider;

        List<Bullet> bullets = new List<Bullet>();
        World world;

        //Mouse mouse;

        public delegate void DestroyBullet(Bullet bullet);
		public delegate void HurtPlayer();
		public delegate void CheckMelee();
		public bool meleeButtonDown = false;


		// for the sheild.
		public bool inSheild = false;
		public bool sheildOnCooldown = false;
		TimeSpan timeInSheild = new TimeSpan(0, 0, 0);
		TimeSpan timeAllowedInSheild = new TimeSpan(0, 0, 4);
		TimeSpan timeSinceLastSheild = new TimeSpan(0, 0, 0);
		TimeSpan sheildCooldown = new TimeSpan(0, 0, 5);


        // for fire rate.
        //int fireRateSeconds = 1;
        TimeSpan timeSinceLastFire = new TimeSpan(0, 0, 0); // can't pass a variable in here so make it the same as firerate
        TimeSpan timeToFire = new TimeSpan(0, 0, 0, 0, 150);
        //bool hasFired = false;

        public Player(string tag, Vector2 position, World world, Mouse mouse, Camera camera) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;


            atexture = new Texture("assets/Hunch2.png");

            baseSprite = new AnimatedSprite(atexture, 64, 64);
            playerWeaponSprite = new AnimatedSprite(atexture, 64, 64);
            playerArm = new AnimatedSprite(atexture, 64, 64);

            playerDrawable = new MultiDrawable(baseSprite);
            playerDrawable.AddDrawable(playerArm);
            playerDrawable.AddDrawable(playerWeaponSprite);

            this.camera = camera;
            
            // Create animations
            idleDownAnimation = new Animation("idleDown", 20, 7);
            idleDownFire = new Animation("idleFire", 50, 7);
            //idleUpAnimation = new Animation("idleUp", 30, 7);
            //idleRightAnimation = new Animation("idleRight", 40, 7);
            //idleLeftAnimation = new Animation("idleLeft", 40, 7, true);

            //runs.
            runDownAnimation = new Animation("runDown", 90, 10);
            runUpAnimation = new Animation("runUp", 100, 10);
            runRightAnimation = new Animation("runRight", 110, 10);
            runLeftAnimation = new Animation("runLeft", 120, 10);

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

            playerStartShell = new Animation("startShell", 130, 4);
            playerEndShell = new Animation("endShell", 140, 4);
            shellShine = new Animation("shellShine", 150, 8);
            shellEndWarning = new Animation("endShellWarning", 160, 8);

            // Add animations
            baseSprite.AddAnimation(idleDownAnimation);
            baseSprite.AddAnimation(idleDownFire);
            //baseSprite.AddAnimation(idleUpAnimation);
            //baseSprite.AddAnimation(idleRightAnimation);
            //baseSprite.AddAnimation(idleLeftAnimation);
            baseSprite.AddAnimation(runDownAnimation);
            baseSprite.AddAnimation(runUpAnimation);
            baseSprite.AddAnimation(runRightAnimation);
            baseSprite.AddAnimation(runLeftAnimation);
            baseSprite.AddAnimation(playerStartShell);
            baseSprite.AddAnimation(playerEndShell);
            baseSprite.AddAnimation(shellShine);
            baseSprite.AddAnimation(shellEndWarning);

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
            baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
            playerDrawable.drawPartsInFront = false;
            playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);
            playerArm.animationController.SetActiveAnimation(playerArmDown);

            this.world = world;
			colliderOffset = new Vector2(-20, -10);
            this.collider = new CharacterCollider(tag, new Vector2(23, 42), this.position + this.colliderOffset);
            this.collider.AddTagToIgnore("bullet");
			this.collider.AddTagToIgnore("characterMelee");
			this.collider.clearVelocityAmount = 0.91f;

			//add on collision enter with enemies.
			HurtPlayer damageCallback = TakeDamage;
			this.collider.CreateOnCollisionEnter("enemy", () => damageCallback());

			// our melee weapons

			this.meleeCollider = new BoxCollider("characterMelee", new Vector2(128, 128), new Vector2(this.position.X - 32, this.position.Y - 32));
			this.meleeCollider.isStatic = true; // i think making it static will allow us to modify position instead of it modifying our position
			this.meleeCollider.AddTagToIgnore("one");
			this.meleeCollider.AddTagToIgnore("bullet");

			//CheckMelee meleeCallback = CheckMeleeRange;
			//this.meleeCollider.CreateOnCollisionEnter("enemy", () => meleeCallback());
			this.meleeCollider.debug = true;



            this.world.AddCollider(this.collider);
			this.world.AddCollider(this.meleeCollider);


            //this.mouse = mouse;

            this.collider.debug = true;

            Console.WriteLine("player end...");
        }

        public override void Update(float deltaTime)
        {
            //this.collider.CalculatePoints();
            this.position = this.collider.position + this.colliderOffset;
			this.meleeCollider.position = new Vector2(this.position.X - 32, this.position.Y - 32);
			//Console.WriteLine ("pos: " + this.position + ", col pos: " + this.meleeCollider.position);
            //asprite.Update(this.position);
            HandleInput(deltaTime);
            //Console.WriteLine("getting called...");

            //Console.WriteLine(bullets.Count);
            foreach(Bullet bullet in bullets)
            {
                bullet.Update(deltaTime);

                if (bullet.collider.debug)
                {
                    bullet.collider.UpdateVertices();
                }
            }

            //remove bullets off screen
            CheckBulletScreenBounds();
            CheckForIdle();

            base.Update(deltaTime);
            camera.SetCenterPosition(this.position);

            if (collider.debug)
            {
                collider.UpdateVertices();
            }
			if (this.meleeCollider.debug)
			{
				this.meleeCollider.UpdateVertices();
			}
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            diffuseSurface.Draw(playerDrawable, this.position, deltaTime);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

			if (this.meleeCollider.debug)
			{
				this.meleeCollider.DrawDebugBox(diffuseSurface, deltaTime);
			}

            foreach(Bullet bullet in bullets)
            {
                bullet.Draw(diffuseSurface, lightMap, deltaTime);

                if (bullet.collider.debug)
                {
                    bullet.collider.DrawDebugBox(diffuseSurface, deltaTime);
                }
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

		/*public void CheckMeleeRange() // moved to the enemy class
		{
			// this will be called when enemies are in melee range.
			// if our key is down we can do damage.
			Console.WriteLine ("melee range!");
			Console.WriteLine ("do we do dmg: " + meleeButtonDown);
		}*/

		public void TakeDamage()
		{
			// not in our shell
			if (inSheild != true)
			{
				Console.WriteLine("i got hit!");
				this.collider.isStatic = false;
			}
			else
			{
				this.collider.isStatic = true;
			}
		}

        public void CheckForIdle()
        {
            /*playerStartShell = new Animation("startShell", 130, 4);
            playerEndShell = new Animation("endShell", 140, 4);
            shellShine = new Animation("shellShine", 150, 8);
            shellEndWarning = new Animation("endShellWarning", 160, 8);*/

            if (Math.Abs(this.collider.velocity.X) < 2f && Math.Abs(this.collider.velocity.Y) < 2f)
            {
                if (this.baseSprite.animationController.GetActiveAnimationName() != "idleFire")
                {
                    if (this.baseSprite.animationController.GetActiveAnimationName() != "shellShine" &&
                        this.baseSprite.animationController.GetActiveAnimationName() != "startShell" &&
                        this.baseSprite.animationController.GetActiveAnimationName() != "endShell" &&
                        this.baseSprite.animationController.GetActiveAnimationName() != "endShellWarning")
                    {
                        //Console.WriteLine("setting idle");
                        this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                        this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                        this.playerArm.animationController.SetActiveAnimation(playerArmClear);
                    }
                }
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
            //Console.WriteLine("removed");
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

			// melee key (,)
			if (keyboard.IsKeyDown(Key.KeyCode.Comma))
			{
				meleeButtonDown = true;
			}

			// reset melee
			if (keyboard.IsKeyDown(Key.KeyCode.Comma) == false)
			{
				meleeButtonDown = false;
			}

			//Console.WriteLine (inSheild);
			// sheild key (.)
			if (keyboard.IsOnlyKeyDown(Key.KeyCode.Period))
			{
				CheckSheild(deltaTime);
			}
			else
			{
                //Console.WriteLine(inSheild);
                // check to see if we had this button
                if (inSheild)
                {
                    // we ending shell
                    this.baseSprite.animationController.SetActiveAnimation(playerEndShell);
                    this.baseSprite.animationController.dontLoop = true;
                }

                // see if we have finished yet.
                if (this.baseSprite.animationController.GetActiveAnimationName() == "endShell")
                {
                    this.baseSprite.animationController.dontLoop = false;
                    this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                    inSheild = false;
                }
			}

			// need to add to the cooldown whether we have the button down or not.
			if (sheildOnCooldown)
			{
				timeSinceLastSheild += new TimeSpan(0, 0, 0, 0,(int)(deltaTime * 1000));
			}


            //Console.WriteLine(mouse.GetMouseWorldPosition());
            //space
            if (keyboard.IsKeyDown(Key.KeyCode.Space))
            {
                //Console.WriteLine(timeSinceLastFire + ", " + timeToFire);
                if (timeSinceLastFire > timeToFire)
                {
					/*
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
					newBullet.collider.CreateOnCollisionEnter("enemy", () => bulletCallback(newBullet));
                    bullets.Add(newBullet);

                    //reset fire.
                    //timeSinceLastFire = new TimeSpan(0, 0, fireRateSeconds);*/

					//Console.WriteLine (	this.baseSprite.animationController.GetActiveAnimationName() );

					// for reference while building
					/*runDownAnimation = new Animation("runDown", 50, 10);
					runUpAnimation = new Animation("runUp", 60, 10);
					runRightAnimation = new Animation("runRight", 70, 10);
					runLeftAnimation = new Animation("runLeft", 80, 10);*/

					float bulletVelocity = 500;

					switch (this.baseSprite.animationController.GetActiveAnimationName() )
					{
						case "runDown":
							//Console.WriteLine("Case 1");
							Vector2 velocityToAdd = new Vector2(0, bulletVelocity);
                            Vector2 spawnPosition = new Vector2(this.position.X + 30, this.position.Y + 44);
							FireBullet(velocityToAdd, spawnPosition);
							break;
						case "runUp":
							//Console.WriteLine("Case 2");
							Vector2 velocityToAdd2 = new Vector2(0, -(bulletVelocity));
                            Vector2 spawnPosition2 = new Vector2(this.position.X + 28, this.position.Y);
							FireBullet(velocityToAdd2, spawnPosition2);
							break;
						case "runRight":
							//Console.WriteLine("Case 2");
							Vector2 velocityToAdd3 = new Vector2(bulletVelocity, 0);
                            Vector2 spawnPosition3 = new Vector2(this.position.X + 50, this.position.Y + 24);
							FireBullet(velocityToAdd3, spawnPosition3);
							break;
						case "runLeft":
							//Console.WriteLine("Case 2");
							Vector2 velocityToAdd4 = new Vector2(-(bulletVelocity), 0);
                            Vector2 spawnPosition4 = new Vector2(this.position.X + 10, this.position.Y + 24);
							FireBullet(velocityToAdd4, spawnPosition4);
							break;
                        case "idleDown":
                        case "idleFire":
                            // animation stuff
                            //this.collider.AddVelocity(new Vector2(0, vel));
                            this.baseSprite.animationController.SetActiveAnimation(idleDownFire);
                            this.playerArm.animationController.SetActiveAnimation(playerArmDown);
                            this.playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);
                            this.playerDrawable.drawPartsInFront = true;

                            // bullet stuff
                            Vector2 velocityToAdd5 = new Vector2(0, bulletVelocity);
                            Vector2 spawnPosition5 = new Vector2(this.position.X + 30, this.position.Y + 44);
							FireBullet(velocityToAdd5, spawnPosition5);

							break;
						default:
							Console.WriteLine("Default case");
							break;
					}

                    timeSinceLastFire = new TimeSpan();
                }
            }
            else
            {
                if (this.baseSprite.animationController.GetActiveAnimationName() == "idleFire")
                {
                    this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                    this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                    this.playerArm.animationController.SetActiveAnimation(playerArmClear);
                }
            }
            //Console.WriteLine((int)(deltaTime * 1000));
            timeSinceLastFire += new TimeSpan(0, 0, 0, 0,(int)(deltaTime * 1000));
            //timeSinceLastFire.Milliseconds += (int)(deltaTime * 1000);

            //Console.WriteLine(this.position + ", " + mouse.GetMouseWorldPosition() + ", " + this.collider.velocity);
        }

		public void FireBullet(Vector2 velocityToAdd, Vector2 spawnPosition)
		{
			Bullet newBullet = new Bullet("bullet", spawnPosition, this.world, velocityToAdd);
			newBullet.collider.AddVelocity(velocityToAdd);
			DestroyBullet bulletCallback = DeleteBullet;
            DestroyBullet bulletCallback2 = DeleteBullet;
			newBullet.collider.CreateOnCollisionEnter("box1", () => bulletCallback(newBullet));
			newBullet.collider.CreateOnCollisionEnter("enemy", () => bulletCallback2(newBullet));
			bullets.Add(newBullet);
		}

		public void CheckSheild(float deltaTime)
		{
			// if we have been holding the button.
			if (inSheild)
			{
                // did we just start? need to check animation
                if (this.baseSprite.animationController.GetActiveAnimationName() == "startShell")
                {
                    if (this.baseSprite.animationController.hasReachedEnd)
                    {
                        this.baseSprite.animationController.dontLoop = false;
                        this.baseSprite.animationController.hasReachedEnd = false;
                        this.baseSprite.animationController.SetActiveAnimation(shellShine);
                        
                    }
                }

                //Console.WriteLine(timeAllowedInSheild.Seconds + ", " + (timeInSheild.TotalSeconds / 2));
                // need to check if we expire soon. play flashing white.
                if (timeAllowedInSheild.Seconds /2 < timeInSheild.TotalSeconds)
                {
                    this.baseSprite.animationController.SetActiveAnimation(shellEndWarning);
                }


				// have we been in the sheild too long?
				if (timeAllowedInSheild < timeInSheild)
				{
					//yes
					inSheild = false;
					sheildOnCooldown = true;
					/*if (sheildCooldown < timeSinceLastSheild)
					{
						timeInSheild = new TimeSpan();
						timeSinceLastSheild = new TimeSpan();
						sheildOnCooldown = false;
					}*/

                    this.baseSprite.animationController.SetActiveAnimation(playerEndShell);
                    this.baseSprite.animationController.dontLoop = true;

                    
				}
				else
				{
					// no
					inSheild = true;
				}
				timeInSheild += new TimeSpan(0, 0, 0, 0,(int)(deltaTime * 1000));
			}
			else // we either had let go or ran out of sheild
			{
				if (sheildOnCooldown)
				{
					// is our cooldown up yet?
					if (sheildCooldown < timeSinceLastSheild)
					{
						timeInSheild = new TimeSpan();
						timeSinceLastSheild = new TimeSpan();
						sheildOnCooldown = false;
						inSheild = true;
                        this.baseSprite.animationController.SetActiveAnimation(playerStartShell);
                        this.baseSprite.animationController.dontLoop = true;
					}

                    // see if we have finished yet.
                    if (this.baseSprite.animationController.GetActiveAnimationName() == "endShell")
                    {
                        this.baseSprite.animationController.dontLoop = false;
                        this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                        //inSheild = false;
                    }
				}
				else
				{
                    //Console.WriteLine("started anim");
                    // start our animation -- need to check when this one is done.
                    this.baseSprite.animationController.SetActiveAnimation(playerStartShell);
                    this.baseSprite.animationController.dontLoop = true;
                    this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                    this.playerArm.animationController.SetActiveAnimation(playerArmClear);

					inSheild = true;
					timeInSheild = new TimeSpan();
					timeSinceLastSheild = new TimeSpan();
					sheildOnCooldown = false;
					inSheild = true;
				}
			}
		}
    }


}
