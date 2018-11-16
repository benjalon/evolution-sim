using EvolutionSim.Logic;
using EvolutionSim.UI;
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
        public static int ELAPSED_TIME; 
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

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

            // Load textures
            var textures = new Dictionary<string, Texture2D>();
            textures.Add("face", Content.Load<Texture2D>("face"));
            textures.Add("pizza", Content.Load<Texture2D>("pizza"));
            textures.Add("tile", Content.Load<Texture2D>("tile"));
            textures.Add("mountain", Content.Load<Texture2D>("mountain"));
            textures.Add("grass", Content.Load<Texture2D>("grass"));

            var screenWidth = GraphicsDevice.Viewport.Bounds.Width;
            var screenHeight = GraphicsDevice.Viewport.Bounds.Height;
            this.simulation = new Simulation(textures, screenWidth, screenHeight);

            this.overlay.CreateOrganismButton.OnClick = (Entity btn) => this.simulation.AddOrganism(30);
            this.overlay.CreateFoodButton.OnClick = (Entity btn) => this.simulation.AddFood(30);
            this.overlay.CreateMountainButton.OnClick = (Entity btn) => this.simulation.AddMountain(30);
            this.overlay.CreateWaterButton.OnClick = (Entity btn) => this.simulation.AddWater(30);
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
            ELAPSED_TIME = gameTime.ElapsedGameTime.Milliseconds;
            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                Exit();
            }

            // Update UI elements
            this.overlay.Update(gameTime);
            this.simulation.Update();

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
            this.simulation.Draw(this.spriteBatch);
            this.spriteBatch.End();

            // Draw UI elements on top
            this.overlay.Draw(this.spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
