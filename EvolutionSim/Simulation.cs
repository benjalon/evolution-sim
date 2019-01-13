using System;
using System.Collections.Generic;
using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.StateManagement;
using EvolutionSim.TileGrid;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    public class Simulation
    {

        private readonly Tuple<Texture2D, Texture2D> healthbarTextures;

        private readonly StateMachine fsm;
        private readonly Grid grid;
        private readonly FullScreenSprite background;

        public TimeManager TimeManager { get; private set; }
        public GridInteractionManager GridInteractionManager { get; private set; }
        public WeatherOverlay WeatherOverlay { get; private set; }

        private readonly List<Texture2D> particleTextures;
        private List<ParticleEffect> particleEffects = new List<ParticleEffect>();

        private readonly List<Attributes> bearBreeds;

        public Simulation()
        {


            this.particleTextures = new List<Texture2D>() { Graphics.SimulationTextures["star"], Graphics.SimulationTextures["diamond"], Graphics.SimulationTextures["circle"] };

            this.bearBreeds = new List<Attributes>()
            {
                new Attributes() { Species = "MiniGreen", Texture = Graphics.SimulationTextures["organism_0"], DietType = DietTypes.Herbivore, MaxHealth = 10, Strength = 0.3f, Speed = 0.7f, ResistCold = false, ResistHeat = false },
                new Attributes() { Species = "MysteryPurp", Texture = Graphics.SimulationTextures["organism_1"], DietType = DietTypes.Herbivore, MaxHealth = 20, Strength = 0.5f, Speed = 0.6f, ResistCold = true, ResistHeat = false },
                new Attributes() { Species = "Blastoise", Texture = Graphics.SimulationTextures["organism_2"], DietType = DietTypes.Omnivore, MaxHealth = 25, Strength = 0.7f, Speed = 0.1f, ResistCold = true, ResistHeat = true },
                new Attributes() { Species = "AngryRed", Texture = Graphics.SimulationTextures["organism_3"], DietType = DietTypes.Canivore, MaxHealth = 28, Strength = 0.8f, Speed = 0.2f, ResistCold = false, ResistHeat = true },
                new Attributes() { Species = "YellowBoi", Texture = Graphics.SimulationTextures["organism_4"], DietType = DietTypes.Omnivore, MaxHealth = 15, Strength = 0.5f, Speed = 0.5f, ResistCold = true, ResistHeat = true }
            };

            this.healthbarTextures = new Tuple<Texture2D, Texture2D>(Graphics.SimulationTextures["healthbar_red"], Graphics.SimulationTextures["healthbar_green"]);

            this.background = new FullScreenSprite(Graphics.SimulationTextures["grass_background"]);

            this.grid = new Grid(Graphics.SimulationTextures["tile"], Graphics.SimulationTextures["mountain"], Graphics.SimulationTextures["water"]);
            this.grid.ShouldSpawnCorpse += SpawnCorpseHandler;

            this.TimeManager = new TimeManager();

            this.fsm = new StateMachine();
            this.fsm.MatingOccurred += this.BirthHandler;


            this.GridInteractionManager = new GridInteractionManager(Graphics.SimulationTextures["tile"]);

            this.WeatherOverlay = new WeatherOverlay(Graphics.SimulationTextures["cold_overlay"], Graphics.SimulationTextures["hot_overlay"]);
        }

        /// <summary>
        /// Call all of the relevant update methods as
        /// the simulation progresses (propergates deltaT through the system)
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            TimeManager.Update(gameTime);
            GridInteractionManager.Update(this, this.grid);
            

            if (TimeManager.Paused)
            {
                return;
            }


            Utility.AttributeUpdater.UpdateAttributes(this.grid.Organisms, this.WeatherOverlay.WeatherSetting, TimeManager.HasSimulationTicked, TimeManager);

            this.grid.UpdateRay();

            this.fsm.UpdateStates(this.grid, TimeManager);
            
            for (var i = this.particleEffects.Count - 1; i >= 0; i--)
            {
                this.particleEffects[i].Update(gameTime);

                if (this.particleEffects[i].Complete)
                {
                    this.particleEffects.RemoveAt(i);
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            this.background.Draw(spriteBatch);
            this.WeatherOverlay.Draw(spriteBatch);

            this.grid.Draw(spriteBatch);

            this.GridInteractionManager.Draw(spriteBatch);
            
            for (var i = this.particleEffects.Count - 1; i >= 0; i--)
            {
                this.particleEffects[i].Draw(spriteBatch);
            }
        }

        private Tuple<int, int> MakeUseableValues(float rawX, float rawY)
        {
            var x = (int)(rawX * 10);
            var y = (int)(rawY * 10);
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }

        /// <summary>
        /// provide a way of determining the highest value from parent's attributes, this is indexed 
        /// in the first position of the tuple
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Tuple<int, int> MakeUseableValues(int x, int y)
        {
            var xIsHigher = x > y;
            return xIsHigher ? new Tuple<int, int>(y, x) : new Tuple<int, int>(x, y);
        }


        /// <summary>
        /// The birth handler is a method attacthed to the mating occured event and listens to when it is trigged in the FSM object
        /// It handles the creation of a new organism based on the stats of the mating couple and then attempts to place the child adjacent to the parents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void BirthHandler(object sender, EventArgs args)
        {


            const float EXTREME = 0.4f;
            const float MIDDLE = 0.2f;
            const float MILD = 0.1f;
   
            var matingArgs = (MatingArgs)args;
            var mother = matingArgs.Mother;
            var father = matingArgs.Father;
            var mutation = matingArgs.Mutation;
            var positioned = false;

            bool newResistHeat = false;
            bool newResistCold = false;
            float newStrength = 0.0f;
            float newSpeed = 0.0f;

            var orderedMaxHealth = MakeUseableValues(mother.Attributes.MaxHealth, father.Attributes.MaxHealth);
            var orderedStrength = MakeUseableValues(mother.Attributes.Strength, father.Attributes.Strength);
            var orderedSpeed = MakeUseableValues(mother.Attributes.Speed, father.Attributes.Speed);

            var simpleCrossbreed = new Attributes()
            {
                Species = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.Species : mother.Attributes.Species,
                Texture = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Texture : mother.Texture,
                DietType = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.DietType : mother.Attributes.DietType,
                MaxHealth = Graphics.RANDOM.Next(orderedMaxHealth.Item1, orderedMaxHealth.Item2),
                Strength = Graphics.RANDOM.Next(orderedStrength.Item1, orderedStrength.Item2) * 0.1f,
                Speed = Graphics.RANDOM.Next(orderedStrength.Item1, orderedStrength.Item2) * 0.1f,
                ResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold,
                ResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat,
            };

            //this takes into account the stdDeviation from the normal
            //workout an average of the mother and father's attributes
            //then offset the change based on the mutation variation

            #region Handle Mutation

            switch (mutation)
            {
                //organism is not resistant to cold or hot, and is crippled in both strength and speed (it will die fast)
                case MatingArgs.Severity.ExtremelyBad:

                    newResistCold = false;
                    newResistHeat = false;

                    //take an average of the parents strength and speed then take away according to mutation category
                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - EXTREME;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - EXTREME;
                    

                    break;
                case MatingArgs.Severity.MiddleBad:

                    newResistCold = false;
                    newResistHeat = false;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - MIDDLE;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - MIDDLE;

                    break;

                case MatingArgs.Severity.MildBad:

                    newResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold;
                    newResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) - MILD;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) - MILD;

                    break;

                case MatingArgs.Severity.MildGood:

                    newResistCold = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistCold : mother.Attributes.ResistCold;
                    newResistHeat = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.ResistHeat : mother.Attributes.ResistHeat;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + MILD;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) + MILD;

                    break;


                case MatingArgs.Severity.MiddleGood:

                    newResistCold = true;
                    newResistHeat = true;

                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + MIDDLE;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2) *0.1f /2) + MIDDLE;

                    break;


                case MatingArgs.Severity.ExtremelyGood:

                    newResistCold = true;
                    newResistHeat = true;


                    newStrength = ((orderedStrength.Item1 + orderedStrength.Item2)*0.1f / 2) + EXTREME;
                    newSpeed = ((orderedSpeed.Item1 + orderedSpeed.Item2)*0.1f / 2) + EXTREME;

                    break;

                default:
                    break;

            }

            var advancedCrossBreed = new Attributes()
                    {
                        //randomly pick between the mother and fathers for the following components:
                        Species = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.Species : mother.Attributes.Species,
                        Texture = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Texture : mother.Texture,
                        DietType = Graphics.RANDOM.NextDouble() >= 0.5 ? father.Attributes.DietType : mother.Attributes.DietType,

                        MaxHealth = Graphics.RANDOM.Next(orderedMaxHealth.Item1, orderedMaxHealth.Item2),
                        Strength = newStrength,
                        Speed = newSpeed,
                        ResistCold = newResistCold,
                        ResistHeat = newResistHeat,
                    };

            #endregion

            //create the nre child
            var child = new Organism(advancedCrossBreed, this.healthbarTextures);

            // Top left corner
            var birthSpot = mother.GridIndex;
            birthSpot.X -= 1;
            birthSpot.Y -= 1;

            // Position the child adjacent to the mother on an empty square
            for (var x = 0; !positioned && x < 3; x++)
            {
                birthSpot.X += x;

                for (var y = 0; !positioned && y < 3; y++)
                {
                    birthSpot.Y += y;

                    if (this.grid.AttemptToPositionAt(child, birthSpot.X, birthSpot.Y))
                    {
                        positioned = true; // We successfully positioned the child so we're done here
                    }
                }
            }

            // We've failed to position the child adjacently so the area must be crowded, just position anywhere on the map
            if (!positioned)
            {
                this.PositionAtRandom(child);
            }

            particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(child).Center));
        }

        /// <summary>
        /// Method to populate organisms randomly in the simulation based on user input
        /// </summary>
        /// <param name="amount"></param>
        public void AddOrganisms(int amount)
        {
            Organism organism;
            for (var i = 0; i < amount; i++)
            {
                organism = new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures);
                PositionAtRandom(organism);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="amount"></param>
        public void AddOrganisms(List<Attributes> attributes, int amount)
        {
            foreach(var attribute in attributes)
            {
                Organism organism;
                for (var i = 0; i < amount; i++)
                {
                    organism = new Organism(attribute, this.healthbarTextures);
                    PositionAtRandom(organism);
                    particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(organism).Center));
                }
            }




        }

        /// <summary>
        /// Randomly add herbivore food based on the input from GUI
        /// </summary>
        /// <param name="amount"></param>
        public void AddFoods(int amount)
        {
            Food food;
            for (var i = 0; i < amount; i++)
            {
                food = new Food(Graphics.SimulationTextures["food"], true, Graphics.RANDOM.Next(3, Food.MAX_GRASS_HEALTH));
                PositionAtRandom(food);
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(food).Center));
            }
        }

        /// <summary>
        /// Add an organism based on the mouse position of a user
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddOrganism(int x, int y)
        {
            var positioned = this.grid.AttemptToPositionAt(new Organism(this.bearBreeds[Graphics.RANDOM.Next(0, this.bearBreeds.Count)], this.healthbarTextures), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }

        }

        /// <summary>
        /// Add food based on the mouse position of the user
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddFood(int x, int y)
        {
            var positioned = this.grid.AttemptToPositionAt(new Food(Graphics.SimulationTextures["food"], true, Graphics.RANDOM.Next(1, Food.MAX_GRASS_HEALTH)), x, y);
            if (positioned)
            {
                particleEffects.Add(new ParticleEffect(this.particleTextures, typeof(SpawnParticle), 10, 1000, this.grid.GetTileAt(x, y).Center));
            }
        }

        private void PositionAtRandom(GridItem item)
        {
            if (!this.grid.AttemptToPositionAt(item, Graphics.RANDOM.Next(0, Grid.TILE_COUNT_X), Graphics.RANDOM.Next(0, Grid.TILE_COUNT_Y)))
            {
                PositionAtRandom(item); // Try again
            }
        }
        
        /// <summary>
        /// Handle death by removing the organism from the grid and removing its reference from the list of organisms.
        /// </summary>
        /// <param name="sender">The organism in question.</param>
        /// <param name="e">Event arguments.</param>
        private void SpawnCorpseHandler(object sender, EventArgs e)
        {
            var organism = ((Organism)sender);
            var tile = (Tile)grid.GetTileAt(organism);
            this.grid.AttemptToPositionAt(new Food(Graphics.SimulationTextures["meat"], false, organism.Attributes.MaxHealth), tile.GridIndex.X, tile.GridIndex.Y);

        }

    }
}
