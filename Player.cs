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
        public Texture meleeTexture;
        public MultiDrawable playerDrawable;

        //public Texture playerWeapon;
        public AnimatedSprite baseSprite;
        public AnimatedSprite playerWeaponSprite;
        public AnimatedSprite playerArm;
        public AnimatedSprite playerMeleeWeapon;

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

        public Animation meleeAnimation;
        public Animation bellSwing;
        public Animation bellClear;

        public Shader test;

        public Camera camera;
        public Keyboard keyboard;

        public CharacterCollider collider;
		Vector2 colliderOffset;
		public BoxCollider meleeCollider;
        public BoxCollider walkCollider;
        Vector2 walkColliderOffset;

        List<Bullet> bullets = new List<Bullet>();
        World world;

        //Mouse mouse;

        public delegate void DestroyBullet(Bullet bullet);
		public delegate void HurtPlayer();
		public delegate void CheckMelee();
		public bool meleeButtonDown = false;
        public bool canMelee = true;
        public delegate void CheckButton();


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

        public Player(string tag, Vector2 position, World world, Mouse mouse, Camera camera, Keyboard keyboard) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;
            this.keyboard = keyboard;


            atexture = new Texture("assets/Hunch2.png");
            meleeTexture = new Texture("assets/Bell.png");

            baseSprite = new AnimatedSprite(atexture, 64, 64, 48);
            playerWeaponSprite = new AnimatedSprite(atexture, 64, 64);
            playerArm = new AnimatedSprite(atexture, 64, 64);
            playerMeleeWeapon = new AnimatedSprite(meleeTexture, 128, 128);

            playerDrawable = new MultiDrawable(baseSprite);
            playerDrawable.AddDrawable(playerArm);
            playerDrawable.AddDrawable(playerWeaponSprite);
            playerDrawable.AddDrawable(playerMeleeWeapon);

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

            meleeAnimation = new Animation("playerMelee", 170, 8);
            bellSwing = new Animation("bellSwing", 1, 8);
            bellClear = new Animation("bellClear", 0, 1);

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
            baseSprite.AddAnimation(meleeAnimation);

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

            playerMeleeWeapon.AddAnimation(bellSwing);
            playerMeleeWeapon.AddAnimation(bellClear);
            playerMeleeWeapon.animationController.SetActiveAnimation(bellClear);
            playerMeleeWeapon.drawingOffset = new Vector2(-32, -32);


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
            this.collider.AddTagToIgnore("characterWalk");
            //this.collider.AddTagToIgnore("tree");
            //this.collider.isStatic = true;
            this.collider.mass = 100;
			this.collider.clearVelocityAmount = 0.91f;

			//add on collision enter with enemies.
			HurtPlayer damageCallback = TakeDamage;
			this.collider.CreateOnCollisionEnter("enemy", () => damageCallback());

			// our melee weapons

			this.meleeCollider = new BoxCollider("characterMelee", new Vector2(96, 96), new Vector2(this.position.X - 16, this.position.Y - 16));
			this.meleeCollider.isStatic = true; // i think making it static will allow us to modify position instead of it modifying our position
			this.meleeCollider.AddTagToIgnore("one");
            this.meleeCollider.AddTagToIgnore("characterWalk");
			this.meleeCollider.AddTagToIgnore("bullet");

			//CheckMelee meleeCallback = CheckMeleeRange;
			//this.meleeCollider.CreateOnCollisionEnter("enemy", () => meleeCallback());
			this.meleeCollider.debug = true;

            // Walk collider on his little feetsies
            this.walkColliderOffset = new Vector2(26, 41);
            this.walkCollider = new BoxCollider("characterWalk", new Vector2(12, 7), this.position + this.walkColliderOffset);
            //this.walkCollider.isStatic = true; // i think making it static will allow us to modify position instead of it modifying our position
            this.walkCollider.AddTagToIgnore("one");
            this.walkCollider.AddTagToIgnore("bullet");
            this.walkCollider.AddTagToIgnore("characterMelee");
            this.walkCollider.AddTagToIgnore("enemy");
            this.walkCollider.AddTagToIgnore("buttonOne");

            CheckButton buttonCallback = CheckButtonDown;
            this.walkCollider.CreateOnCollisionEnter("buttonOne", () => buttonCallback());

            this.walkCollider.debug = true;

            this.world.AddCollider(this.walkCollider);
            this.world.AddCollider(this.collider);
			this.world.AddCollider(this.meleeCollider);

            this.objectDrawable = playerDrawable;

            //this.mouse = mouse;

            this.collider.debug = true;

            Console.WriteLine("player end...");
        }

        public override void Update(float deltaTime)
        {
            //this.collider.CalculatePoints();
            this.position = this.walkCollider.position - this.walkColliderOffset;
			this.meleeCollider.position = new Vector2(this.position.X - 16, this.position.Y - 16);
            this.collider.position = this.position - this.colliderOffset;

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

            Console.WriteLine(playerMeleeWeapon.animationController.GetActiveAnimationName());

            //remove bullets off screen
            CheckBulletScreenBounds();
            CheckForIdle();

            base.Update(deltaTime);
            camera.SetCenterPosition(Util.Lerp(camera.GetCurCenter(), this.position, 0.08f));

            if (collider.debug)
            {
                collider.UpdateVertices();
            }
			if (this.meleeCollider.debug)
			{
				this.meleeCollider.UpdateVertices();
			}
            if (this.walkCollider.debug)
            {
                this.walkCollider.UpdateVertices(Color.Magenta);
            }
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            diffuseSurface.Draw(playerDrawable);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

			if (this.meleeCollider.debug)
			{
				this.meleeCollider.DrawDebugBox(diffuseSurface, deltaTime);
			}

            if (this.walkCollider.debug)
            {
                this.walkCollider.DrawDebugBox(diffuseSurface, deltaTime);
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

        public void CheckButtonDown()
        {
            //Console.WriteLine("on button");
            if (keyboard.IsKeyDown(Key.KeyCode.E))
            {
                Console.WriteLine("on button and e down");
                // do button event here.
            }
        }

		public void TakeDamage()
		{
			// not in our shell
			if (inSheild != true)
			{
				Console.WriteLine("i got hit!");
				//this.collider.isStatic = false;
			}
			else
			{
                Console.WriteLine("im safe!");
				//this.collider.isStatic = true;
			}
		}

        public void CheckForIdle()
        {
            //aConsole.WriteLine("---");
            if (Math.Abs(this.walkCollider.velocity.X) < 2.5f && Math.Abs(this.walkCollider.velocity.Y) < 2.5f)
            {
                if (this.baseSprite.animationController.GetActiveAnimationName() != "idleFire" && this.baseSprite.animationController.GetActiveAnimationName() != "playerMelee")
                {
                    if (CheckIfInShell() == false)
                    {
                        //Console.WriteLine("setting idle");
                        this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                        this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                        this.playerArm.animationController.SetActiveAnimation(playerArmClear);
                    }
                }
            }
            else
            {
                if (this.baseSprite.animationController.GetActiveAnimationName() == "idleDown")
                {
                    Console.WriteLine(this.baseSprite.animationController.GetActiveAnimationName() + ", we arent idle, but have idle anim D:");
                    if (Math.Abs(this.walkCollider.velocity.X) > Math.Abs(this.walkCollider.velocity.Y))
                    {
                        //we know it's left or right.
                        if (this.walkCollider.velocity.X > 0)
                        {
                            // we know we are walking right.
                            PlayInputAnim(runRightAnimation, playerArmRight, weaponRight);
                            this.playerDrawable.drawPartsInFront = false;
                        }
                        else
                        {
                            PlayInputAnim(runLeftAnimation, playerArmLeft, weaponLeft);
                            this.playerDrawable.drawPartsInFront = true;
                        }
                    }
                    else
                    {
                        //we know its up or down.
                        if (this.walkCollider.velocity.Y > 0)
                        {
                            // we know down.
                            PlayInputAnim(runDownAnimation, playerArmDown, weaponDown);
                            this.playerDrawable.drawPartsInFront = true;
                        }
                        else
                        {
                            PlayInputAnim(runUpAnimation, playerArmUp, weaponUp);
                            this.playerDrawable.drawPartsInFront = false;
                        }
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

        public bool CheckIfInShell()
        {
            if (this.baseSprite.animationController.GetActiveAnimationName() == "shellShine" ||
                        this.baseSprite.animationController.GetActiveAnimationName() == "startShell" ||
                        this.baseSprite.animationController.GetActiveAnimationName() == "endShell" ||
                        this.baseSprite.animationController.GetActiveAnimationName() == "endShellWarning")
            {
                return true;
            }
            return false;
        }

        public void PlayInputAnim(Animation baseAnimation, Animation armAnimation, Animation weaponAnimation)
        {
            if (this.baseSprite.animationController.GetActiveAnimationName() == "playerMelee")
            {
                if (this.baseSprite.animationController.hasReachedEnd)
                {
                    this.baseSprite.animationController.SetActiveAnimation(baseAnimation);
                    this.playerArm.animationController.SetActiveAnimation(armAnimation);
                    this.playerWeaponSprite.animationController.SetActiveAnimation(weaponAnimation);
                }
            }
            else
            {
                this.baseSprite.animationController.SetActiveAnimation(baseAnimation);
                this.playerArm.animationController.SetActiveAnimation(armAnimation);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponAnimation);
            }
        }

        public void HandleInput(float deltaTime)
        {
            //Keyboard keyboard = new Keyboard();
            float vel = 5;
            //Console.WriteLine(camera.GetTopLeftScreenBounds() + ", " + camera.GetBottomRightScreenBounds());

            //up keydown
            if (keyboard.IsKeyDown(Key.KeyCode.W) 
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.D))
            {
                
                this.walkCollider.AddVelocity(new Vector2(0, -vel));
                /*this.baseSprite.animationController.SetActiveAnimation(runUpAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmUp);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponUp);*/
                PlayInputAnim(runUpAnimation, playerArmUp, weaponUp);
                this.playerDrawable.drawPartsInFront = false;

            }

            //down keydown
            if (keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.walkCollider.AddVelocity(new Vector2(0, vel));
                /*this.baseSprite.animationController.SetActiveAnimation(runDownAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmDown);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponDown);*/
                PlayInputAnim(runDownAnimation, playerArmDown, weaponDown);
                this.playerDrawable.drawPartsInFront = true;
            }

            //left keydown
            if (keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.W)
                && !keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.walkCollider.AddVelocity(new Vector2(-vel, 0));
                /*this.baseSprite.animationController.SetActiveAnimation(runLeftAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmLeft);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponLeft);*/
                PlayInputAnim(runLeftAnimation, playerArmLeft, weaponLeft);
                this.playerDrawable.drawPartsInFront = true;
            }

            //right keydown
            if (keyboard.IsKeyDown(Key.KeyCode.D)
                && !keyboard.IsKeyDown(Key.KeyCode.A)
                && !keyboard.IsKeyDown(Key.KeyCode.S)
                && !keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.walkCollider.AddVelocity(new Vector2(vel, 0));
                /*this.baseSprite.animationController.SetActiveAnimation(runRightAnimation);
                this.playerArm.animationController.SetActiveAnimation(playerArmRight);
                this.playerWeaponSprite.animationController.SetActiveAnimation(weaponRight);*/
                PlayInputAnim(runRightAnimation, playerArmRight, weaponRight);
                this.playerDrawable.drawPartsInFront = false;
            }

            // now we need horizontal directions

            //w & a
            if (keyboard.IsKeyDown(Key.KeyCode.W) && keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.walkCollider.AddVelocity(new Vector2(-vel, -vel));
            }

            //a & s
            if (keyboard.IsKeyDown(Key.KeyCode.A) && keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.walkCollider.AddVelocity(new Vector2(-vel, vel));
            }

            //s & d
            if (keyboard.IsKeyDown(Key.KeyCode.S) && keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.walkCollider.AddVelocity(new Vector2(vel, vel));
            }

            //d & w
            if (keyboard.IsKeyDown(Key.KeyCode.D) && keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.walkCollider.AddVelocity(new Vector2(vel, -vel));
            }

            //w & s
            if (keyboard.IsKeyDown(Key.KeyCode.W) && keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.walkCollider.AddVelocity(new Vector2(0, 0));
            }

            //a & d
            if (keyboard.IsKeyDown(Key.KeyCode.A) && keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.walkCollider.AddVelocity(new Vector2(0, 0));
            }

			// melee key (,)
			if (keyboard.IsKeyDown(Key.KeyCode.Comma))
			{
				meleeButtonDown = true;
                if (canMelee)
                {
                    canMelee = false;
                    //this.baseSprite.animationController.dontLoop = false;
                    this.baseSprite.animationController.SetActiveAnimation(meleeAnimation);
                    this.baseSprite.animationController.dontLoop = true;

                    this.playerWeaponSprite.animationController.SetActiveAnimation(weaponClear);
                    this.playerArm.animationController.SetActiveAnimation(playerArmClear);

                    this.playerMeleeWeapon.animationController.SetActiveAnimation(bellSwing);
                    this.playerMeleeWeapon.animationController.dontLoop = true;

                }

                //Console.WriteLine("checking button");
                // if these are true means we are holding the button down.
                if (this.playerMeleeWeapon.animationController.hasReachedEnd)
                {
                    this.playerMeleeWeapon.animationController.SetActiveAnimation(bellClear);
                }

                if (this.baseSprite.animationController.GetActiveAnimationName() == "playerMelee" && this.baseSprite.animationController.hasReachedEnd)
                {
                    this.playerMeleeWeapon.animationController.SetActiveAnimation(bellClear);
                }
			}
            else
			{
				meleeButtonDown = false;
                canMelee = true;

                if (this.baseSprite.animationController.GetActiveAnimationName() == "playerMelee")
                {
                    if (this.baseSprite.animationController.hasReachedEnd)
                    {
                        //Console.WriteLine("reached end");
                        this.baseSprite.animationController.SetActiveAnimation(idleDownAnimation);
                        this.playerMeleeWeapon.animationController.SetActiveAnimation(bellClear);
                        canMelee = true;
                    }
                }
                else
                {
                    //since this will be called every frame the comma isn't done we dont want to mess with how the shell works.
                    if (CheckIfInShell() == false)
                    {
                        this.baseSprite.animationController.dontLoop = false;
                    }
                }

                // we want to play full bell anim
                if (this.playerMeleeWeapon.animationController.GetActiveAnimationName() == "bellSwing" && this.playerMeleeWeapon.animationController.hasReachedEnd)
                {
                    this.playerMeleeWeapon.animationController.SetActiveAnimation(bellClear);
                    this.playerMeleeWeapon.animationController.dontLoop = false;
                }
                
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
            //DestroyBullet bulletCallback2 = DeleteBullet;
            //DestroyBullet bulletCallback3 = DeleteBullet;
			newBullet.collider.CreateOnCollisionEnter("box1", () => bulletCallback(newBullet));
            newBullet.collider.CreateOnCollisionEnter("bigEye", () => bulletCallback(newBullet));
            newBullet.collider.CreateOnCollisionEnter("littleEye", () => bulletCallback(newBullet));
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
