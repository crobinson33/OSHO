using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Background : BaseObject
    {
        public Texture bgtex;
        public Sprite bg;

        public Background(string tag, Vector2 position)  : base(tag)
        {
            bgtex = new Texture("assets/background.png");
            bg = new Sprite(bgtex, 640, 400);
            this.position = position;
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            diffuseSurface.Draw(bg, position, deltaTime);

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }
    }
}
