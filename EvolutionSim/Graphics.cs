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
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;
        public const int SIMULATION_WIDTH = WINDOW_WIDTH - Overlay.PANEL_WIDTH;

        public static Random RANDOM { get; private set; } = new Random();

        private readonly GraphicsDeviceManager graphics;
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
            textures.Add("grass_background", Content.Load<Texture2D>("Grass"));
            textures.Add("hot_overlay", Content.Load<Texture2D>("Hot"));
            textures.Add("cold_overlay", Content.Load<Texture2D>("Cold"));
            textures.Add("bear_0", Content.Load<Texture2D>("Species_Obese_Bear_0"));
            textures.Add("bear_1", Content.Load<Texture2D>("Species_Obese_Bear_1"));
            textures.Add("bear_2", Content.Load<Texture2D>("Species_Obese_Bear_2"));
            textures.Add("bear_3", Content.Load<Texture2D>("Species_Obese_Bear_3"));
            textures.Add("bear_4", Content.Load<Texture2D>("Species_Obese_Bear_4"));
            textures.Add("food", Content.Load<Texture2D>("Food"));
            textures.Add("meat", Content.Load<Texture2D>("Meat"));
            textures.Add("tile", Content.Load<Texture2D>("Tile"));
            textures.Add("mountain", Content.Load<Texture2D>("Mountain"));
            textures.Add("water", Content.Load<Texture2D>("Water"));
            textures.Add("healthbar_green", Content.Load<Texture2D>("Healthbar_Green"));
            textures.Add("healthbar_red", Content.Load<Texture2D>("Healthbar_Red"));

            this.simulation = new Simulation(textures);

            this.overlay = new Overlay(this.simulation);
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
            GraphicsDevice.Clear(Color.LightGreen); // Set background color
            UserInterface.Active.Draw(spriteBatch); // Draw UI elements (doesn't affect draw order because it draws to a render target

            //this.WriteFPS(gameTime);

            // Draw graphical elements
            this.spriteBatch.Begin();
            this.simulation.Draw(this.spriteBatch);
            this.spriteBatch.End();

            // Draw UI elements on top
            UserInterface.Active.DrawMainRenderTarget(spriteBatch);

            base.Draw(gameTime);
        }

        private void WriteFPS(GameTime gameTime)
        {
            this.fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            if (this.fpsOld != this.fps)
            {
                Console.WriteLine(this.fps);
                this.fpsOld = this.fps;
            }
        }
    }
}
