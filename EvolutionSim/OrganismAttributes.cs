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
        public int _DetectionRadius;
        public int _DetectionDiameter;
        public FoodType _foodType;


        public OrganismAttributes(float health_passed,
                                  int age_passed,
                                  double hunger_passed,
                                  double speed_passed,
                                  double strength_passsed,
                                  FoodType foodType_passed)
        {
            _DetectionRadius = 3;
            _DetectionDiameter = _DetectionRadius * 2;
            _health = health_passed;
            _age = age_passed;
            _hunger = 0;
            _speed = speed_passed;
            _strength = strength_passsed;
            _foodType = foodType_passed;

        }





    }
}
