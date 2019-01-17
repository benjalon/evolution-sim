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
        private const float STARVING_THRESHOLD = 0.1f;
        private const float HUNGRY_RATE = 0.003f;
        
       

        public static void UpdateAttributes(List<Organism> organisms, WeatherSettings weatherSettings, Boolean SimulationTick, TimeManager timeManager)
        {
     
        Organism organism;

            for (var i = organisms.Count - 1; i >= 0; i--)
            {
                organism = organisms[i];
                if (SimulationTick)
                {
                    if (timeManager.HasWeatherCooldownExpired(organism))
                    {
                        UpdateWeatherAttribute(organism, weatherSettings);
                    }
                    UpdateHungerAttribute(organism);
                    UpdateAgeAttribute(organism);
                }
                timeManager.UpdateOrganismTimers(organism);
            }


        }

        /// <summary>
        /// This handles the decrementing of stats based on the weather conditions
        /// </summary>
        /// <param name="organism"></param>
        /// <param name="weatherSettings"></param>
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
            if (organism.Age > organism.MaxAge)
            {
                //kill the organism
                organism.DecreaseHealth(Organism.KILL_HEALTH);
            }


        }
        private static void UpdateHungerAttribute(Organism organism)
        {
            if (organism.Hunger > STARVING_THRESHOLD)
            {
                organism.Hunger -= HUNGRY_RATE;
                //organism.IncreaseHealth(1); // TODO: Maybe organisms should heal up over time?
            }
            else //organism is starving so reduce health
            {
                organism.Hunger = 0;
                organism.DecreaseHealth(1);
            }
        }
    }
}
