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

        public Background(string tag)  : base(tag)
        {
            bgtex = new Texture("assets/background.png");
            bg = new Sprite(bgtex, 640, 400);
        }

        public override void Draw(Surface surface, float deltaTime)
        {
            //Console.WriteLine("getting called...");
            surface.Draw(bg, new Vector2(0,0), deltaTime);

            base.Draw(surface, deltaTime);
        }
    }
}
