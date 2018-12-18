using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim.Utility
{
    class ParticleEffect
    {
        private readonly List<Particle> particles = new List<Particle>();

        private int msSinceSpawn = 0;
        private readonly int msLifeSpan;

        public bool Complete { get => this.msSinceSpawn >= this.msLifeSpan; }

        public ParticleEffect(List<Texture2D> textures, Type particleType, int particleCount, int msLifeSpan, Vector2 position)
        {
            this.msLifeSpan = msLifeSpan;

            for (var i = 0; i < particleCount; i++)
            {
                this.particles.Add((Particle)Activator.CreateInstance(particleType, new object[] { textures[Graphics.RANDOM.Next(textures.Count)], position }));
            }
        }

        public void Update(GameTime gameTime)
        {
            this.msSinceSpawn += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var particle in this.particles)
            {
                particle.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in this.particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
