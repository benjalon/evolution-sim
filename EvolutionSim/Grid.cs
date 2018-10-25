using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Grid
    {
        private Tile[,] _tiles;
        public List<Sprite> Organisms { get; private set; } = new List<Sprite>(); // List of all organisms currently in the grid, which can be used to find their parent tile

        private int horizontalCount;
        private int verticalCount;

        private Random _random = new Random();
        
        public Grid(ref Texture2D tileTexture, int width, int height)
        {
            horizontalCount = width / Tile.TILE_SIZE;
            verticalCount = height / Tile.TILE_SIZE;

            _tiles = new Tile[horizontalCount, verticalCount];

            for (var i = 0; i < horizontalCount; i++)
            {
                for (var j = 0; j < verticalCount; j++)
                {
                    _tiles[i, j] = new Tile(ref tileTexture, new Rectangle(i * Tile.TILE_SIZE, j * Tile.TILE_SIZE, Tile.TILE_SIZE, Tile.TILE_SIZE));
                }
            }
        }

        /// <summary>
        /// Add an inhabitant at a random place in the grid
        /// </summary>
        /// <param name="sprite">the name of the sprite</param>
        public void AddInhabitant(Sprite sprite)
        {
            Organisms.Add(sprite); // Keep track of newly added organisms so we can get them later

            var x = _random.Next(0, horizontalCount);
            var y = _random.Next(0, verticalCount);
            _tiles[x, y].AddInhabitant(sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile.Draw(spriteBatch);
            }
        }


        /// <summary>
        /// Find and move toward the nearest food source given an organism.
        /// </summary>
        /// <param name="_passedOrganism"></param>
        public void TrackFood(Organism _passedOrganism)
        {
            var startTile = _passedOrganism.ParentTile;

            // TODO: Whoever does the AI path traversal
            // var destinationTile = _tiles[i][j];
        }

        /// <summary>
        /// Find and move toward the nearest potential mate given an organism.
        /// </summary>
        /// <param name="_passedOrganism"></param>
        public void TrackMate(Organism _passedOrganism)
        {
            var startTile = _passedOrganism.ParentTile;

            // TODO: Whoever does the AI path traversal
            // var destinationTile = _tiles[i][j];
        }
    }
}
