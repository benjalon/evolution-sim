using Microsoft.Xna.Framework;

namespace EvolutionSim.Utility
{
    public class TimeManager
    {
        public static int DELTA_MS { get; private set; }
        public static float MOVE_SPEED { get; private set; } = 0.01f;
        
        private const int DEFAULT_MATING_COOLDOWN = 9000;
        private const int DEFAULT_ACTION_COOLDOWN = 600;

        private int MATING_COOLDOWN = DEFAULT_MATING_COOLDOWN;
        private int ACTION_COOLDOWN = DEFAULT_ACTION_COOLDOWN;
        
        public void Update(GameTime gameTime)
        {
            DELTA_MS = gameTime.ElapsedGameTime.Milliseconds;
        }

        public void SetSpeed(int multiplier)
        {
            MATING_COOLDOWN = (int)DEFAULT_MATING_COOLDOWN / multiplier;
            ACTION_COOLDOWN = (int)DEFAULT_ACTION_COOLDOWN / multiplier;
        }

        public bool ActionCooldownExpired(int msSinceLastMovement)
        {
            return msSinceLastMovement > ACTION_COOLDOWN;
        }

        public bool MatingCooldownExpired(int msSinceLastMovement)
        {
            return msSinceLastMovement > MATING_COOLDOWN;
        }
    }
}
