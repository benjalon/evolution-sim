using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvolutionSim
{
    public class Organism : MapItem
    {
        public static int TOTAL_POPULATION = 0;
        public OrganismAttributes _attributes;
        private String _speciesName;

        //what state is the organism currently in
        public PotentialStates organismState { get; set; }

        public MapItem Sprite { get; set; }

        float health_passed = 1;
        int age_passed = 0;
        const int timeConst = 20000;
        double hunger_passed = 0.6;
        double speed_passed = 500;
        double strength_passsed = 50;
        FoodType foodType_passed = FoodType.Carnivore;

        // private OrganismState _state;

        public Organism(ref Texture2D texture)
            : base(ref texture)
        {
            _attributes = new OrganismAttributes(health_passed, age_passed, hunger_passed, speed_passed, strength_passsed, foodType_passed);
            TOTAL_POPULATION++;
            organismState = PotentialStates.Roaming;
            System.Diagnostics.Debug.WriteLine("Total Population: " + TOTAL_POPULATION);
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

       

        /// <summary>
        /// Constantly reduce food over time
        /// </summary>
        public void decrementAttributes(GameTime gameTime)
        {

            //hardcode a value that doesn't go down too fast
            _attributes._hunger -= 0.00005;

            Console.WriteLine(_attributes._hunger);
           
        }

    }

    }
