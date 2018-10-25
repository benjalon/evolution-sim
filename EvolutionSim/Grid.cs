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
        private Tile[,] _tiles;
        public List<Sprite> Organisms { get; private set; } = new List<Sprite>(); // List of all organisms currently in the grid, which can be used to find their parent tile

        private int horizontalCount;
        private int verticalCount;

        private Random _random = new Random();
        
        private const int MS_PER_DIRECTION_CHANGE = 0; // The time in milliseconds per movement update
        private int _msSinceDirectionChange = MS_PER_DIRECTION_CHANGE;
        
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
        public void Move(GameTime gameTime)
        {
            _msSinceDirectionChange += gameTime.ElapsedGameTime.Milliseconds;

            var shouldChangeDirection = _msSinceDirectionChange > MS_PER_DIRECTION_CHANGE;
            if (shouldChangeDirection)
            {
                _msSinceDirectionChange = 0;
                
                for (var i = 0; i < horizontalCount; i++)
                {
                    for (var j = 0; j < verticalCount; j++)
                    {
                        if (_tiles[i, j].HasInhabitant())
                        {
                            Roam(_tiles[i, j]);
                        }
                    }
                }
            }
        }


        //Takes tile.inhabitant, moves them randomly. 
        public void Roam(Tile state)
        {
            //decide destination
            var num = (Directions)_random.Next(0, 4);
            var destinationTileX = state.GridPositionX;
            var destinationTileY = state.GridPositionY;

            switch (num)
            {
                case Directions.Up:
                    if (destinationTileY > 0)
                    {
                        destinationTileY -= 1;
                    }
                    break;
                case Directions.Left:
                    if (destinationTileX > 0)
                    {
                        destinationTileX -= 1;
                    }
                    break;
                case Directions.Down:
                    if (destinationTileY < _tiles.GetLength(1)-1)
                    {
                        destinationTileY += 1;
                    }
                    break;
                case Directions.Right:
                    if (destinationTileX < _tiles.GetLength(0)-1)
                    {
                        destinationTileX += 1;
                    }
                    break;
            }

            var destinationTile = _tiles[destinationTileX, destinationTileY];
            if (!destinationTile.HasInhabitant())
            {
                state.MoveInhabitant(destinationTile);
            }

            //if destination full decide again.
        }

        //private void MoveInhabitant(int x, int y, int endX, int endY)
        //{
        //    var endPosition = _tiles[endX, endY].Rectangle;

        //    _tiles[x, y].MoveInhabitant(endPosition);

        //}
    }
}
