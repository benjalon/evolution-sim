using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvolutionSim
{
    class Organism : MovingSprite
    {

        public static int TOTAL_POPULATION = 0;
        private OrganismAttributes _attributes;
        private String _speciesName;


        // private OrganismState _state;

        public Organism(ref Texture2D texture, Rectangle rectangle, int movementSpeed = 2, float detectionRadius = -1.0f)
            : base(ref texture, rectangle, movementSpeed, detectionRadius)
        {
            _attributes = new OrganismAttributes(100, 0, 50, 500, 50, FoodType.Carnivore);
            TOTAL_POPULATION++;
        }

        public override String ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Species Name: ").Append(_speciesName).Append(Environment.NewLine);
            stringBuilder.Append("Health: ").Append(_attributes._health).Append(Environment.NewLine);
            stringBuilder.Append("Age: ").Append(_attributes._age).Append(Environment.NewLine);
            stringBuilder.Append("Speed: ").Append(_attributes._speed).Append(Environment.NewLine);
            return stringBuilder.ToString();

        }
    }
}
