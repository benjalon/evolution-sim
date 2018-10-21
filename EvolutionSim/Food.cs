using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Food : Sprite
    {
        private BoundingCircle _detectionArea;

        public Food(ref Texture2D texture, Rectangle rectangle, float detectionRadius)
            : base(ref texture, rectangle)
        {
            _detectionArea = new BoundingCircle(rectangle.Center.ToVector2(), detectionRadius);
        }
    }
}
