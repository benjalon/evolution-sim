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

        public int PositionX { get => this.mouseState.X; }
        public int PositionY { get => this.mouseState.Y; }
        public bool IsClicked { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseState != this.mouseStateOld; }
        public bool IsHeld { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseState == this.mouseStateOld; }
        public bool IsReleased { get => this.mouseState.LeftButton == ButtonState.Released && this.mouseState != this.mouseStateOld; }

        public void Update()
        {
            this.mouseStateOld = mouseState;
            this.mouseState = Mouse.GetState();
        }
    }
}
