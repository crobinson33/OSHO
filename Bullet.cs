using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBagel;

namespace OSHO
{
    public class Bullet : VisibleObject
    {
        public Texture texture;
        public AnimatedSprite aSprite;
        public Animation shooting;
        public Spotlight light;
        public float width;
        public float height;

        public BoxCollider collider;
        public Vector2 colliderOffset;

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

            colliderOffset = new Vector2(-3, -3);
            collider = new BoxCollider(tag, new Vector2(10, 10), this.position + this.colliderOffset);
            collider.AddTagToIgnore("one");
            collider.AddTagToIgnore(tag);
            collider.AddTagToIgnore("characterWalk");
			collider.AddTagToIgnore("characterMelee");
            collider.AddTagToIgnore("buttonOne");
            collider.AddTagToIgnore("buttonTwo");
            collider.AddTagToIgnore("buttonThree");
            collider.clearVelocityAmount = 1;
            world.AddCollider(collider);

            this.velocity = vel;

            this.collider.debug = true;

            this.light = new Spotlight(30, new Color(0.419f, 1f, 0f), this.collider.position, 100, 0.1f, true, true, 1.5f, 20f);
            light.shader.SetParameter("thisLightIntensity", light.intensity);

            aSprite.selfIlluminateShader = new Shader(null, "shaders/selfIlluminate.frag");
            this.objectDrawable = aSprite;
        }

        public override void Update(float deltaTime)
        {
            //this.collider.AddVelocity(this.velocity);
            this.position = this.collider.position + this.colliderOffset;
            base.Update(deltaTime);
            light.Update(new Vector2((this.collider.position.X + 5), (this.collider.position.Y + 5)), deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(aSprite);

            lightMap.Draw(this.light);

            aSprite.AddShader(aSprite.selfIlluminateShader);
            lightMap.Draw(aSprite);
            aSprite.RemoveShader();

            base.Draw(diffuseSurface, lightMap, deltaTime);
        }

        public void Dispose()
        {
            this.collider = null;
            this.texture = null;
            this.aSprite = null;
        }
    }
}
