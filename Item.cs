using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Item : VisibleObject
    {
        public BoxCollider collider;
        public Vector2 colliderOffset;

        public Texture atexture;
        public AnimatedSprite asprite;

        public Animation defaultAnimation;
        public Animation downRunAnimation;

		public bool isFinished = false;


        public Item(string tag, string texFilePath, Vector2 frameSize, Vector2 colliderSize, Vector2 pos, World world, Vector2 colliderOffset) : base(tag)
        {
            this.position = pos;

            atexture = new Texture(texFilePath);

            asprite = new AnimatedSprite(atexture, (int)frameSize.X, (int)frameSize.Y, 0);

            this.colliderOffset = colliderOffset;
            collider = new BoxCollider(tag, colliderSize, this.position);
            
            //collider.isStatic = true;
            //this.collider.mass = 100000000;
            world.AddCollider(collider);

            this.objectDrawable = asprite;

            collider.debug = true;
			this.isAlive = false;
        }

        public void CreateAnimation(string animName, int frame, int numOfFrames)
        {
            // Create animations
            Animation newAnim = new Animation(animName, frame, frame);
            //defaultAnimation = new Animation("downRun", 11, 7);

            // Add animations
            asprite.AddAnimation(newAnim);
            //asprite.AddAnimation(downRunAnimation);

            // Test animation
            asprite.animationController.SetActiveAnimation(newAnim);
        }


        public override void Update(float deltaTime)
        {
            this.collider.CalculatePoints();
            this.position = collider.position + this.colliderOffset;
            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(asprite);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }
    }
}
