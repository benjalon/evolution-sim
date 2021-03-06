﻿using EvolutionSim.Data;
using EvolutionSim.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim.TileGrid
{
    /// <summary>
    /// The simulation grid. Made up of a number of a 2D array of Tile objects which may or may not house GridItems.
    /// </summary>
    public class Grid
    {
        public const int TILE_COUNT_X = Graphics.SIMULATION_WIDTH / Tile.TILE_SIZE;
        public const int TILE_COUNT_Y = Graphics.SIMULATION_WIDTH / Tile.TILE_SIZE;

        public List<Organism> Organisms { get; private set; } = new List<Organism>();
        public List<Food> Foods { get; private set; } = new List<Food>();
        public List<Terrain> Terrains { get; private set; } = new List<Terrain>();

        private readonly Tile[][] tiles; // This MUST stay private, if you are trying to manipulate it elsewhere then the code is coupled which probably means it should happen here

        public event EventHandler ShouldSpawnCorpse;

        /// <summary>
        /// Create a Grid with given attributes.
        /// </summary>
        /// <param name="highlightTexture">The texture drawn over a tile which is highlighted with the mouse.</param>
        /// <param name="mountainTexture">Terrain texture for mountain terrain.</param>
        /// <param name="waterTexture">Terrain texture for water terrain.</param>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public Grid(Texture2D highlightTexture, Texture2D mountainTexture, Texture2D waterTexture)
        {
            this.tiles = new Tile[TILE_COUNT_X][];

            // Create the jagged tile array by creating a new tile at each index
            for (var x = 0; x < TILE_COUNT_X; x++)
            {
                this.tiles[x] = new Tile[TILE_COUNT_Y];
                for (var y = 0; y < TILE_COUNT_Y; y++)
                {
                    this.tiles[x][y] = new Tile(mountainTexture, waterTexture, new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Draw the grid by iterating through each of the tiles, food and organisms and calling their draw.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var terrain in Terrains)
            {
                terrain.Draw(spriteBatch); // Draw all food objects in simulation
            }

            foreach (var food in Foods)
            {
                food.Draw(spriteBatch); // Draw all food objects in simulation
            }

            foreach (var organism in Organisms)
            {
                organism.Draw(spriteBatch); // Draw all organism objects in simulation
            }
        }

        

        /// <summary>
        /// Attempt to position the given item at the given index. If the space is occupied already it will fail.
        /// </summary>
        /// <param name="item">The item to be positioned.</param>
        /// <param name="x">The x index of the tile to position at.</param>
        /// <param name="y">The y index of the tile to position at.</param>
        /// <returns>True if successfully positioned, false if the space was occupied.</returns>
        public bool AttemptToPositionAt(GridItem item, int x, int y)
        {
            if (!InBounds(x, y) || this.tiles[x][y].HasInhabitant)
            {
                return false; // Space occupied
            }

            this.tiles[x][y].AddInhabitant(item);
            item.SetInitialScreenPosition(x * Tile.TILE_SIZE, y * Tile.TILE_SIZE, Tile.TILE_SIZE, Tile.TILE_SIZE);

            // Add the item to the simulation and set up appropriate event handlers
            if (item.GetType() == typeof(Organism))
            {
                var organism = (Organism)item;
                organism.DeathOccurred += OrganismDeathHandler;
                Organisms.Add(organism);
            }
            else if (item.GetType() == typeof(Food))
            {
                var food = (Food)item;
                food.DeathOccurred += FoodEatenHandler;
                Foods.Add(food);
            }



            return true; // Successfully positioned
        }

        /// <summary>
        /// Set the terrain at the given index to the type specified.
        /// </summary>
        /// <param name="type">The type of terrain to position.</param>
        /// <param name="x">The x index of the tile to position at.</param>
        /// <param name="y">The y index of the tile to position at.</param>
        public void SetTerrainAt(TerrainTypes terrainType, int x, int y)
        {
            if (!InBounds(x, y))
            {
                return; // Out of bounds
            }

            var tile = this.tiles[x][y];

            switch (terrainType)
            {
                case TerrainTypes.Grass:
                    if (this.Terrains.Contains(tile.Terrain))
                    {
                        this.Terrains.Remove(tile.Terrain);
                    }
                    break;
                case TerrainTypes.Mountain:
                case TerrainTypes.Water:
                    if (!this.Terrains.Contains(tile.Terrain))
                    {
                        this.Terrains.Add(tile.Terrain);
                    }
                    break;
                default:
                    return;
            }

            tile.SetTerrain(terrainType);
        }

        /// <summary>
        /// Move the given organism to the specified position.
        /// </summary>
        /// <param name="organism">The organism to move.</param>
        /// <param name="destinationX">The x index of the tile to move to.</param>
        /// <param name="destinationY">The y index of the tile to move to.</param>
        public void ReparentOrganism(Organism organism, int destinationX, int destinationY)
        {
            var parentTile = this.tiles[organism.GridIndex.X][organism.GridIndex.Y];
            var destinationTile = this.tiles[destinationX][destinationY];
            if (!destinationTile.HasInhabitant)
            {
                parentTile.MoveInhabitant(destinationTile);
            }
        }

        /// <summary>
        /// Get the tile instance at the given index.
        /// </summary>
        /// <param name="x">The x index of the tile.</param>
        /// <param name="y">The y index of the tile.</param>
        /// <returns>The tile in question.</returns>
        public Tile GetTileAt(int x, int y)
        {
            return this.tiles[x][y];
        }

        /// <summary>
        /// Get the tile instance at the position of the given item.
        /// </summary>
        /// <param name="item">The item who's tile is required.</param>
        /// <returns>The tile in question.</returns>
        public Tile GetTileAt(GridItem item)
        {
            return this.tiles[item.GridIndex.X][item.GridIndex.Y];
        }

        /// <summary>
        /// Checks whether there is food at the given index.
        /// </summary>
        /// <param name="x">The x index of the tile to check.</param>
        /// <param name="y">The y index of the tile to check.</param>
        /// <returns>True if there is food, false if there is not.</returns>
        public bool IsFoodAt(int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            return inhabitant != null && inhabitant.GetType() == typeof(Food);
        }

        /// <summary>
        /// Checks whether there is a mate at the given index.
        /// </summary>
        /// <param name="organism">The organism performing the check</param>
        /// <param name="x">The x index of the tile to check.</param>
        /// <param name="y">The y index of the tile to check.</param>
        /// <returns>True if there is a mate, false if there is not.</returns>
        public bool IsMateAt(Organism organism, int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            if (inhabitant == organism)
            {
                return false; // The organism is checking itself, so it isn't a mate.
            }
            return inhabitant != null && inhabitant.GetType() == typeof(Organism) && ((Organism)inhabitant).State == States.SeekMate;
        }

        public bool IsOrganismAt(Organism organism, int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            if (inhabitant == organism)
            {
                return false; // The organism is checking itself, so it isn't a mate.
            }
            return inhabitant != null && inhabitant.GetType() == typeof(Organism);
        }

        public bool IsAdjacent(Point StartPosition, Point EndPosition)
        {
            var distance = Math.Floor(Math.Sqrt((StartPosition.X - EndPosition.X) * (StartPosition.X - EndPosition.X) + (StartPosition.Y - EndPosition.Y) * (StartPosition.Y - EndPosition.Y)));
            return distance == 1;
        }

        /// <summary>
        /// Checks whether the given tile exists in the tiles array.
        /// </summary>
        /// <param name="x">The x index to check.</param>
        /// <param name="y">The y index to check.</param>
        /// <returns>True if it is, false if it is not.</returns>
        public bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < TILE_COUNT_X && y < TILE_COUNT_Y;
        }

        /// <summary>
        /// Handle death by removing the organism from the grid and removing its reference from the list of organisms.
        /// </summary>
        /// <param name="sender">The organism in question.</param>
        /// <param name="e">Event arguments.</param>
        private void OrganismDeathHandler(object sender, EventArgs e)
        {
            var organism = (Organism)sender;
            if(organism.State == States.MovingToMate)
            {
                ((Organism)organism.DestinationTile.Inhabitant).WaitingForMate = false;
                ((Organism)organism.DestinationTile.Inhabitant).State = States.SeekMate;

            }
            this.GetTileAt(organism).RemoveInhabitant();
            this.Organisms.Remove(organism);
            GridItem.TOTAL_GRID_ITEMS--;
            Organism.TOTAL_POPULATION--;

            ShouldSpawnCorpse?.Invoke(organism, EventArgs.Empty);
    }

        /// <summary>
        /// Handle food being eaten by removing the food from the grid and removing its reference from the list of food.
        /// </summary>
        /// <param name="sender">The food in question.</param>
        /// <param name="e">Event arguments.</param>
        private void FoodEatenHandler(object sender, EventArgs e)
        {
            var food = (Food)sender;
            this.GetTileAt(food).RemoveInhabitant();
            this.Foods.Remove(food);
            GridItem.TOTAL_GRID_ITEMS--;

        }

        /// <summary>
        /// Called to kill food present on the
        /// grid over time
        /// </summary>
        public void UpdateFood()
        {

            var numFood = this.Foods.Count;

            //this loop gradually reduces the food 
            for (var i = numFood - 1; i >= 0; i--)
            {

                this.Foods[i].WitherFood();
            }

        }

        /// <summary>
        /// This method is called from the roam method
        /// </summary>
        /// <param name="organism"></param>
        /// <returns></returns>
        public Tile FindRandomNearbyEmptyTile(Organism organism)
        {
            const int MINIMUM_BOUND = 0;
            var organismTile = GetTileAt(organism);
            var minX = organismTile.GridIndex.X - Organism.DETECTION_RADIUS;
            var minY = organismTile.GridIndex.Y - Organism.DETECTION_RADIUS;
            var maxX = organismTile.GridIndex.X + Organism.DETECTION_RADIUS + 1; // +1 because random is exclusive on upper limit
            var maxY = organismTile.GridIndex.Y + Organism.DETECTION_RADIUS + 1;

            if (minX < MINIMUM_BOUND)
            {
                minX = MINIMUM_BOUND;
            }
            else if (maxX > TILE_COUNT_X)
            { 
                maxX = TILE_COUNT_X;
            }

            if (minY < MINIMUM_BOUND)
            {
                minY = MINIMUM_BOUND;
            }
            else if (maxY > TILE_COUNT_X)
            {
                maxY = TILE_COUNT_Y;
            }

            var tile = this.tiles[Graphics.RANDOM.Next(minX, maxX)][Graphics.RANDOM.Next(minY, maxY)];
            if (tile.HasInhabitant)
            {
                return FindRandomNearbyEmptyTile(organism);
            }

            return tile;
        }
        public static int ManhattanDistance(Organism a, Organism b)
        {
            return Math.Abs(a.GridIndex.X - b.GridIndex.X) + Math.Abs(a.GridIndex.Y - b.GridIndex.Y);
        }
    }
}
