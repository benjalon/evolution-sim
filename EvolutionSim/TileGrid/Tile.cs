﻿using EvolutionSim.Data;
using EvolutionSim.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid
{

    public class Tile
    {
        public const int TILE_SIZE = 32;

        public Terrain Terrain { get; private set; }

        public Point GridIndex { get; private set; } // The index of this tile on the grid, this is not the object's actual screen position
        public int ScreenPositionX { get => GridIndex.X * TILE_SIZE; } // The actual screen position of the tile, this is not the grid index
        public int ScreenPositionY { get => GridIndex.Y * TILE_SIZE; } // The actual screen position of the tile, this is not the grid index

        public int MovementDifficulty { get => Terrain.MovementDifficulty; }

        public GridItem Inhabitant { get; private set; } // The inhabitant being managed by this tile, can be food or organisms
        
        public bool HasInhabitant { get => Inhabitant != null; }
        public bool HasOrganismInhabitant { get => HasInhabitant && Inhabitant.GetType() == typeof(Organism); }
        public bool HasFoodInhabitant { get => HasInhabitant && Inhabitant.GetType() == typeof(Food); }

        public Vector2 Center { get => new Vector2(ScreenPositionX + TILE_SIZE * 0.5f, ScreenPositionY + TILE_SIZE * 0.5f); }

        /// <summary>
        /// Create a new tile object.
        /// </summary>
        /// <param name="tileTexture">Tile debug texture.</param>
        /// <param name="mountainTexture">Mountain terrain texture.</param>
        /// <param name="waterTexture">Water terrain texture.</param>
        /// <param name="gridIndex">The index in the grid to position this tile at.</param>
        public Tile(Texture2D mountainTexture, Texture2D waterTexture, Point gridIndex)
        {
            this.GridIndex = gridIndex;

            this.Terrain = new Terrain(new Texture2D[] { null, mountainTexture, waterTexture }, new Rectangle(this.ScreenPositionX, this.ScreenPositionY, TILE_SIZE, TILE_SIZE));
        }

        /// <summary>
        /// Set an inhabitant to this tile. This should only be used to place inhabitants in the grid for the first time, never to reposition them.
        /// </summary>
        /// <param name="gridItem">The item to place on the grid.</param>
        public void AddInhabitant(GridItem gridItem)
        {
            Inhabitant = gridItem;
            gridItem.SetGridIndex(GridIndex.X, GridIndex.Y);
        }

        /// <summary>
        /// Remove the inhabitant from this tile by taking away the reference.
        /// </summary>
        public void RemoveInhabitant()
        {
            Inhabitant = null;
        }

        /// <summary>
        /// Repositions an inhabitant, cleaning up their existing reference.
        /// </summary>
        /// <param name="destination">The tile to move the item to.</param>
        public void MoveInhabitant(Tile destination)
        {
            if (HasInhabitant)
            {
                // Move and reference the item to the destination tile
                Inhabitant.SetGridIndex(destination.GridIndex.X, destination.GridIndex.Y);
                destination.Inhabitant = Inhabitant;

                Inhabitant = null; // This tile is no longer managing the item
            }
        }

        /// <summary>
        /// Set the terrain of this tile to the given type.
        /// </summary>
        /// <param name="terrainType">The type of terrain to set.</param>
        public void SetTerrain(TerrainTypes terrainTypes)
        {
            Terrain.Type = terrainTypes;
        }
    }
}
