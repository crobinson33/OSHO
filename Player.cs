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

        public Player(string tag, Vector2 position) : base(tag)
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

            Console.WriteLine("player end...");
        }

        public override void Update(float deltaTime)
        {
            //Console.WriteLine("getting called...");
            base.Update(deltaTime);
        }

        public override void Draw(Surface surface)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(asprite, new Vector2(300, 300));


            base.Draw(surface);
        }
    }
}
