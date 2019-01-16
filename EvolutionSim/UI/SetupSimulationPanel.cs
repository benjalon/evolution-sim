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
        private float MAX_ATTRIBUTE_INPUT = 1.0f;

        private Panel speciesTexturePanel;
        private Panel attributePanel;

        public TextInput speciesName;
        private TextInput startHealth;
        private TextInput startSpeed;
        private TextInput startStrength;
        private TextInput startIntellgence;
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

            this.Size = (new Vector2(Graphics.WINDOW_WIDTH / 2.0f, (Graphics.WINDOW_HEIGHT / 1.25f) - tabButtonSIze));

            CreatePanel();

        }

        public Nullable<Attributes> GetPanelData()
        {
            try
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

                if (float.TryParse(startSpeed.Value, out var speedInput))
                {
                    if (speedInput > MAX_ATTRIBUTE_INPUT)
                    {
                        speedInput = MAX_ATTRIBUTE_INPUT;
                    }

                    startingAttributes.Speed = speedInput;
                }

                if (float.TryParse(startStrength.Value, out var strengthInput))
                {
                    if (strengthInput > MAX_ATTRIBUTE_INPUT)
                    {
                        strengthInput = MAX_ATTRIBUTE_INPUT;
                    }

                    startingAttributes.Strength = strengthInput;
                }

                if (float.TryParse(startIntellgence.Value, out var intelligenceInput))
                {
                    if (intelligenceInput > MAX_ATTRIBUTE_INPUT)
                    {
                        intelligenceInput = MAX_ATTRIBUTE_INPUT;
                    }

                    startingAttributes.Intelligence = intelligenceInput;
                }

                startingAttributes.ResistHeat = Convert.ToBoolean(resistHeatChoice.SelectedValue);
                startingAttributes.ResistCold = Convert.ToBoolean(resistColdChoice.SelectedValue);
                return startingAttributes;
            }
            catch (Exception e)
            {
                Console.WriteLine("really?!?");
                return null;

            }



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



            this.AddChild(attributePanel);
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

            Label labelStartSpeed = new Label("Starting Speed 0 - 1.0: ");
            this.startSpeed = new TextInput();


            Label labelStartStrength = new Label("Starting Strength 0 - 1.0: ");
            this.startStrength = new TextInput();

            Label labelStartIntelligence = new Label("Starting Intelligence 0 - 1.0: ");
            this.startIntellgence = new TextInput();


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
            attributePanel.AddChild(labelStartIntelligence);
            attributePanel.AddChild(startIntellgence);
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
