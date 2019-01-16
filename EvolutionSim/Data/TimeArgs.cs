using System;

namespace EvolutionSim.Data
{
    /// <summary>
    /// The different speeds which the simulation can be set to
    /// </summary>
    public enum TimeSettings
    {
        Normal,
        Fast,
        Paused
    }

    /// <summary>
    /// Object to handle time adjustments
    /// </summary>
    public class TimeArgs : EventArgs
    {
        public TimeSettings TimeSetting { get; private set; }

        public TimeArgs(TimeSettings timeSetting) : base()
        {
            TimeSetting = timeSetting;
        }
    }
}
