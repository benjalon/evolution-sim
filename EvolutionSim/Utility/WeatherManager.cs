﻿using EvolutionSim.TileGrid.GridItems;
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

        public void Update(List<Organism> organisms, TimeManager timeManager)
        {
            if (!timeManager.HasSimulationTicked)
            {
                return; // Wait a bit
            }

            int organismsCount;
            Organism organism;
            switch (this.currentWeather)
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
        
        public void SetWeather(WeatherSettings setting)
        {
            this.currentWeather = setting;

            switch (setting)
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
}
