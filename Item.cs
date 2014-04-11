using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Item : BaseObject
    {
        BoxCollider collider;

        public Texture atexture;
        public AnimatedSprite asprite;
        public Animation defaultAnimation;
        public Animation downRunAnimation;


        public Item(string tag, Vector2 pos, World world) : base(tag)
        {
            this.position = pos;

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

            collider = new BoxCollider(tag, new Vector2(64, 64), this.position);
            world.AddCollider(collider);
        }


        public override void Update(float deltaTime)
        {
            this.collider.CalculatePoints();
            this.position = collider.position;
            base.Update(deltaTime);
        }

        public override void Draw(Surface surface)
        {
            surface.Draw(asprite, this.position);

            base.Draw(surface);
        }
    }
}
