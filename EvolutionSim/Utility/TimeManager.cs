using EvolutionSim.TileGrid.GridItems;
using Microsoft.Xna.Framework;

namespace EvolutionSim.Utility
{
    public class TimeManager
    {
        public static int DELTA_MS { get; private set; }

        public static float MOVE_SPEED { get; set; } = 0.01f;

        public static int DEFAULT_MATING_COOLDOWN { get; } = 9000;
        public static int DEFAULT_ACTION_COOLDOWN { get; } = 1200;

        private int matingCooldown = DEFAULT_MATING_COOLDOWN;
        private int actionCooldown = DEFAULT_ACTION_COOLDOWN;
        
        public bool Paused { get; set; } = false;
        private int pausedElapsed = 0;

        public void Update(GameTime gameTime)
        {
            DELTA_MS = gameTime.ElapsedGameTime.Milliseconds;
            
            if (Paused)
            {
                pausedElapsed += DELTA_MS;
            }
            else if (pausedElapsed > 0)
            {
                pausedElapsed -= DELTA_MS;
            }
        }

        public void UpdateOrganismTimers(Organism organism)
        {
            organism.MsSinceLastAction += TimeManager.DELTA_MS;
            organism.MsSinceLastMate += TimeManager.DELTA_MS;
        }

        public void SetSpeed(int multiplier)
        {
            Paused = false;
            matingCooldown = (int)DEFAULT_MATING_COOLDOWN / multiplier;
            actionCooldown = (int)DEFAULT_ACTION_COOLDOWN / multiplier;
        }

        public bool HasActionCooldownExpired(Organism organism)
        {
            var cooldownExpired = organism.MsSinceLastAction > actionCooldown + pausedElapsed;

            if (cooldownExpired)
            {
                organism.MsSinceLastAction = 0;
            }

            return cooldownExpired;
        }

        public bool HasMatingCooldownExpired(Organism organism)
        {
            var cooldownExpired = organism.MsSinceLastMate > actionCooldown + pausedElapsed;

            if (cooldownExpired)
            {
                organism.MsSinceLastMate = 0;
            }

            return cooldownExpired;
        }
    }
}
