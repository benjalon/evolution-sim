﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    enum TerrainTypes
    {
        Grass
    }

    public class Tile : Sprite
    {
        public const int TILE_SIZE = 16;

        private Rectangle _rectangle;

        private Sprite _inhabitant;
        private TerrainTypes _terrain = TerrainTypes.Grass; // For the time being, everything is standard grass

        public Tile(ref Texture2D texture, Rectangle rectangle): base(ref texture, rectangle)
        {

        }

        public void AddInhabitant(Sprite sprite)
        {
            _inhabitant = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //_inhabitant.Draw(spriteBatch);
        }
    }
}
