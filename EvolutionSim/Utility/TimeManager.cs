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
        
        public void Update(GameTime gameTime)
        {
            DELTA_MS = gameTime.ElapsedGameTime.Milliseconds;
        }

        public void SetSpeed(int multiplier)
        {
            matingCooldown = (int)DEFAULT_MATING_COOLDOWN / multiplier;
            actionCooldown = (int)DEFAULT_ACTION_COOLDOWN / multiplier;
        }

        public bool ActionCooldownExpired(int msSinceLastAction)
        {
            return msSinceLastAction > actionCooldown;
        }

        public bool MatingCooldownExpired(int msSinceLastMate)
        {
            return msSinceLastMate > matingCooldown;
        }
    }
}
