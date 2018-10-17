using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    /// <summary>
    /// A combination of a texture and rectangle form a sprite, where the texture is drawn on top of the rectangle.
    /// This class is purely for static sprites such as banners, background textures or sprites not requiring any
    /// kind of updates.
    /// </summary>
    public abstract class Sprite
    {
        protected Texture2D _texture;
        protected Rectangle _rectangle;
        public Color Color { get; set; }

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the Sprite</param>
        /// <param name="rectangle">The position and size of the Sprite</param>
        public Sprite(ref Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
            Color = Color.White;
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color);
        }
    }
}
