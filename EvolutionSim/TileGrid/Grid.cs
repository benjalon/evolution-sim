﻿using EvolutionSim.StateManagement;
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
            for (var i = 0; i < HorizontalCount; i++)
            {
                for (var j = 0; j < VerticalCount; j++)
                {
                    this.tiles[i][j].Draw(spriteBatch);
                }
            }

            foreach (Food food in Foods)
            {
                food.Draw(spriteBatch);
            }

            foreach (Organism organism in Organisms)
            {
                organism.Draw(spriteBatch);
            }
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

        public Tile GetTileAt(int x, int y)
        {
            return this.tiles[x][y];
        }

        public Tile GetTileAt(GridItem item)
        {
            return this.tiles[item.GridPosition.X][item.GridPosition.Y];
        }

        public bool IsFoodAt(int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            return inhabitant != null && inhabitant.GetType() == typeof(Food);
        }

        public bool IsMateAt(Organism organism, int x, int y)
        {
            var inhabitant = this.tiles[x][y].Inhabitant;
            if (inhabitant == organism)
            {
                return false;
            }
            return inhabitant != null && inhabitant.GetType() == typeof(Organism) && ((Organism)inhabitant).OrganismState == PotentialStates.SeekMate;
        }

        public static Boolean InBounds(int x, int y)
        {
            if (y >= Grid.VerticalCount || y < 0 || x >= Grid.HorizontalCount || x < 0)
            {
                return false;
            }
            return true;
        }

        private void OrganismDeathHandler(object sender, EventArgs e)
        {
            var organism = (Organism)sender;

            this.GetTileAt(organism).RemoveInhabitant();
            this.Organisms.Remove(organism);
        }

        private void FoodDeathHandler(object sender, EventArgs e)
        {
            var food = (Food)sender;

            this.GetTileAt(food).RemoveInhabitant();
            this.Foods.Remove(food);
        }
    }
}
