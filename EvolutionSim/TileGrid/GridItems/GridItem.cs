using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// A combination of a texture and rectangle form a sprite, where the texture is drawn on top of the rectangle.
    /// This class is purely for static sprites such as banners, background textures or sprites not requiring any
    /// kind of updates.
    /// </summary>
    public abstract class GridItem
    {
        protected Texture2D texture;
        public Color Color { get; private set; }

        public Point GridPosition;

        protected Rectangle rectangle;
        public Rectangle Rectangle
        {
            get => this.rectangle;
        }

        protected int _health { get; private set; } = 80;
        public event EventHandler DeathOccurred;

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the GridItem</param>
        public GridItem(Texture2D texture)
        {
            this.texture = texture;
            Color = Color.White;
        }

        public void UpdateRectangle(float x, float y)
        {
            this.rectangle.X = (int)x;
            this.rectangle.Y = (int)y;
        }

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the GridItem</param>
        /// <param name="rectangle">The position and size of the GridItem</param>
        public GridItem(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            Color = Color.White;
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, Rectangle, Color);
        }

        /// <summary>
        /// Reparents the sprite to the given tile (i.e. makes it an inhabitant of the tile).
        /// </summary>
        /// <param name="tile">The tile to move to</param>
        public void MoveToTile(Tile tile)
        {
            this.GridPosition.X = tile.GridPositionX;
            this.GridPosition.Y = tile.GridPositionY;
            this.rectangle = tile.Rectangle;
        }

        /// <summary>
        /// Lower health by the given amount, handling death if it occurs
        /// </summary>
        /// <param name="value">The value to lower by</param>
        public void LowerHealth(int value)
        {
            _health -= value;
            if (_health <= 0)
            {
                OnDeath(EventArgs.Empty); // Remove from brain collection
            }
        }

        public virtual void OnDeath(EventArgs e)
        {
            // BeginInvoke() ???
            DeathOccurred?.Invoke(this, e);
        }
    }
}
