using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    enum Directions
    {
        Up,
        Left,
        Down,
        Right
    }

    public class Grid
    {
        public Tile[][] _tiles;

        public static int HorizontalCount { get; private set; }
        public static int VerticalCount { get; private set; }

        private Random _random = new Random();
        
        public Grid(Texture2D tileTexture, int width, int height)
        {
            HorizontalCount = width / Tile.TILE_SIZE;
            VerticalCount = height / Tile.TILE_SIZE;

            _tiles = new Tile[HorizontalCount][];

            for (var i = 0; i < HorizontalCount; i++)
            {
                _tiles[i] = new Tile[VerticalCount];
                for (var j = 0; j < VerticalCount; j++)
                {
                    _tiles[i][j] = new Tile(tileTexture, new Rectangle(i * Tile.TILE_SIZE, j * Tile.TILE_SIZE, Tile.TILE_SIZE, Tile.TILE_SIZE));
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < HorizontalCount; i++)
            {
                for (var j = 0; j < VerticalCount; j++)
                {
                    _tiles[i][j].Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Find and move toward the nearest food source given an organism.
        /// </summary>
        /// <param name="_passedOrganism"></param>
        public bool TrackFood(Organism _passedOrganism)
        {
            var startTile = _passedOrganism.ParentTile;
            bool found = false;

            // TODO: Whoever does the AI path traversal
            // var destinationTile = _tiles[i][j];

            return found;
        }

        /// <summary>
        /// Find and move toward the nearest potential mate given an organism.
        /// </summary>
        /// <param name="_passedOrganism"></param>
        public bool TrackMate(Organism _passedOrganism)
        {
           // var startTile = _passedOrganism.ParentTile;
            bool found = false;

            // TODO: Whoever does the AI path traversal
            // var destinationTile = _tiles[i][j];

            return found;
        }

        public bool TryToPosition(MapItem item, int positionX, int positionY)
        {
            if (_tiles[positionX][positionY].HasMapItem())
            {
                return false;
            }

            _tiles[positionX][positionY].AddMapItem(item);
            return true;
        }

    }
}
