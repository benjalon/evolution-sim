using EvolutionSim.Data;
using EvolutionSim.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.Utility
{
    public static class AttributeUpdater
    {
        public static void UpdateAttributes(List<Organism> organisms, WeatherSettings weatherSettings)
        {
            Organism organism;

            for (var i = organisms.Count - 1; i >= 0; i--)
            {
                organism = organisms[i];
                UpdateWeatherAttribute(organism, weatherSettings);
                UpdateHungerAttribute(organism);
                UpdateAgeAttribute(organism);
            }

        }

        private static void UpdateWeatherAttribute(Organism organism, WeatherSettings weatherSettings)
        {
            switch (weatherSettings)
            {
                case WeatherSettings.Cold:

                        if (!organism.Attributes.ResistCold)
                        {
                            organism.DecreaseHealth(1);
                        }

                    break;
                case WeatherSettings.Hot:
                        if (!organism.Attributes.ResistHeat)
                        {
                            organism.DecreaseHealth(1);
                        }
                    break;
                case WeatherSettings.Warm:
                default:
                    break;
            }
        }
        private static void UpdateAgeAttribute(Organism organism)
        {
            organism.Age += 1;
            if (organism.Age > 1000)
            {
                organism.DecreaseHealth(999);
            }


        }
        private static void UpdateHungerAttribute(Organism organism)
        {
            if (organism.Hunger > 0)
            {
                organism.Hunger -= 0.001f;
                //organism.IncreaseHealth(1); // TODO: Maybe organisms should heal up over time?
            }
            else
            {
                organism.Hunger = 0;
                organism.DecreaseHealth(1);
            }
        }
    }
}
