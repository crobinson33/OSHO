using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Bullet : BaseObject
    {
        public Texture texture;
        public Sprite sprite;
        public float width;
        public float height;

        public BoxCollider collider;

        public Vector2 velocity;

        public Bullet(string tag, Vector2 pos, World world, Vector2 vel) : base(tag)
        {
            this.position = pos;

            texture = new Texture("laser.png");
            sprite = new Sprite(texture, new Vector2(0, 0), 60, 60);
            width = texture.width;
            height = texture.height;

            collider = new BoxCollider(tag, new Vector2(width, height), this.position);
            collider.AddTagToIgnore("player");
            collider.AddTagToIgnore(tag);
            world.AddCollider(collider);

            this.velocity = vel;
        }

        public override void Update(float deltaTime)
        {
            //this.collider.AddVelocity(this.velocity);
            this.position = this.collider.position;
            base.Update(deltaTime);
        }

        public override void Draw(Surface surface)
        {
            surface.Draw(sprite, this.position);

            base.Draw(surface);
        }

        public void Dispose()
        {
            this.collider = null;
            this.texture = null;
            this.sprite = null;
        }
    }
}
