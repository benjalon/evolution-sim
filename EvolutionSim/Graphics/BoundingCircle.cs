using Microsoft.Xna.Framework;

namespace EvolutionSim
{
    // Code adapted from rbwhitaker.wikidot.com/circle-collision-detection
    public class BoundingCircle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }
        
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        
        public bool Contains(Rectangle rectangle)
        {
            return ((rectangle.Center.ToVector2() - Center).Length() <= Radius);
        }
    }
}
