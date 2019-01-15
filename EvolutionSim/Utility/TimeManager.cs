using EvolutionSim.Data;
using EvolutionSim.Sprites;
using Microsoft.Xna.Framework;

namespace EvolutionSim.Utility
{
    public class TimeManager
    {
        public const float MOVE_SPEED = 0.01f;
        private const int FAST_SPEED = 4;

        public TimeSettings TimeSetting
        {
            set
            {
                switch (value)
                {
                    case TimeSettings.Normal:
                        matingCooldown = DEFAULT_MATING_COOLDOWN;
                        roamCooldown = DEFAULT_ROAM_COOLDOWN;
                        simulationTickCooldown = DEFAULT_SIMULATION_TICK_COOLDOWN;
                        Paused = false;
                        break;
                    case TimeSettings.Fast:
                        matingCooldown = DEFAULT_MATING_COOLDOWN / FAST_SPEED;
                        roamCooldown = DEFAULT_ROAM_COOLDOWN / FAST_SPEED;
                        simulationTickCooldown = DEFAULT_SIMULATION_TICK_COOLDOWN / FAST_SPEED;
                        Paused = false;
                        break;
                    case TimeSettings.Paused:
                        Paused = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private int deltaMs;

        // A simulation tick is a global progression of events. Each time this happens food gets eaten slightly, mating progresses, organisms get slightly more hungry etc.
        private const int DEFAULT_SIMULATION_TICK_COOLDOWN = 1000;
        private int simulationTickCooldown = DEFAULT_SIMULATION_TICK_COOLDOWN;
        private int msSinceLastTick = 0;
        public bool HasSimulationTicked { get => msSinceLastTick > simulationTickCooldown + pausedElapsed; }

        private const int DEFAULT_MATING_COOLDOWN = 20000;
        private const int DEFAULT_HUNTING_COOLDOWN = 20000;
        private int matingCooldown = DEFAULT_MATING_COOLDOWN;
        private int huntingCooldown = DEFAULT_HUNTING_COOLDOWN;

        private const int DEFAULT_ROAM_COOLDOWN = 3000;
        private int roamCooldown = DEFAULT_ROAM_COOLDOWN;

        public bool Paused { get; set; } = false;
        private int pausedElapsed = 0;

        public void Update(GameTime gameTime)
        {
            this.deltaMs = gameTime.ElapsedGameTime.Milliseconds;

            if (HasSimulationTicked)
            {
                msSinceLastTick = 0;
            }
            else
            {
                msSinceLastTick += this.deltaMs;
            }

            if (Paused)
            {
                pausedElapsed += this.deltaMs;
            }
            else if (pausedElapsed > 0)
            {
                pausedElapsed -= this.deltaMs;
            }
        }

        public void UpdateOrganismTimers(Organism organism)
        {
            organism.MsSinceLastMate += this.deltaMs;

            if (organism.DestinationTile == null)
            {
                organism.MsSinceLastRoam += this.deltaMs;

            }

            organism.MsSinceLastHunted += this.deltaMs;
        }

        public bool HasMatingCooldownExpired(Organism organism)
        {
            var cooldownExpired = organism.MsSinceLastMate > matingCooldown + pausedElapsed;

            if (cooldownExpired)
            {
                organism.MsSinceLastMate = 0;
            }

            return cooldownExpired;
        }


        public bool HasHuntingCooldownExpired(Organism organism)
        {

            var cooldownExpired = organism.MsSinceLastHunted > huntingCooldown + pausedElapsed;

            if(cooldownExpired)
            {
                organism.MsSinceLastHunted = 0;
            }

            return cooldownExpired;

        }

        public bool HasRoamingCooldownExpired(Organism organism)
        {
            var cooldownExpired = organism.MsSinceLastRoam > roamCooldown + pausedElapsed;

            if (cooldownExpired)
            {
                organism.MsSinceLastRoam = 0;
            }

            return cooldownExpired;
        }
    }
}
