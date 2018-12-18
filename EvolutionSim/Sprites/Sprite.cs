using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Sprites
{
    /// <summary>
    /// A combination of a texture and rectangle form a sprite, where the texture is drawn on top of the rectangle.
    /// This class is purely for static sprites such as banners, background textures or sprites not requiring any
    /// kind of updates.
    /// </summary>
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        protected Rectangle rectangle;
        public Rectangle Rectangle { get => this.rectangle; } // Alias for the rectangle because structs and properties don't play nice

        /// <summary>
        /// Create a static sprite from a given texture.
        /// </summary>
        /// <param name="texture">The appearance of the Sprite.</param>
        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
        }

        /// <summary>
        /// Create a static sprite from a given rectangle.
        /// </summary>
        /// <param name="rectangle">The position of the Sprite.</param
        public Sprite(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }


        /// <summary>
        /// Create a static sprite from a given texture and rectangle.
        /// </summary>
        /// <param name="texture">The appearance of the Sprite.</param>
        /// <param name="rectangle">The position of the Sprite.</param>
        public Sprite(Texture2D texture, Rectangle rectangle)
        {
            this.Texture = texture;
            this.rectangle = rectangle;
        }
        
        /// <summary>
        /// Set the position of the item to a new place on the screen. Note that this is the actual screen position and not the grid index.
        /// </summary>
        /// <param name="x">The x position to move to.</param>
        /// <param name="y">The y position to move to.</param>
        public virtual void SetScreenPosition(int x, int y)
        {
            if (this.rectangle == null)
            {
                return;
            }

            this.rectangle.X = x;
            this.rectangle.Y = y;
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.rectangle == null || this.Texture == null)
            {
                return;
            }

            spriteBatch.Draw(this.Texture, Rectangle, Color.White);
        }
    }
}
