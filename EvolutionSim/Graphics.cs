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
                    LoadSetupSimulation();
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

            // Load textures
            var textures = new Dictionary<string, Texture2D>
            {
                { "grass_background", Content.Load<Texture2D>("Grass") },
                { "hot_overlay", Content.Load<Texture2D>("Hot") },
                { "cold_overlay", Content.Load<Texture2D>("Cold") },
                { "bear_0", Content.Load<Texture2D>("Species_Obese_Bear_0") },
                { "bear_1", Content.Load<Texture2D>("Species_Obese_Bear_1") },
                { "bear_2", Content.Load<Texture2D>("Species_Obese_Bear_2") },
                { "bear_3", Content.Load<Texture2D>("Species_Obese_Bear_3") },
                { "bear_4", Content.Load<Texture2D>("Species_Obese_Bear_4") },
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
            this.simulation = new Simulation(textures);
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


        private void LoadSetupSimulation()
        {
            UserInterface.Active.UseRenderTarget = true;
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            // Main Panel
            Panel mainPanel = new Panel(new Vector2(WINDOW_WIDTH/2, WINDOW_HEIGHT/1.25f));
       
            Header title = new Header("Create Organism");
            HorizontalLine titleLine = new HorizontalLine();
            // Species Name Controls
            Label input = new Label("Input Species Name:");
            TextInput speciesName = new TextInput();
            
            // Species Appearance Controls
            DropDown textureName = new DropDown();
            textureName.AddItem("bear_0");
            textureName.AddItem("bear_1");
            textureName.AddItem("bear_2");
            textureName.AddItem("bear_3");
            textureName.AddItem("bear_4");
            
            HorizontalLine speciesLine = new HorizontalLine();

            Panel speciesTexturePanel = new Panel(new Vector2(mainPanel.Size.X / 4, mainPanel.Size.Y / 2.5f), anchor: Anchor.CenterLeft, offset: new Vector2(0,mainPanel.Padding.Y*2)) ;
            Panel attributePanel = new Panel(new Vector2(mainPanel.Size.X - speciesTexturePanel.Size.X - mainPanel.Padding.X*2, mainPanel.Size.Y / 2.5f), anchor: Anchor.CenterRight, offset: new Vector2(0, mainPanel.Padding.Y * 2));


            Image textureImage = new Image(Content.Load<Texture2D>("Species_Obese_Bear_0"),drawMode: ImageDrawMode.Stretch);
            speciesTexturePanel.AddChild(textureImage);

            SetupSimulationTextures = new Dictionary<string, Texture2D>
            {
                { "bear_0", Content.Load<Texture2D>("Species_Obese_Bear_0") },
                { "bear_1", Content.Load<Texture2D>("Species_Obese_Bear_1") },
                { "bear_2", Content.Load<Texture2D>("Species_Obese_Bear_2") },
                { "bear_3", Content.Load<Texture2D>("Species_Obese_Bear_3") },
                { "bear_4", Content.Load<Texture2D>("Species_Obese_Bear_4") },

            };
            Paragraph attributeSelection = new Paragraph("Attribute Selection: ");

            // Attributes
            Label labelStartHealth = new Label("Start Health: ");
            TextInput startHealth = new TextInput();
            startHealth.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());

            Label labelStartSpeed = new Label("Start Speed: ");
            TextInput startSpeed = new TextInput();
            startSpeed.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());


            Label labelStartStrength = new Label("Start Strength: ");
            TextInput startStrength = new TextInput();
            startStrength.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());


            // Resist Cold DropDown
            Label labelResistCold = new Label("Resist Cold: ");
            DropDown resistColdChoice = new DropDown();
            resistColdChoice.AddItem("True");
            resistColdChoice.AddItem("False");

            // Resist Heat DropDown
            Label labelResistHeat = new Label("Resist Heat: ");
            DropDown resistHeatChoice = new DropDown();
            resistHeatChoice.AddItem("True");
            resistHeatChoice.AddItem("False");

            // Diet Type DropDown
            Label labelDietType = new Label("Diet Type: ");
            DropDown dietTypeChoice = new DropDown();
            dietTypeChoice.AddItem("Herbivore");
            dietTypeChoice.AddItem("Carnivore");
            dietTypeChoice.AddItem("Omnivore");

            

            Label labelInitialPopulation = new Label("Initial Population: ");
            TextInput initialPopulation = new TextInput();
            initialPopulation.Size = new Vector2(mainPanel.Size.X / 2, mainPanel.Size.Y/15);
            initialPopulation.Anchor = Anchor.AutoCenter;
            initialPopulation.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());


            
            // Adding elements to UI and panel
            UserInterface.Active.AddEntity(mainPanel);
            mainPanel.AddChild(title);
            mainPanel.AddChild(titleLine);
            mainPanel.AddChild(input);
            mainPanel.AddChild(speciesName);
            mainPanel.AddChild(speciesLine);
            mainPanel.AddChild(textureName);
            mainPanel.AddChild(speciesTexturePanel);
            

            // Attributes
            attributePanel.AddChild(attributeSelection);
            attributePanel.AddChild(labelStartHealth);
            attributePanel.AddChild(startHealth);
            attributePanel.AddChild(labelStartSpeed);
            attributePanel.AddChild(startSpeed);
            attributePanel.AddChild(labelStartStrength);
            attributePanel.AddChild(startStrength);
            attributePanel.AddChild(labelResistCold);
            attributePanel.AddChild(resistColdChoice);
            attributePanel.AddChild(labelResistHeat);
            attributePanel.AddChild(resistHeatChoice);
            attributePanel.AddChild(labelDietType);
            attributePanel.AddChild(dietTypeChoice);



            mainPanel.AddChild(attributePanel);
            mainPanel.AddChild(labelInitialPopulation);
            mainPanel.AddChild(initialPopulation);

            Button finished = new Button("Finished!", size: new Vector2(mainPanel.Size.X / 2, mainPanel.Size.Y / 20), anchor: Anchor.AutoCenter);
            finished.OnClick = (Entity btn) =>
            {
                startingArributes = new Attributes();
                startingArributes.Species = speciesName.Value;
                startingArributes.Texture = textureImage.Texture;
                switch (dietTypeChoice.SelectedValue)
                {
                    case "Omnivore":
                        startingArributes.DietType = DietTypes.Omnivore;
                        break;
                    case "Herbivore":
                        startingArributes.DietType = DietTypes.Herbivore;
                        break;
                    case "Carnivore":
                        startingArributes.DietType = DietTypes.Canivore;
                        break;
                }
                startingArributes.MaxHealth = Convert.ToInt32(startHealth.Value);
                startingArributes.Speed = Convert.ToInt32(startSpeed.Value);
                startingArributes.Strength = Convert.ToInt32(startStrength.Value);
                startingArributes.ResistHeat = Convert.ToBoolean(resistHeatChoice.SelectedValue);
                startingArributes.ResistCold = Convert.ToBoolean(resistColdChoice.SelectedValue);
                InitPopulation = Convert.ToInt32(initialPopulation.Value);
                UserInterface.Active.Clear();
                state = Utility.GameState.Running;
                LoadContent();
            };

            mainPanel.AddChild(finished);


            attributePanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;

            textureName.OnValueChange = (Entity entity) =>
            {
                textureImage.Texture = SetupSimulationTextures[textureName.SelectedValue];
            };

            




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
                case Utility.GameState.StartMenu:
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
            UserInterface.Active.Draw(spriteBatch); // Draw UI elements (doesn't affect draw order because it draws to a render target
            
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
            }

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
