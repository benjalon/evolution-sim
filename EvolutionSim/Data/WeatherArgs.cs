using System;

namespace EvolutionSim.Data
{
    public enum WeatherSettings
    {
        Warm,
        Cold,
        Hot
    }

    public class WeatherArgs : EventArgs
    {
        public WeatherSettings weatherSetting { get; private set; }

        public WeatherArgs(WeatherSettings weatherSetting) : base()
        {
            this.weatherSetting = weatherSetting;
        }
    }
}
