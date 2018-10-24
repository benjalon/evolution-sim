using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim
{
    public class Grid
    {
        private Tile[,] _tiles;

        private Random _random = new Random();

        public Grid(ref Texture2D tileTexture, int width, int height)
        {
            var horizontalCount = width / Tile.TILE_SIZE;
            var verticalCount = height / Tile.TILE_SIZE;

            _tiles = new Tile[horizontalCount, verticalCount];

            for (var i = 0; i < horizontalCount; i++)
            {
                for (var j = 0; j < verticalCount; j++)
                {
                    _tiles[i, j] = new Tile(ref tileTexture, new Rectangle(i * Tile.TILE_SIZE, j * Tile.TILE_SIZE, Tile.TILE_SIZE, Tile.TILE_SIZE));
                }
            }
        }

        public void AddInhabitant(Sprite sprite)
        {
            var x = _random.Next(0, 3);
            var y = _random.Next(0, 3);

            _tiles[x, y].AddInhabitant(sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
