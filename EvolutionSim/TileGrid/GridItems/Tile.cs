﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    enum TerrainTypes
    {
        Grass
    }

    public class Tile : GridItem
    {
        public const int TILE_SIZE = 8;

        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public GridItem Inhabitant { get; private set; }

        private TerrainTypes terrain = TerrainTypes.Grass; // For the time being, everything is standard grass

        public Tile(Texture2D texture, Rectangle rectangle) : base(texture, rectangle)
        {
            GridPositionX = rectangle.X / TILE_SIZE;
            GridPositionY = rectangle.Y / TILE_SIZE;
            base.GridPosition.X = GridPositionX;
            base.GridPosition.Y = GridPositionY;
        }

        public void AddMapItem(GridItem gridItem)
        {
            Inhabitant = gridItem;
            gridItem.MoveToTile(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

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