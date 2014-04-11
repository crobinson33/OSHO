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
        public Animation defaultAnimation;
        public Animation downRunAnimation;

        public CharacterCollider collider;

        public Player(string tag, Vector2 position, World world) : base(tag)
        {
            Console.WriteLine("player start...");
            this.position = position;


            atexture = new Texture("HunchWideSprites.png");

            asprite = new AnimatedSprite(atexture, 64, 64);

            
            // Create animations
            downRunAnimation = new Animation("default", 10, 10);
            defaultAnimation = new Animation("downRun", 11, 7);

            // Add animations
            asprite.AddAnimation(defaultAnimation);
            asprite.AddAnimation(downRunAnimation);

            // Test animation
            asprite.animationController.SetActiveAnimation(downRunAnimation);

            collider = new CharacterCollider("player", new Vector2(64, 64), this.position);
            world.AddCollider(collider);

            Console.WriteLine("player end...");
        }

        public override void Update(float deltaTime)
        {
            this.collider.CalculatePoints();
            this.position = this.collider.position;
            //asprite.Update(this.position);
            HandleInput();
            //Console.WriteLine("getting called...");
            base.Update(deltaTime);
        }

        public override void Draw(Surface surface)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(asprite, this.position);


            base.Draw(surface);
        }

        public void HandleInput()
        {
            Keyboard keyboard = new Keyboard();
            float vel = 50;

            //up
            if (keyboard.IsKeyDown(Key.KeyCode.W))
            {
                this.collider.AddVelocity(new Vector2(0, -vel));
            }

            //down
            if (keyboard.IsKeyDown(Key.KeyCode.S))
            {
                this.collider.AddVelocity(new Vector2(0, vel));
            }

            //left
            if (keyboard.IsKeyDown(Key.KeyCode.A))
            {
                this.collider.AddVelocity(new Vector2(-vel, 0));
            }

            //right
            if (keyboard.IsKeyDown(Key.KeyCode.D))
            {
                this.collider.AddVelocity(new Vector2(vel, 0));
            }

            //Console.WriteLine(this.position + ", " + this.collider.position + ", " + this.collider.velocity);
        }
    }
}
