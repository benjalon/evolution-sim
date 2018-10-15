﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim
{
    class Organism
    {
        private OrganismAttributes _attributes;
        private String _speciesName;
        public Source.Sprite _sprite { get; set; }


        // private OrganismState _state;

        public Organism(Source.Sprite sprite_Passed)
        {
            _attributes = new OrganismAttributes(100, 0, 50, 500, 50, FoodType.Carnivore);
            _sprite = sprite_Passed;
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