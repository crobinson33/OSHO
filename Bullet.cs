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
        public AnimatedSprite aSprite;
        public Animation shooting;
        public float width;
        public float height;

        public BoxCollider collider;

        public Vector2 velocity;

        public Bullet(string tag, Vector2 pos, World world, Vector2 vel) : base(tag)
        {
            this.position = pos;

            texture = new Texture("assets/bullet.png");
            aSprite = new AnimatedSprite(texture, 16, 16);
            shooting = new Animation("Shooting", 0, 2);

            aSprite.AddAnimation(shooting);
            aSprite.animationController.SetActiveAnimation(shooting);

            width = texture.width;
            height = texture.height;

            collider = new BoxCollider(tag, new Vector2(width, height), this.position);
            collider.AddTagToIgnore("one");
            collider.AddTagToIgnore(tag);
            collider.clearVelocityAmount = 1;
            world.AddCollider(collider);

            this.velocity = vel;

            this.collider.debug = true;
        }

        public override void Update(float deltaTime)
        {
            //this.collider.AddVelocity(this.velocity);
            this.position = this.collider.position;
            base.Update(deltaTime);
        }

        public override void Draw(Surface surface, float deltaTime)
        {
            surface.Draw(aSprite, this.position, deltaTime);

            base.Draw(surface, deltaTime);
        }

        public void Dispose()
        {
            this.collider = null;
            this.texture = null;
            this.aSprite = null;
        }
    }
}
