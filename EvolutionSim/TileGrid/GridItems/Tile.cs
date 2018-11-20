using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    public enum TerrainTypes
    {
        Grass,
        Mountain,
        Water
    }

    public class Tile
    {
        public const int TILE_SIZE = 32;
        
        private Texture2D mountainTexture;
        private Texture2D waterTexture;

        // TODO: Needs reimplementing under the new tile system
        private TerrainTypes terrain = TerrainTypes.Grass; 
        public int MovementDifficulty = 1;
        private int difficultyModifier = 3;

        public Point GridIndex; // The index of this tile on the grid, this is not the object's actual screen position
        public int ScreenPositionX { get => GridIndex.X * TILE_SIZE; } // The actual screen position of the tile, this is not the grid index
        public int ScreenPositionY { get => GridIndex.Y * TILE_SIZE; } // The actual screen position of the tile, this is not the grid index

        public GridItem Inhabitant { get; private set; } // The inhabitant being managed by this tile, can be food or organisms

        /// <summary>
        /// Create a new tile object.
        /// </summary>
        /// <param name="tileTexture">Tile debug texture.</param>
        /// <param name="mountainTexture">Mountain terrain texture.</param>
        /// <param name="waterTexture">Water terrain texture.</param>
        /// <param name="gridIndex">The index in the grid to position this tile at.</param>
        public Tile(Texture2D tileTexture, Texture2D mountainTexture, Texture2D waterTexture, Point gridIndex)
        {
            this.GridIndex = gridIndex;
            this.mountainTexture = mountainTexture;
            this.waterTexture = waterTexture;
        }

        /// <summary>
        /// Set an inhabitant to this tile. This should only be used to place inhabitants in the grid for the first time, never to reposition them.
        /// </summary>
        /// <param name="gridItem">The item to place on the grid.</param>
        public void AddInhabitant(GridItem gridItem)
        {
            Inhabitant = gridItem;
            gridItem.SetGridIndex(this);
        }

        /// <summary>
        /// Remove the inhabitant from this tile by taking away the reference.
        /// </summary>
        public void RemoveInhabitant()
        {
            Inhabitant = null;
        }

        /// <summary>
        /// Repositions an inhabitant, cleaning up their existing reference.
        /// </summary>
        /// <param name="destination">The tile to move the item to.</param>
        public void MoveInhabitant(Tile destination)
        {
            if (HasInhabitant())
            {
                // Move and reference the item to the destination tile
                Inhabitant.SetGridIndex(destination);
                destination.Inhabitant = Inhabitant;

                Inhabitant = null; // This tile is no longer managing the item
            }
        }

        /// <summary>
        /// Set the terrain of this tile to the given type.
        /// </summary>
        /// <param name="terrainType">The type of terrain to set.</param>
        public void SetTerrain(TerrainTypes terrainType)
        {
            this.terrain = terrainType;
            this.MovementDifficulty = (int)terrainType * this.difficultyModifier;
        }

        /// <summary>
        /// Check if this tile is tracking an inhabitant.
        /// </summary>
        /// <returns>Whether this tile has an inhabitant or not.</returns>
        public bool HasInhabitant()
        {
            return Inhabitant != null;
        }
    }
}
