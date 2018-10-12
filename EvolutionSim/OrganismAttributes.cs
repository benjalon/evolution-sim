using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    class OrganismAttributes
    {
        public int _age { get; set; }
        public double _health { get; set; }
        public double _hunger { get; set; }
        public double _speed { get; set; }
        public double _strength { get; set; }
        public FoodType _foodType;


        public OrganismAttributes(double Health_Passed,
                                  int Age_Passed,
                                  double Hunger_Passed,
                                  double Speed_Passed,
                                  double Strength_Passsed,
                                  FoodType FoodType_Passed)
        {

            _health = Health_Passed;
            _age = Age_Passed;
            _hunger = Hunger_Passed;
            _speed = Speed_Passed;
            _strength = Strength_Passsed;
            _foodType = FoodType_Passed;

        }



    }
}
