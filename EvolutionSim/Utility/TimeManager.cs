using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework;
using System;

namespace EvolutionSim.Utility
{
    public class TimeManager
    {
        public static float MOVE_SPEED { get; } = 0.01f;

        private int deltaMs;

        // A simulation tick is a global progression of events. Each time this happens food gets eaten slightly, mating progresses, organisms get slightly more hungry etc.
        private const int DEFAULT_SIMULATION_TICK_COOLDOWN = 1000;
        private static int SIMULATION_TICK_COOLDOWN = DEFAULT_SIMULATION_TICK_COOLDOWN;
        private static int MS_SINCE_LAST_TICK = 0;
        public static bool HAS_SIMULATION_TICKED { get => MS_SINCE_LAST_TICK > SIMULATION_TICK_COOLDOWN + PAUSED_ELAPSED; }

        private const int DEFAULT_MATING_COOLDOWN = 10000;
        private int matingCooldown = DEFAULT_MATING_COOLDOWN;

        public bool Paused { get; set; } = false;
        private static int PAUSED_ELAPSED = 0;

        public void Update(GameTime gameTime)
        {
            this.deltaMs = gameTime.ElapsedGameTime.Milliseconds;

            if (HAS_SIMULATION_TICKED)
            {
                //Console.WriteLine("tick");
                MS_SINCE_LAST_TICK = 0;
            }
            else
            {
                MS_SINCE_LAST_TICK += this.deltaMs;
            }
         
            if (Paused)
            {
                PAUSED_ELAPSED += this.deltaMs;
            }
            else if (PAUSED_ELAPSED > 0)
            {
                PAUSED_ELAPSED -= this.deltaMs;
            }
        }

        public void UpdateOrganismTimers(Organism organism)
        {
            organism.MsSinceLastMate += this.deltaMs;
        }

        public void SetSpeed(int multiplier)
        {
            Paused = false;
            matingCooldown = (int)DEFAULT_MATING_COOLDOWN / multiplier;
            SIMULATION_TICK_COOLDOWN = (int)DEFAULT_SIMULATION_TICK_COOLDOWN / multiplier;
        }

        public bool HasMatingCooldownExpired(Organism organism)
        {
            var cooldownExpired = organism.MsSinceLastMate > matingCooldown + PAUSED_ELAPSED;

            if (cooldownExpired)
            {
                organism.MsSinceLastMate = 0;
            }

            return cooldownExpired;
        }
    }
}
