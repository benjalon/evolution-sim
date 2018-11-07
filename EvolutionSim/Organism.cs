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

        public Tile DestinationTile;

       

        public const int MS_PER_DIRECTION_CHANGE = 2000;

        public int MilliSecondsSinceLastMovement;
        //what state is the organism currently in
        public PotentialStates organismState { get; set; }
        public Boolean MovingOnPath{ get; set; }
        public List<Tile> _Path { get; set; }
 
        
        public MapItem Sprite { get; set; }
        
        int age_passed = 0;
        double hunger_passed = 0.6;
        double speed_passed = 500;
        double strength_passsed = 50;
        FoodType foodType_passed = FoodType.Carnivore;

        // private OrganismState _state;

        public Organism(Texture2D texture)
            : base(texture)
        {
            _attributes = new OrganismAttributes(age_passed, hunger_passed, speed_passed, strength_passsed, foodType_passed);
            TOTAL_POPULATION++;
            organismState = PotentialStates.Roaming;
            _Path = new List<Tile>();
        }

        public override String ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Species Name: ").Append(_speciesName).Append(Environment.NewLine);
            stringBuilder.Append("Age: ").Append(_attributes._age).Append(Environment.NewLine);
            stringBuilder.Append("Speed: ").Append(_attributes._speed).Append(Environment.NewLine);
            return stringBuilder.ToString();

        }

       

        /// <summary>
        /// Constantly reduce food over time
        /// </summary>
  

        }

    }
