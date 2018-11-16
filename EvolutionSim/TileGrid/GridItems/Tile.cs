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

    public class Tile : GridItem
    {
        public const int TILE_SIZE = 16;

        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public GridItem Inhabitant { get; private set; }

        private TerrainTypes terrain = TerrainTypes.Grass; // For the time being, everything is standard grass        
        private Texture2D mountainTexture;
        private Texture2D waterTexture;
        public int MovementDifficulty = 1;
        private int difficultyModifier = 3;

        public Tile(Texture2D tileTexture, Texture2D mountainTexture, Texture2D waterTexture, Rectangle rectangle) : base(tileTexture, rectangle)
        {
            GridPositionX = rectangle.X / TILE_SIZE;
            GridPositionY = rectangle.Y / TILE_SIZE;
            base.GridPosition.X = GridPositionX;
            base.GridPosition.Y = GridPositionY;
            this.mountainTexture = mountainTexture;
            this.waterTexture = waterTexture;
        }

        public void AddMapItem(GridItem gridItem)
        {
            Inhabitant = gridItem;
            gridItem.MoveToTile(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            switch (this.terrain)
            {
                case TerrainTypes.Mountain:
                    spriteBatch.Draw(this.mountainTexture, Rectangle, Color);
                    break;
                case TerrainTypes.Water:
                    spriteBatch.Draw(this.waterTexture, Rectangle, Color);
                    break;
                default:
                    break;
            }

            if (HasInhabitant())
            {
                Inhabitant.Draw(spriteBatch);
            }
        }

        public void MoveInhabitant(Tile endPosition)
        {
            if (HasInhabitant())
            {
                Inhabitant.MoveToTile(endPosition);
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
