using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Utility
{
    public abstract class Particle
    {
        private readonly Texture2D texture;
        private Vector2 position;
        protected Vector2 velocity;
        protected float rotation;
        protected float rotationVelocity;
        protected Color color;
        protected float scale;

        public Particle(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Update()
        {
            this.position += this.velocity;
            this.rotation += this.rotationVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, this.color * 0.5f, this.rotation, new Vector2(this.texture.Width * 0.5f, this.texture.Height * 0.5f), this.scale, SpriteEffects.None, 0.0f);
        }
    }

    public class SpawnParticle : Particle
    {
        public SpawnParticle(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.velocity = new Vector2(1.0f * (float)(Graphics.RANDOM.NextDouble() * 2 - 1), 1.0f * (float)(Graphics.RANDOM.NextDouble() * 2 - 1));
            this.rotation = 0.0f;
            this.rotationVelocity = 0.1f * (float)(Graphics.RANDOM.NextDouble() * 2 - 1);
            this.color = new Color((float)Graphics.RANDOM.NextDouble(), (float)Graphics.RANDOM.NextDouble(), (float)Graphics.RANDOM.NextDouble());
            this.scale = (float)Graphics.RANDOM.NextDouble() * 0.5f;
        }
    }
}
