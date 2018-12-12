using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Utility
{
    /// <summary>
    /// A full screen sprite.
    /// </summary>
    public class FullScreenSprite : Sprite
    {
        public FullScreenSprite(Texture2D texture) : base(texture, new Rectangle(0, 0, Graphics.WINDOW_WIDTH, Graphics.WINDOW_HEIGHT)) { }
        public FullScreenSprite() : base(new Rectangle(0, 0, Graphics.WINDOW_WIDTH, Graphics.WINDOW_HEIGHT)) { }
    }
}
