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
        public BoxCollider collider;

        public Texture atexture;
        public AnimatedSprite asprite;
        public Animation defaultAnimation;
        public Animation downRunAnimation;


        public Item(string tag, Vector2 pos, World world) : base(tag)
        {
            this.position = pos;

            atexture = new Texture("assets/HunchSprite.png");

            asprite = new AnimatedSprite(atexture, 64, 64, 0);


            // Create animations
            downRunAnimation = new Animation("default", 10, 10);
            defaultAnimation = new Animation("downRun", 11, 7);

            // Add animations
            asprite.AddAnimation(defaultAnimation);
            asprite.AddAnimation(downRunAnimation);

            // Test animation
            asprite.animationController.SetActiveAnimation(downRunAnimation);

            collider = new BoxCollider(tag, new Vector2(64, 64), this.position);
            collider.isStatic = true;
            this.collider.mass = 100000000;
            world.AddCollider(collider);

            this.objectDrawable = asprite;

            collider.debug = true;
        }


        public override void Update(float deltaTime)
        {
            this.collider.CalculatePoints();
            this.position = collider.position;
            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(asprite, this.position, deltaTime);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }
    }
}
