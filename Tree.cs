using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Tree : VisibleObject
    {
        public BoxCollider collider;
        Vector2 colliderOffset;

        public Texture texture;
        public Sprite sprite;
        public Tree(string tag, Vector2 pos, World world) : base(tag)
        {
            this.position = pos;
            texture = new Texture("assets/bigtree.png");
            sprite = new Sprite(texture, 160, 160, 150);

            colliderOffset = new Vector2(-64, -124);
            collider = new BoxCollider(tag, new Vector2(30, 30), this.position);
            collider.isStatic = true;
            this.collider.mass = 100000000;
            world.AddCollider(collider);

            this.objectDrawable = sprite;

            //collider.debug = true;
			this.isAlive = false;
        }

        public override void Update(float deltaTime)
        {
            this.position = this.collider.position + this.colliderOffset;
            base.Update(deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(sprite);

            if (collider.debug)
            {
                collider.DrawDebugBox(diffuseSurface, deltaTime);
            }

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }
    }
}
