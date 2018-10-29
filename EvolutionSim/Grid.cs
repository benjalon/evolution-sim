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
        public List<Organism> Organisms { get; private set; } = new List<Organism>(); // List of all organisms currently in the grid, which can be used to find their parent tile
        public List<Food> Foods { get; private set; } = new List<Food>();

        private int horizontalCount;
        private int verticalCount;

        private Random _random = new Random();
        
        private const int MS_PER_DIRECTION_CHANGE = 2500; // The time in milliseconds per movement update
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
        /// 
        public void AddOrganism(Organism organism)
        {
            
        Organisms.Add(organism); // Keep track of newly added organisms so we can get them later
            var x = _random.Next(0, horizontalCount);
            var y = _random.Next(0, verticalCount);
            _tiles[x, y].AddInhabitant(organism);
        }
        public void AddFood(Food food)
        {

            Foods.Add(food); // Keep track of newly added organisms so we can get them later

            var x = _random.Next(0, horizontalCount);
            var y = _random.Next(0, verticalCount);
            _tiles[x, y].AddInhabitant(food);
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

        private Boolean InBounds(int x, int y)
        {
            if (y >= verticalCount || y < 0 || x >= horizontalCount || x < 0){
                return false;
            }
            return true;
        }
        private Boolean FoodInRange(Tile tile)
        {
            // Check +X +Y -X -Y
            for (int i = 1; i < 3; i++)
            {


                if (InBounds(tile.GridPositionX +i,tile.GridPositionY) && _tiles[tile.GridPositionX + i, tile.GridPositionY].Inhabitant is Food) { 
                        
                        System.Diagnostics.Debug.WriteLine("FOOD DETECTED");
                     
                    }

                if (InBounds(tile.GridPositionX - i, tile.GridPositionY) && _tiles[tile.GridPositionX - i, tile.GridPositionY].Inhabitant is Food)
                {

                    System.Diagnostics.Debug.WriteLine("FOOD DETECTED");

                }
                if (InBounds(tile.GridPositionX, tile.GridPositionY + i) && _tiles[tile.GridPositionX, tile.GridPositionY + i].Inhabitant is Food)
                {

                    System.Diagnostics.Debug.WriteLine("FOOD DETECTED");

                }
                if (InBounds(tile.GridPositionX, tile.GridPositionY - i) && _tiles[tile.GridPositionX, tile.GridPositionY - i].Inhabitant is Food)
                {

                    System.Diagnostics.Debug.WriteLine("FOOD DETECTED");

                }



            }



            return false;
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
                            if (_tiles[i, j].Inhabitant is Organism)
                            {
                                FoodInRange(_tiles[i, j]);
                                Roam(_tiles[i, j]);
                            }
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
