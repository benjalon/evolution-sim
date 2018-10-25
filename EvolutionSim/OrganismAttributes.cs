using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    public class OrganismAttributes
    {
        public int _age { get; set; }
        public float _health { get; set; }
        public double _hunger { get; set; }
        public double _speed { get; set; }
        public double _strength { get; set; }
        public FoodType _foodType;


        public OrganismAttributes(float health_passed,
                                  int age_passed,
                                  double hunger_passed,
                                  double speed_passed,
                                  double strength_passsed,
                                  FoodType foodType_passed)
        {

            _health = health_passed;
            _age = age_passed;
            _hunger = hunger_passed;
            _speed = speed_passed;
            _strength = strength_passsed;
            _foodType = foodType_passed;

        }



    }
}
