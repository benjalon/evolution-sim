using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EvolutionSim.Utility
{
    public enum WeatherSettings
    {
        Warm,
        Cold,
        Hot
    }

    public class WeatherManager : FullScreenSprite
    {
        private readonly Texture2D coldTexture;
        private readonly Texture2D hotTexture;

        private WeatherSettings currentWeather = WeatherSettings.Warm;

        public WeatherManager(Texture2D coldTexture, Texture2D hotTexture): base()
        {
            this.coldTexture = coldTexture;
            this.hotTexture = hotTexture;
        }

        public void Update(List<Organism> organisms)
        {
            if (!TimeManager.HAS_SIMULATION_TICKED)
            {
                return; // Wait a bit
            }

            int organismsCount;
            Organism organism;
            switch (this.currentWeather)
            {
                case WeatherSettings.Cold:
                    organismsCount = organisms.Count;
                    for (var i = organismsCount - 1; i >= 0; i--)
                    {
                        organism = organisms[i];
                        if (!organism.Attributes.ResistCold)
                        {
                            organism.DecreaseHealth(1);
                        }
                    }
                    break;
                case WeatherSettings.Hot:
                    organismsCount = organisms.Count;
                    for (var i = organismsCount - 1; i >= 0; i--)
                    {
                        organism = organisms[i];
                        if (!organism.Attributes.ResistHeat)
                        {
                            organism.DecreaseHealth(1);
                        }
                    }
                    break;
                case WeatherSettings.Warm:
                default:
                    break;
            }
        }
        
        public void SetWeather(WeatherSettings setting)
        {
            this.currentWeather = setting;

            switch (setting)
            {
                case WeatherSettings.Cold:
                    this.texture = this.coldTexture;
                    break;
                case WeatherSettings.Hot:
                    this.texture = this.hotTexture;
                    break;
                case WeatherSettings.Warm:
                default:
                    this.texture = null;
                    break;
            }
        }
    }
}
