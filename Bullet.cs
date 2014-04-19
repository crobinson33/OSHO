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
        public Spotlight light;
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
			collider.AddTagToIgnore("characterMelee");
            collider.clearVelocityAmount = 1;
            world.AddCollider(collider);

            this.velocity = vel;

            this.collider.debug = true;

            this.light = new Spotlight(20, new Color(1f, 1f, 1f), this.collider.position, 100, 0.05f, true);
            light.shader.SetParameter("thisLightIntensity", light.intensity);

            aSprite.selfIlluminateShader = new Shader(null, "shaders/selfIlluminate.frag");
        }

        public override void Update(float deltaTime)
        {
            //this.collider.AddVelocity(this.velocity);
            this.position = this.collider.position;
            base.Update(deltaTime);
            light.Update(new Vector2((this.collider.position.X + (aSprite.width / 2)), (this.collider.position.Y + (aSprite.height / 2))), deltaTime);
        }

        public override void Draw(Surface diffuseSurface, Surface lightMap, float deltaTime)
        {
            diffuseSurface.Draw(aSprite, this.position, deltaTime);

            lightMap.Draw(this.light, light.position, deltaTime);

            aSprite.AddShader(aSprite.selfIlluminateShader);
            lightMap.Draw(aSprite, this.position, deltaTime);
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
