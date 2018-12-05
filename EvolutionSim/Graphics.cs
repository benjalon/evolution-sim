using EvolutionSim.Logic;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.UI;
using EvolutionSim.Utility;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Graphics : Game
    {
        public static int WINDOW_WIDTH = 1920;
        public static int WINDOW_HEIGHT = 1080;
        public static Random Random { get; private set; } = new Random();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Background background;
        private Overlay overlay;
        private Simulation simulation;

        private double fps = 0;
        private double fpsOld = 0;

        public Graphics()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            this.graphics.ApplyChanges();
        }

        /// <summary>
        /// Initial setup.
        /// </summary>
        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            this.overlay = new Overlay();
            
            base.Initialize();
        }

        /// <summary>
        /// Asset loading (textures, sounds etc.)
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            var screenWidth = GraphicsDevice.Viewport.Bounds.Width;
            var screenHeight = GraphicsDevice.Viewport.Bounds.Height;

            this.background = new Background(Content.Load<Texture2D>("grass"), screenWidth, screenHeight);

            // Load textures
            var textures = new Dictionary<string, Texture2D>();
            textures.Add("bear_0", Content.Load<Texture2D>("Species_Obese_Bear_0"));
            textures.Add("bear_1", Content.Load<Texture2D>("Species_Obese_Bear_1"));
            textures.Add("bear_2", Content.Load<Texture2D>("Species_Obese_Bear_2"));
            textures.Add("bear_3", Content.Load<Texture2D>("Species_Obese_Bear_3"));
            textures.Add("bear_4", Content.Load<Texture2D>("Species_Obese_Bear_4"));
            textures.Add("food", Content.Load<Texture2D>("food"));
            textures.Add("tile", Content.Load<Texture2D>("tile"));
            textures.Add("mountain", Content.Load<Texture2D>("mountain"));
            textures.Add("water", Content.Load<Texture2D>("water"));

            this.simulation = new Simulation(textures, screenWidth, screenHeight);

            this.CreateUIHandlers();
        }
        
        /// <summary>
        /// Asset unloading
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Update the application
        /// </summary>
        /// <param name="gameTime">Delta - time since last update call</param>
        protected override void Update(GameTime gameTime)
        {
            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                Exit();
            }

            // Update UI elements
            this.simulation.Update(gameTime);
            this.overlay.Update(gameTime, this.simulation.TileHighlight);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw graphical elements to screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            this.fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            if (this.fpsOld != this.fps)
            {
                Console.WriteLine(this.fps);
                this.fpsOld = this.fps;
            }

            GraphicsDevice.Clear(Color.LightGreen); // Set background color

            // Draw graphical elements
            this.spriteBatch.Begin();
            this.background.Draw(this.spriteBatch);
            this.simulation.Draw(this.spriteBatch);
            this.spriteBatch.End();

            // Draw UI elements on top
            this.overlay.Draw(this.spriteBatch);
            
            base.Draw(gameTime);
        }

        private void CreateUIHandlers()
        {
            this.overlay.OrganismCreateButton.OnClick = (Entity btn) =>
            {
                int input;
                if (int.TryParse(this.overlay.OrganismCountInput.Value, out input))
                {
                    this.simulation.AddOrganisms(input);
                }
            };

            this.overlay.FoodCreateButton.OnClick = (Entity btn) =>
            {
                int input;
                if (int.TryParse(this.overlay.FoodCountInput.Value, out input))
                {
                    this.simulation.AddFoods(input);
                }
            };

            this.overlay.NothingRadio.OnClick = (Entity btn) => this.simulation.SelectedRadioItem = RadioItems.Grass;
            this.overlay.MountainRadio.OnClick = (Entity btn) => this.simulation.SelectedRadioItem = RadioItems.Mountain;
            this.overlay.WaterRadio.OnClick = (Entity btn) => this.simulation.SelectedRadioItem = RadioItems.Water;
            this.overlay.OrganismRadio.OnClick = (Entity btn) => this.simulation.SelectedRadioItem = RadioItems.Organism;
            this.overlay.FoodRadio.OnClick = (Entity btn) => this.simulation.SelectedRadioItem = RadioItems.Food;

            this.overlay.NormalButton.OnClick = (Entity btn) => this.simulation.TimeManager.SetSpeed(1);
            this.overlay.FastButton.OnClick = (Entity btn) => this.simulation.TimeManager.SetSpeed(3);
            this.overlay.PauseButton.OnClick = (Entity btn) => this.simulation.TimeManager.Paused = true;

            this.overlay.OrganismHungerValue.OnValueChange = (Entity btn) =>
            {
                int input;
                if (int.TryParse(((TextInput)btn).Value, out input))
                {
                    this.simulation.TileHighlight.SelectedOrganism.attributes.Hunger = input;
                }
            };
        }
    }
}
