using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.TileGrid.GridItems
{
    /// <summary>
    /// A Sprite object which needs to be constrained to the Grid.
    /// </summary>
    public abstract class GridItem : Sprite
    {
        public Point GridIndex; // The index of this item on the grid, this is not the object's actual screen position

        protected int defaultHealth;
        protected int Health { get; private set; }
        public event EventHandler DeathOccurred;

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the GridItem</param>
        public GridItem(Texture2D texture, int health) : base(texture)
        {
            this.defaultHealth = health;
            Health = health;
        }

        /// <summary>
        /// Give the item a rectangle used for drawing.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width of the item.</param>
        /// <param name="height">The height of the item.</param>
        public void SetInitialScreenPosition(int x, int y, int width, int height)
        {
            this.rectangle = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Reparents the sprite to the given tile (i.e. makes it an inhabitant of the tile).
        /// </summary>
        /// <param name="tile">The tile to move to</param>
        public void SetGridIndex(Tile tile)
        {
            this.GridIndex.X = tile.GridIndex.X;
            this.GridIndex.Y = tile.GridIndex.Y;
        }

        /// <summary>
        /// Increase health by the given amount, up to the default maximum
        /// </summary>
        /// <param name="value">The value to lower by</param>
        public virtual void IncreaseHealth(int value)
        {
            Health += value;
            if (Health >= defaultHealth)
            {
                Health = defaultHealth;
            }
        }

        /// <summary>
        /// Lower health by the given amount, handling death if it occurs
        /// </summary>
        /// <param name="value">The value to lower by</param>
        public virtual void DecreaseHealth(int value)
        {
            Health -= value;
            if (Health <= 0)
            {
                DeathOccurred?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
