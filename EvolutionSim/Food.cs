using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Food : MapItem
    {
        public FoodType foodType { get; private set; }
        
        public Food(Texture2D texture, FoodType foodType)
            : base(texture)
        {
            this.foodType = foodType;
        }

        /// <summary>
        /// Eats the food, lowering its health over time
        /// </summary>
        /// <returns>Whether or not the food is fully eaten</returns>
        public void Eat()
        {
            LowerHealth(999); // In the future this should take time but for now it's instant
        }
    }
}
