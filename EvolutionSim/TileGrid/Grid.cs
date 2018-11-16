using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim.TileGrid
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
        private Tile[][] tiles; // This MUST stay private, if you are trying to manipulate it elsewhere then the code is coupled which probably means it should happen here
        public List<Organism> Organisms { get; private set; } = new List<Organism>();
        public List<Food> Foods { get; private set; } = new List<Food>();
        public static int HorizontalCount { get; private set; }
        public static int VerticalCount { get; private set; }

        private Random _random = new Random();
        
        public Grid(Texture2D tileTexture, Texture2D mountainTexture, Texture2D waterTexture, int width, int height)
        {
            HorizontalCount = width / Tile.TILE_SIZE;
            VerticalCount = height / Tile.TILE_SIZE;

            this.tiles = new Tile[HorizontalCount][];

            for (var i = 0; i < HorizontalCount; i++)
            {
                this.tiles[i] = new Tile[VerticalCount];
                for (var j = 0; j < VerticalCount; j++)
                {
                    this.tiles[i][j] = new Tile(tileTexture, mountainTexture, waterTexture, new Rectangle(i * Tile.TILE_SIZE, j * Tile.TILE_SIZE, Tile.TILE_SIZE, Tile.TILE_SIZE));
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Organism organism in Organisms)
            {
                organism.Draw(spriteBatch);
            }

            foreach (Food food in Foods)
            {
                food.Draw(spriteBatch);
            }
            
            for (var i = 0; i < HorizontalCount; i++)
            {
                for (var j = 0; j < VerticalCount; j++)
                {
                    this.tiles[i][j].Draw(spriteBatch);
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

        public bool AttemptToPositionAt(GridItem item, int x, int y)
        {
            if (this.tiles[x][y].HasInhabitant())
            {
                return false; // Space occupied
            }

            this.tiles[x][y].AddMapItem(item);

            if (item.GetType() == typeof(Organism))
            {
                var organism = (Organism)item;
                organism.DeathOccurred += OrganismDeathHandler;
                Organisms.Add(organism);
            }
            else if (item.GetType() == typeof(Food))
            {
                var food = (Food)item;
                food.DeathOccurred += FoodDeathHandler;
                Foods.Add(food);
            }

            return true; // Successfully positioned
        }

        public void SetTerrainAt(TerrainTypes type, int x, int y)
        {
            this.tiles[x][y].SetTerrain(type);
        }

        public void MoveOrganism(Organism organism, int destinationX, int destinationY)
        {
            var parentTile = this.tiles[organism.GridPosition.X][organism.GridPosition.Y];
            var destinationTile = this.tiles[destinationX][destinationY];
            if (!destinationTile.HasInhabitant())
            {
                parentTile.MoveInhabitant(destinationTile);
            }
        }

        public void MoveOrganism(Organism organism, Tile destination)
        {
            var parentTile = this.tiles[organism.GridPosition.X][organism.GridPosition.Y];
            if (!destination.HasInhabitant())
            {
                parentTile.MoveInhabitant(destination);
            }
        }

        public bool IsFoodAt(int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            return inhabitant != null && inhabitant.GetType() == typeof(Food);
        }

        /// <summary>
        /// This method is called by an organism who is searching for another one to mate with
        /// </summary>
        /// <param name="organism"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsMateAt(Organism organism, int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            if (inhabitant == organism)
            {
                return false;
            }
            return inhabitant != null && inhabitant.GetType() == typeof(Organism) && ((Organism)inhabitant).OrganismState == PotentialStates.SeekMate;
        }

        public Tile GetTileAt(int x, int y)
        {
            return this.tiles[x][y];
        }

        private void OrganismDeathHandler(object sender, EventArgs e)
        {
            this.Organisms.Remove((Organism)sender);
        }

        private void FoodDeathHandler(object sender, EventArgs e)
        {
            this.Foods.Remove((Food)sender);
        }

        public static Boolean InBounds(int x, int y)
        {
            if (y >= Grid.VerticalCount || y < 0 || x >= Grid.HorizontalCount || x < 0)
            {
                return false;
            }
            return true;
        }
    }
}
