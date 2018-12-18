using Microsoft.Xna.Framework.Input;

namespace EvolutionSim.Utility
{
    public class MouseStateManager
    {
        private MouseState mouseState;
        private MouseState mouseStateOld;

        public int ScreenPositionX { get => this.mouseState.X; }
        public int ScreenPositionY { get => this.mouseState.Y; }
        public bool IsClicked { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseStateOld.LeftButton == ButtonState.Released; }
        public bool IsHeld { get => this.mouseState.LeftButton == ButtonState.Pressed && this.mouseStateOld.LeftButton == ButtonState.Pressed; }

        public void Update()
        {
            this.mouseStateOld = this.mouseState;
            this.mouseState = Mouse.GetState();
        }
    }
}
