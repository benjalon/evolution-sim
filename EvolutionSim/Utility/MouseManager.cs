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
        public bool IsWithinGrid { get => this.TileIndexX >= 0 && this.TileIndexX < Grid.TILE_COUNT_X && this.TileIndexY >= 0 && this.TileIndexY < Grid.TILE_COUNT_Y; }

        public bool IsClicked { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseStateOld.LeftButton == ButtonState.Released; }
        public bool IsClickedWithinGrid { get => this.IsClicked && this.IsWithinGrid; }

        public bool IsHeld { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseStateOld.LeftButton == ButtonState.Pressed; }
        public bool IsHeldWithinGrid { get => this.IsHeld && this.IsWithinGrid; }

        public void Update()
        {
            this.mouseStateOld = this.mouseState;
            this.mouseState = Mouse.GetState();
        }
    }
}
