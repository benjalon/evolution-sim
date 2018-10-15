using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.Source
{
    /// <summary>
    /// A combination of a texture and rectangle form a sprite, where the texture is drawn on top of the rectangle.
    /// </summary>
    class Sprite
    {
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Color _color;

        private Random _random = new Random(); // Temporary

        /// <summary>
        /// Create a sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the Sprite</param>
        /// <param name="rectangle">The position and size of the Sprite</param>
        public Sprite(ref Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
            _color = Color.White;
        }

        /// <summary>
        /// Create a sprite from a given texture, rectangle and color
        /// </summary>
        /// <param name="texture">The appearance of the Sprite</param>
        /// <param name="rectangle">The position and size of the Sprite</param>
        /// <param name="color">The color of the sprite (White = default)</param>
        public Sprite(ref Texture2D texture, Rectangle rectangle, Color color)
        {
            _texture = texture;
            _rectangle = rectangle;
            _color = color;
        }

        /// <summary>
        /// Update the Sprite by moving its rectangle
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Temporary
            _rectangle.X += _random.Next(-7, 8);
            _rectangle.Y += _random.Next(-7, 8);
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, _color);
        }
    }
}
