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

        public Point GridIndex; // The index of this tile on the grid, this is not the object's actual screen position
        public int ScreenPositionX { get => GridIndex.X * TILE_SIZE; } // The actual screen position of the tile
        public int ScreenPositionY { get => GridIndex.Y * TILE_SIZE; } // The actual screen position of the tile

        public GridItem Inhabitant { get; private set; }
        private TerrainTypes terrain = TerrainTypes.Grass; // For the time being, everything is standard grass        

        public int MovementDifficulty = 1;
        private int difficultyModifier = 3;

        public Tile(Texture2D tileTexture, Texture2D mountainTexture, Texture2D waterTexture, Point gridIndex)
        {
            this.GridIndex = gridIndex;
            this.mountainTexture = mountainTexture;
            this.waterTexture = waterTexture;
        }

        public void SetInhabitant(GridItem gridItem)
        {
            Inhabitant = gridItem;

            gridItem.SetGridIndex(this);
        }

        public void MoveInhabitant(Tile endPosition)
        {
            if (HasInhabitant())
            {
                Inhabitant.SetGridIndex(endPosition);
                endPosition.Inhabitant = Inhabitant;
                Inhabitant = null;
            }
        }

        public void SetTerrain(TerrainTypes terrainType)
        {
            this.terrain = terrainType;
            this.MovementDifficulty = (int)terrainType * this.difficultyModifier;
        }

        public void RemoveInhabitant()
        {
            Inhabitant = null;
        }

        public bool HasInhabitant()
        {
            return Inhabitant != null;
        }
    }
}
