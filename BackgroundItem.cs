using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class BackgroundItem : VisibleObject
    {
        public Texture atexture;
        public AnimatedSprite asprite;

        public Animation defaultAnimation;

        public BackgroundItem(string tag, string texFilePath, Vector2 frameSize, Vector2 pos) : base(tag)
        {
            this.position = pos;
            atexture = new Texture(texFilePath);

            asprite = new AnimatedSprite(atexture, (int)frameSize.X, (int)frameSize.Y, 0);

            defaultAnimation = new Animation("default", 0, 1);

            asprite.AddAnimation(defaultAnimation);
            asprite.animationController.SetActiveAnimation(defaultAnimation);

            this.objectDrawable = asprite;
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            //Console.WriteLine("drawing grass");
            diffuseSurface.Draw(asprite);

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }
    }
}
