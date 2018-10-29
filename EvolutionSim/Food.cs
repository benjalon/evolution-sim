using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Food : MapItem
    {
        private BoundingCircle _detectionArea;
        public FoodType foodType { get; set; }
        public int foodHealth{ get; set; }


        public Food(ref Texture2D texture,FoodType foodType, int foodHealth)
            : base(ref texture)
        {
            this.foodHealth = foodHealth;
            this.foodType = foodType;
          // ' _detectionArea = new BoundingCircle(rectangle.Center.ToVector2(), detectionRadius);
        }
    }
}
