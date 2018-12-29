using EvolutionSim.Data;
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
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;
        public const int SIMULATION_WIDTH = WINDOW_WIDTH - Overlay.PANEL_WIDTH;

        public static Utility.GameState state = Utility.GameState.StartMenu;

        public static Random RANDOM { get; private set; } = new Random();

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private Overlay overlay;
        private Simulation simulation;
        private SetupSimulation setupSimulation;

        public static Dictionary<String, Texture2D> SimulationTextures;

        private double fps = 0;
        private double fpsOld = 0;

        // Need to figure out a way around this
        private Dictionary<String, Texture2D> SetupSimulationTextures;
        private Attributes startingArributes;
        private int InitPopulation = 0;



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

            // Load textures
            SimulationTextures = new Dictionary<string, Texture2D>
            {
                { "grass_background", Content.Load<Texture2D>("Grass") },
                { "hot_overlay", Content.Load<Texture2D>("Hot") },
                { "cold_overlay", Content.Load<Texture2D>("Cold") },
                { "organism_0", Content.Load<Texture2D>("Species_Obese_Bear_0") },
                { "organism_1", Content.Load<Texture2D>("Species_Obese_Bear_1") },
                { "organism_2", Content.Load<Texture2D>("Species_Obese_Bear_2") },
                { "organism_3", Content.Load<Texture2D>("Species_Obese_Bear_3") },
                { "organism_4", Content.Load<Texture2D>("Species_Obese_Bear_4") },
                { "food", Content.Load<Texture2D>("Food") },
                { "meat", Content.Load<Texture2D>("Meat") },
                { "tile", Content.Load<Texture2D>("Tile") },
                { "mountain", Content.Load<Texture2D>("Mountain") },
                { "water", Content.Load<Texture2D>("Water") },
                { "healthbar_green", Content.Load<Texture2D>("Healthbar_Green") },
                { "healthbar_red", Content.Load<Texture2D>("Healthbar_Red") },
                { "circle", Content.Load<Texture2D>("Circle") },
                { "star", Content.Load<Texture2D>("Star") },
                { "diamond", Content.Load<Texture2D>("Diamond") }
            };
        }

        /// <summary>
        /// Asset loading (textures, sounds etc.)
        /// </summary>
        protected override void LoadContent()
        {
             switch (state)
            {
                case Utility.GameState.StartMenu:
                    LoadStartMenu();
                    break;
                case Utility.GameState.Running:
                    LoadSimulation();
                    break;
                case Utility.GameState.Setup:
                     this.setupSimulation = new SetupSimulation();
                    break;
                case Utility.GameState.Exit:
                    Exit();
                    break;
            }

        }

        private void LoadStartMenu()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            Paragraph header = new Header("Welcome to Evolution-Sim", Anchor.TopCenter, new Vector2(0, WINDOW_HEIGHT / 8));
            header.FillColor = Color.White;
            header.Scale = 5.0f;

            HorizontalLine horizontalLine = new HorizontalLine(Anchor.TopCenter, new Vector2(0, WINDOW_HEIGHT / 4));
            horizontalLine.Padding = new Vector2(10, 10);

            Button sandboxMode = new Button("Sandbox Mode", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(WINDOW_WIDTH / 6, 50), new Vector2(0, WINDOW_HEIGHT / 8));
            Button simulationSetup = new Button("Setup Simulation", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(WINDOW_WIDTH / 6, 50));
            Button exit = new Button("Exit", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(WINDOW_WIDTH / 6, 50));
            sandboxMode.OnClick = (Entity btn) => {
                state = Utility.GameState.Running;
                UserInterface.Active.Clear();
                LoadContent();
            };
            simulationSetup.OnClick = (Entity btn) => {
                state = Utility.GameState.Setup;
                UserInterface.Active.Clear();
                LoadContent();
            };
            exit.OnClick = (Entity btn) => {
                state = Utility.GameState.Exit;
                LoadContent();
            };


            UserInterface.Active.AddEntity(header);
            UserInterface.Active.AddEntity(horizontalLine);
            UserInterface.Active.AddEntity(sandboxMode);
            UserInterface.Active.AddEntity(simulationSetup);
            UserInterface.Active.AddEntity(exit);
        }
        private void LoadSimulation()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

           
            this.simulation = new Simulation(SimulationTextures);
            //var organismCreateButton = new Button("Organism", ButtonSkin.Default, Anchor.AutoInline, new Vector2(BUTTON_WIDTH, ELEMENT_HEIGHT));

            // Move this?
            this.overlay = new Overlay();
            this.overlay.OrganismsAdded += OrganismsAddedHandler;
            this.overlay.FoodsAdded += FoodsAddedHandler;
            this.overlay.DrawingSettingChanged += DrawingSettingChangedHandler;
            this.overlay.TimeSettingChanged += TimeSettingChangedHandler;
            this.overlay.WeatherSettingChanged += WeatherSettingChangedHandler;
            this.simulation.AddOrganisms(startingArributes, InitPopulation);
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

            switch (state)
            {

                case Utility.GameState.Running:
                    this.simulation.Update(gameTime);
                    this.overlay.Update(gameTime, simulation.GridInteractionManager.SelectedOrganism);
                    break;
                case Utility.GameState.Setup:
                    if (SetupSimulation.SetupFinished)
                    {
                        state = Utility.GameState.Running;
                        UserInterface.Active.Clear();

                    }
                    else
                    {
                        this.setupSimulation.Update(gameTime);
                    }
                    break;


            }
            UserInterface.Active.Update(gameTime);

            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                state = Utility.GameState.Exit;
                Exit();
            }

          

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw graphical elements to screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen); // Set background color
            
            //this.WriteFPS(gameTime);

            // Draw graphical elements
            this.spriteBatch.Begin();
            switch (state)
            {
                case Utility.GameState.Running:
                    this.simulation.Draw(this.spriteBatch);
                    break;
                case Utility.GameState.StartMenu:
                    break;
                case Utility.GameState.Setup:
                    //this.setupSimulation.Draw(this.spriteBatch);
                    break;
            }

            this.spriteBatch.End();
            UserInterface.Active.Draw(spriteBatch); // Draw UI elements (doesn't affect draw order because it draws to a render target


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

        private void OrganismsAddedHandler(object sender, EventArgs e)
        {
            this.simulation.AddOrganisms(((CreationArgs)e).Count);
        }

        private void FoodsAddedHandler(object sender, EventArgs e)
        {
            this.simulation.AddFoods(((CreationArgs)e).Count);
        }

        private void DrawingSettingChangedHandler(object sender, EventArgs e)
        {
            this.simulation.GridInteractionManager.DrawingSetting = ((DrawingArgs)e).DrawingSetting;
        }

        private void TimeSettingChangedHandler(object sender, EventArgs e)
        {
            this.simulation.TimeManager.TimeSetting = ((TimeArgs)e).TimeSetting;
        }

        private void WeatherSettingChangedHandler(object sender, EventArgs e)
        {
            this.simulation.WeatherOverlay.WeatherSetting = ((WeatherArgs)e).weatherSetting;
        }
    }
}
