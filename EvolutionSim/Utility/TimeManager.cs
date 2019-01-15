using EvolutionSim.Data;
using EvolutionSim.Sprites;
using Microsoft.Xna.Framework;

namespace EvolutionSim.Utility
{
    public class TimeManager
    {
        private const int DEFAULT_MATING_COOLDOWN = 20000;
        private const int DEFAULT_WEATHER_COOLDOWN = 5000;
        private const int DEFAULT_HUNTING_COOLDOWN = 30000;
        private const int DEFAULT_ROAM_COOLDOWN = 3000;
        private const int DEFAULT_SIMULATION_TICK_COOLDOWN = 1000;
        private const int DEFAULT_GRASS_SPAWN_COOLDOWN = 1500;

        public const float MOVE_SPEED = 0.01f;
        private const int FAST_SPEED = 4;


        private int simulationTickCooldown = DEFAULT_SIMULATION_TICK_COOLDOWN;
        private int matingCooldown = DEFAULT_MATING_COOLDOWN;
        private int huntingCooldown = DEFAULT_HUNTING_COOLDOWN;
        private int roamCooldown = DEFAULT_ROAM_COOLDOWN;
        private int weatherCooldown = DEFAULT_WEATHER_COOLDOWN;
        private int grassCooldown = DEFAULT_GRASS_SPAWN_COOLDOWN;

        



        private int deltaMs;
        private int msSinceLastTick = 0;
        private int msSinceLastGrass = 0;

        public bool Paused { get; set; } = false;
        private int pausedElapsed = 0;

        public bool HasSimulationTicked { get => msSinceLastTick > simulationTickCooldown + pausedElapsed; }
        public bool HasGrassTicked { get => msSinceLastGrass > grassCooldown + pausedElapsed; }


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
                        weatherCooldown = DEFAULT_WEATHER_COOLDOWN;
                        grassCooldown = DEFAULT_GRASS_SPAWN_COOLDOWN;
                        Paused = false;
                        break;
                    case TimeSettings.Fast:
                        matingCooldown = DEFAULT_MATING_COOLDOWN / FAST_SPEED;
                        roamCooldown = DEFAULT_ROAM_COOLDOWN / FAST_SPEED;
                        simulationTickCooldown = DEFAULT_SIMULATION_TICK_COOLDOWN / FAST_SPEED;
                        weatherCooldown = DEFAULT_WEATHER_COOLDOWN/FAST_SPEED;
                        grassCooldown = DEFAULT_GRASS_SPAWN_COOLDOWN/FAST_SPEED;
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



        // A simulation tick is a global progression of events. Each time this happens food gets eaten slightly, mating progresses, organisms get slightly more hungry etc.




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

            UpdateGrassSpawn(gameTime);

        }

        private void UpdateGrassSpawn(GameTime gameTime)
        {
            if (HasGrassTicked)
            {
                // SPAWN GRASS

                msSinceLastGrass = 0;
                
            }
            else
            {
                msSinceLastGrass += this.deltaMs;
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

            organism.MsSinceLastWeather += this.deltaMs;
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

        public bool HasRoamingCooldownExpired(Organism organism, int multiplier = 1)
        {
            var cooldownExpired = organism.MsSinceLastRoam > (roamCooldown + pausedElapsed) * multiplier;

            if (cooldownExpired)
            {
                organism.MsSinceLastRoam = 0;
            }

            return cooldownExpired;
        }

        public bool HasWeatherCooldownExpired(Organism organism, int multiplier = 1)
        {
            var cooldownExpired = organism.MsSinceLastWeather > (weatherCooldown + pausedElapsed) * multiplier;

            if (cooldownExpired)
            {
                organism.MsSinceLastWeather = 0;
            }

            return cooldownExpired;
        }
    }
}
