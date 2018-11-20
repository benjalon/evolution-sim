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
        protected Rectangle rectangle;
        public Rectangle Rectangle { get => this.rectangle; } // Alias for the rectangle because structs and properties don't play nice

        public Point TileIndex; // The index of this item on the grid, this is not the object's actual screen position
        
        protected int _health { get; private set; } = 80;
        public event EventHandler DeathOccurred;

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the GridItem</param>
        public GridItem(Texture2D texture)
        {
            this.texture = texture;
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
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, Rectangle, Color.White);
        }

        /// <summary>
        /// Set the position of the item to a new place on the screen. Note that this is the actual screen position and not the grid index.
        /// </summary>
        /// <param name="x">The x position to move to.</param>
        /// <param name="y">The y position to move to.</param>
        public void SetPosition(int x, int y)
        {
            this.rectangle.X = x;
            this.rectangle.Y = y;
        }

        /// <summary>
        /// Reparents the sprite to the given tile (i.e. makes it an inhabitant of the tile).
        /// </summary>
        /// <param name="tile">The tile to move to</param>
        public void SetTileIndex(Tile tile)
        {
            this.TileIndex.X = tile.TileIndex.X;
            this.TileIndex.Y = tile.TileIndex.Y;
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
                DeathOccurred?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
