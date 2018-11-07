using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim
{
    enum TerrainTypes
    {
        Grass
    }

    public class Tile : MapItem
    {
        public const int TILE_SIZE = 10;

        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public MapItem Inhabitant { get; private set; }

        private TerrainTypes _terrain = TerrainTypes.Grass; // For the time being, everything is standard grass

        public Tile(Texture2D texture, Rectangle rectangle) : base(texture, rectangle)
        {
            GridPositionX = rectangle.X / TILE_SIZE;
            GridPositionY = rectangle.Y / TILE_SIZE;
            base.GridPosition.X = GridPositionX;
            base.GridPosition.Y = GridPositionY;
        }

        public void AddMapItem(MapItem mapItem)
        {
            Inhabitant = mapItem;
            mapItem.MoveToTile(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (HasMapItem())
            {
                Inhabitant.Draw(spriteBatch);
            }
        }

        public void MoveInhabitant(Tile endPosition)
        {
            Inhabitant.MoveToTile(endPosition);
            endPosition.Inhabitant = Inhabitant;
            Inhabitant = null;
        }

        public void RemoveInhabitant()
        {
            Inhabitant = null;
        }

        public bool HasMapItem()
        {
            return Inhabitant != null;
        }
    }
}
