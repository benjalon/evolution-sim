﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Food : MapItem
    {
        private BoundingCircle _detectionArea;
        public FoodType foodType { get; set; }
        public int foodHealth{ get; set; }


        public Food(Texture2D texture, FoodType foodType, int foodHealth)
            : base(texture)
        {
            this.foodHealth = foodHealth;
            this.foodType = foodType;
            // ' _detectionArea = new BoundingCircle(rectangle.Center.ToVector2(), detectionRadius);
        }

        public void decrementHealth(GameTime gameTime)
        {
            //decrement the health of the food
            if(this.foodHealth  > 0)
            {

                foodHealth -= 1;

            }

            else
            {

                this.foodHealth = 0;
            }



        } 

    }
}
