using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.Sprites
{
    public class Healthbar : Sprite
    {
        private const int WIDTH = 32;
        private const int HEIGHT = 4;
        private const int OFFSET_Y = 6;

        public int CurrentHealth { set => this.innerRectangle.Width = (int)((WIDTH * ((float)value / this.maxHealth)) + 0.5f); }
        private bool isAtFullHealth { get => this.innerRectangle.Width == this.rectangle.Width; }

        private int maxHealth;

        // In this instance, the .inner variables refer to the green actual health which fills up/empties depending on hp
        // whereas the standard .rectangle and .texture refer to the outer red healthbar which is always the same size
        private Texture2D innerTexture;
        private Rectangle innerRectangle;

        public Healthbar(Tuple<Texture2D, Texture2D> textures, Rectangle parentRectangle, int maxHealth) : base(textures.Item1, new Rectangle(0, 0, WIDTH, HEIGHT))
        {
            this.maxHealth = maxHealth;
            this.innerTexture = textures.Item2;
            this.innerRectangle = new Rectangle(0, 0, WIDTH, HEIGHT);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isAtFullHealth)
            {
                return; // Don't draw healthbars at full health
            }

            base.Draw(spriteBatch);
            spriteBatch.Draw(this.innerTexture, this.innerRectangle, Color.White);
        }

        public override void SetScreenPosition(int x, int y)
        {
            var yPos = y - OFFSET_Y;

            base.SetScreenPosition(x, yPos);
            this.innerRectangle.X = x;
            this.innerRectangle.Y = yPos;
        }
    }
}
