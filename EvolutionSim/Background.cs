using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Background
    {
        private Texture2D texture;
        private Rectangle rectangle;

        public Background(Texture2D texture, int screenWidth, int screenHeight)
        {
            this.texture = texture;
            this.rectangle = new Rectangle(0, 0, screenWidth, screenHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
