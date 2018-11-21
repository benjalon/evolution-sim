using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Utility
{
    /// <summary>
    /// A full screen sprite.
    /// </summary>
    public class Background : Sprite
    {
        public Background(Texture2D texture, int screenWidth, int screenHeight) : base(texture, new Rectangle(0, 0, screenWidth, screenHeight)) { }
    }
}
