using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.Source
{
    class Sprite
    {
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Color _color;

        private Random random = new Random();

        public Sprite(ref Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
            _color = Color.White;
        }

        public Sprite(ref Texture2D texture, Rectangle rectangle, Color color)
        {
            _texture = texture;
            _rectangle = rectangle;
            _color = color;
        }

        public void Update(GameTime gameTime)
        {
            _rectangle.X += random.Next(-7, 8);
            _rectangle.Y += random.Next(-7, 8);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, _color);
        }
    }
}
