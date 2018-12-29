using EvolutionSim.Data;
using EvolutionSim.Sprites;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EvolutionSim.UI
{
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class SetupSimulation
    {
        private Panel mainPanel;
        private Panel speciesTexturePanel;
        private Panel attributePanel;

        private TextInput speciesName;
        private TextInput startHealth;
        private TextInput startSpeed;
        private TextInput startStrength;
        private TextInput startPopulation;

        private DropDown textureName;
        private DropDown resistColdChoice;
        private DropDown resistHeatChoice;
        private DropDown dietTypeChoice;

        private Button finishedButton;

        private Image textureImage;
        private Dictionary<string, Texture2D> SetupSimulationTextures;

        public static Boolean SetupFinished = false;
        private Attributes startingArributes;
        private List<String> textureNameList;

        public SetupSimulation()
        {
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.CursorScale = 0.5f;

            this.CreatePanels();
   

        }



        /// <summary>
        /// Take input from input devices
        /// </summary>
        /// <param name="gameTime">Time elapsed since last update call</param>
        public void Update(GameTime gameTime)
        {


            UserInterface.Active.Update(gameTime);

        }

        /// <summary>
        /// Draw the UI to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch within which this should be drawn</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }

        private void CreatePanels()
        {
            this.mainPanel= new Panel(new Vector2(Graphics.WINDOW_WIDTH / 2, Graphics.WINDOW_HEIGHT / 1.25f));

            HorizontalLine titleLine = new HorizontalLine();
            // ---------------------------------------------------
            // Species Name Controls
            Label input = new Label("Input Species Name:");
            this.speciesName = new TextInput();

            // ---------------------------------------------------
            // Species Appearance Controls
            this.textureName = new DropDown();


            this.textureNameList =  Graphics.SimulationTextures.Keys.Where(name => name.Contains("organism_")).ToList();
            textureNameList.ForEach(item => this.textureName.AddItem(item));


            HorizontalLine speciesLine = new HorizontalLine();

            UserInterface.Active.AddEntity(mainPanel);
            //mainPanel.AddChild(title);
            mainPanel.AddChild(titleLine);
            mainPanel.AddChild(input);
            mainPanel.AddChild(speciesName);
            mainPanel.AddChild(speciesLine);
            mainPanel.AddChild(textureName);

            CreateTexturePanel();
            mainPanel.AddChild(speciesTexturePanel);

            CreateAttributePanel();


            Label labelStartPopulation = new Label("Initial Population: ");
            this.startPopulation = new TextInput();
            this.startPopulation.Size = new Vector2(mainPanel.Size.X / 2, mainPanel.Size.Y / 15);
            this.startPopulation.Anchor = Anchor.AutoCenter;
            this.startPopulation.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());

            mainPanel.AddChild(attributePanel);
            mainPanel.AddChild(labelStartPopulation);
            mainPanel.AddChild(startPopulation);

            this.finishedButton = new Button("Finished!", size: new Vector2(mainPanel.Size.X / 2, mainPanel.Size.Y / 20), anchor: Anchor.AutoCenter);

            SetFinishButton();
            mainPanel.AddChild(this.finishedButton);

        }

        private void SetFinishButton()
        {
            this.finishedButton.OnClick = (Entity btn) =>
            {
                SetupFinished = true;
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
                //InitPopulation = Convert.ToInt32(initialPopulation.Value);
                UserInterface.Active.Clear();
                //state = Utility.GameState.Running;
                //LoadContent();
            };
        }

        private void CreateTexturePanel()
        {
            this.speciesTexturePanel = new Panel(new Vector2(mainPanel.Size.X / 4, mainPanel.Size.Y / 2.5f), anchor: Anchor.CenterLeft, offset: new Vector2(0, mainPanel.Padding.Y * 2));
            //this.textureImage = new Image(Microsoft.Xna.Framework.Content.Conte.Load<Texture2D>("Species_Obese_Bear_0"), drawMode: ImageDrawMode.Stretch);
            textureImage = new Image (Graphics.SimulationTextures[textureNameList.First()]);
            this.speciesTexturePanel.AddChild(textureImage);


            textureName.OnValueChange = (Entity entity) =>
            {
                textureImage.Texture = SetupSimulationTextures[textureName.SelectedValue];
            };
        }
        private void CreateAttributePanel()
        {
            this.attributePanel = new Panel(new Vector2(mainPanel.Size.X - speciesTexturePanel.Size.X - mainPanel.Padding.X * 2, mainPanel.Size.Y / 2.5f), anchor: Anchor.CenterRight, offset: new Vector2(0, mainPanel.Padding.Y * 2));
            Paragraph attributeSelection = new Paragraph("Attribute Selection: ");

            // Attributes
            Label labelStartHealth = new Label("Start Health: ");
            this.startHealth = new TextInput();
            this.startHealth.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());

            Label labelStartSpeed = new Label("Start Speed: ");
            this.startSpeed = new TextInput();
            this.startSpeed.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());


            Label labelStartStrength = new Label("Start Strength: ");
            this.startStrength = new TextInput();
            this.startStrength.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());


            // Resist Cold DropDown
            Label labelResistCold = new Label("Resist Cold: ");
            this.resistColdChoice = new DropDown();
            this.resistColdChoice.AddItem("True");
            this.resistColdChoice.AddItem("False");

            // Resist Heat DropDown
            Label labelResistHeat = new Label("Resist Heat: ");
            this.resistHeatChoice = new DropDown();
            this.resistHeatChoice.AddItem("True");
            this.resistHeatChoice.AddItem("False");

            // Diet Type DropDown
            Label labelDietType = new Label("Diet Type: ");
            this.dietTypeChoice = new DropDown();
            this.dietTypeChoice.AddItem("Herbivore");
            this.dietTypeChoice.AddItem("Carnivore");
            this.dietTypeChoice.AddItem("Omnivore");


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

            attributePanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;


        }
        private void LoadSetupSimulation()
        {
            UserInterface.Active.UseRenderTarget = true;
            //this.spriteBatch = new SpriteBatch(GraphicsDevice);



        }
    }
}
