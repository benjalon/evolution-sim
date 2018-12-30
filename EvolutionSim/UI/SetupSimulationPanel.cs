using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.Data;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.UI
{
    public class SetupSimulationPanel : Panel
    {
        private Panel speciesTexturePanel;
        private Panel attributePanel;

        public TextInput speciesName;
        private TextInput startHealth;
        private TextInput startSpeed;
        private TextInput startStrength;
        private TextInput startPopulation;

        private DropDown textureName;
        private DropDown resistColdChoice;
        private DropDown resistHeatChoice;
        private DropDown dietTypeChoice;

        private Image textureImage;


        public int InitPopulation;


        private List<String> textureNameList;

        public SetupSimulationPanel(float tabButtonSIze)
        {

            //base.Size = 
            this.Size = (new Vector2(Graphics.WINDOW_WIDTH/2.0f, (Graphics.WINDOW_HEIGHT / 1.25f)- tabButtonSIze));

            CreatePanel();
            
        }

        public Attributes GetPanelData()
        {
            Attributes startingAttributes;
            startingAttributes = new Attributes();
            startingAttributes.Species = speciesName.Value;
            startingAttributes.Texture = textureImage.Texture;
            switch (dietTypeChoice.SelectedValue)
            {
                case "Omnivore":
                    startingAttributes.DietType = DietTypes.Omnivore;
                    break;
                case "Herbivore":
                    startingAttributes.DietType = DietTypes.Herbivore;
                    break;
                case "Carnivore":
                    startingAttributes.DietType = DietTypes.Canivore;
                    break;
            }
            startingAttributes.MaxHealth = Convert.ToInt32(startHealth.Value);
            startingAttributes.Speed = Convert.ToInt32(startSpeed.Value);
            startingAttributes.Strength = Convert.ToInt32(startStrength.Value);
            startingAttributes.ResistHeat = Convert.ToBoolean(resistHeatChoice.SelectedValue);
            startingAttributes.ResistCold = Convert.ToBoolean(resistColdChoice.SelectedValue);
           // this.InitPopulation = Convert.ToInt32(startPopulation.Value);

            return startingAttributes;
        }

        private void CreatePanel()
        {
            Header header = new Header("Create Organism");
            HorizontalLine titleLine = new HorizontalLine();
            // ---------------------------------------------------
            // Species Name Controls
            Label input = new Label("Input Species Name:");
            this.speciesName = new TextInput();


            // ---------------------------------------------------
            // Species Appearance Controls
            this.textureName = new DropDown();


            this.textureNameList = Graphics.SimulationTextures.Keys.Where(name => name.Contains("organism_")).ToList();
            textureNameList.ForEach(item => this.textureName.AddItem(item));


            HorizontalLine speciesLine = new HorizontalLine();

            this.AddChild(header);
            this.AddChild(titleLine);
            this.AddChild(input);
            this.AddChild(speciesName);
            this.AddChild(speciesLine);
            this.AddChild(textureName);

            CreateTexturePanel();
            this.AddChild(speciesTexturePanel);

            CreateAttributePanel();


            Label labelStartPopulation = new Label("Initial Population: ");
            this.startPopulation = new TextInput();
            this.startPopulation.Size = new Vector2(this.Size.X / 2, this.Size.Y / 15);
            this.startPopulation.Anchor = Anchor.AutoCenter;
            this.startPopulation.Validators.Add(new GeonBit.UI.Entities.TextValidators.TextValidatorNumbersOnly());

            this.AddChild(attributePanel);
            this.AddChild(labelStartPopulation);
            this.AddChild(startPopulation);
        }


        private void CreateTexturePanel()
        {
            this.speciesTexturePanel = new Panel(new Vector2(this.Size.X / 4, this.Size.Y / 2.5f), anchor: Anchor.CenterLeft, offset: new Vector2(0, this.Padding.Y * 2));
            //this.textureImage = new Image(Microsoft.Xna.Framework.Content.Conte.Load<Texture2D>("Species_Obese_Bear_0"), drawMode: ImageDrawMode.Stretch);
            textureImage = new Image(Graphics.SimulationTextures[textureNameList.First()]);
            this.speciesTexturePanel.AddChild(textureImage);


            textureName.OnValueChange = (Entity entity) =>
            {
                textureImage.Texture = Graphics.SimulationTextures[textureName.SelectedValue];
            };
        }
        private void CreateAttributePanel()
        {
            this.attributePanel = new Panel(new Vector2(this.Size.X - speciesTexturePanel.Size.X - this.Padding.X * 2, this.Size.Y / 2.5f), anchor: Anchor.CenterRight, offset: new Vector2(0, this.Padding.Y * 2));
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
    }
}
