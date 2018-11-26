using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Utility
{
    public class MouseManager
    {
        private MouseState mouseState;
        private MouseState mouseStateOld;

        public int ScreenPositionX { get => this.mouseState.X; }
        public int ScreenPositionY { get => this.mouseState.Y; }
        public int TileIndexX { get => this.mouseState.X / Tile.TILE_SIZE; }
        public int TileIndexY { get => this.mouseState.Y / Tile.TILE_SIZE; }
        public bool IsClicked { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseState != this.mouseStateOld; }
        public bool IsWithinGrid { get => Grid.InBounds(this.TileIndexX, this.TileIndexY); }
        public bool IsClickedWithinGrid { get => this.IsClicked && this.IsWithinGrid; }

        public void Update()
        {
            this.mouseStateOld = this.mouseState;
            this.mouseState = Mouse.GetState();
        }
    }
}
