using EvolutionSim.Data;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EvolutionSim.Sprites
{
    public class WeatherOverlay : FullScreenSprite
    {
        private readonly Texture2D coldTexture;
        private readonly Texture2D hotTexture;

        private WeatherSettings weatherSetting = WeatherSettings.Warm;
        public WeatherSettings WeatherSetting
        {
            get => weatherSetting;
            set
            {
                this.weatherSetting = value;

                switch (weatherSetting)
                {
                    case WeatherSettings.Cold:
                        this.Texture = this.coldTexture;
                        break;
                    case WeatherSettings.Hot:
                        this.Texture = this.hotTexture;
                        break;
                    case WeatherSettings.Warm:
                    default:
                        this.Texture = null;
                        break;
                }
            }
        }

        public WeatherOverlay(Texture2D coldTexture, Texture2D hotTexture): base()
        {
            this.coldTexture = coldTexture;
            this.hotTexture = hotTexture;
        }

        public void Update(List<Organism> organisms)
        { 
            Organism organism;
            switch (this.weatherSetting)
            {
                case WeatherSettings.Cold:
                    for (var i = organisms.Count - 1; i >= 0; i--)
                    {
                        organism = organisms[i];
                        if (!organism.Attributes.ResistCold)
                        {
                            organism.DecreaseHealth(1);
                        }
                    }
                    break;
                case WeatherSettings.Hot:
                    for (var i = organisms.Count - 1; i >= 0; i--)
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
    }
}
