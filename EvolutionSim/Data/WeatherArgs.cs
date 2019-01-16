using System;

namespace EvolutionSim.Data
{
    /// <summary>
    /// Different weather settings possible
    /// </summary>
    public enum WeatherSettings
    {
        Warm,
        Cold,
        Hot
    }

    /// <summary>
    /// object to handle weather arguments passed
    /// </summary>
    public class WeatherArgs : EventArgs
    {
        public WeatherSettings weatherSetting { get; private set; }

        public WeatherArgs(WeatherSettings weatherSetting) : base()
        {
            this.weatherSetting = weatherSetting;
        }
    }
}
