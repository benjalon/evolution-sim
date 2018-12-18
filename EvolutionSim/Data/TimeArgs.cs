using System;

namespace EvolutionSim.Data
{
    public enum TimeSettings
    {
        Normal,
        Fast,
        Paused
    }

    public class TimeArgs : EventArgs
    {
        public TimeSettings TimeSetting { get; private set; }

        public TimeArgs(TimeSettings timeSetting) : base()
        {
            TimeSetting = timeSetting;
        }
    }
}
