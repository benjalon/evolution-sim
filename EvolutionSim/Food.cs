using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Food : MapItem
    {
        private BoundingCircle _detectionArea;
        public FoodType foodType { get; set; }
        public int foodHealth { get; set; }


        public Food(Texture2D texture, FoodType foodType, int foodHealth)
            : base(texture)
        {
            this.foodHealth = foodHealth;
            this.foodType = foodType;
            this.foodHealth = 10;
            // ' _detectionArea = new BoundingCircle(rectangle.Center.ToVector2(), detectionRadius);
        }
    }
}
