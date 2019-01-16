using EvolutionSim.Data;
using EvolutionSim.Sprites;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.UI
{    
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class Overlay
    {
        private const int MAX_ITEM_INPUT = 100;
        private const int MAX_AGE_INPUT = 300;
        private const float MAX_ATTRIBUTE_INPUT = 1.0f;

        public const int PANEL_WIDTH = 300;

        private const int TOP_PANEL_HEIGHT = 130;
        private const int MIDDLE_PANEL_EXPANDED_HEIGHT = Graphics.WINDOW_HEIGHT - TOP_PANEL_HEIGHT;
        private const int MIDDLE_PANEL_CONTRACTED_HEIGHT = 590;
        private const int BOTTOM_PANEL_OFFSET = 30;
        private const int BOTTOM_PANEL_HEIGHT = Graphics.WINDOW_HEIGHT - TOP_PANEL_HEIGHT - MIDDLE_PANEL_CONTRACTED_HEIGHT + BOTTOM_PANEL_OFFSET;
        private const int TEXT_WIDTH = 110;
        private const int EDIT_TEXT_WIDTH = 180;
        private const int BUTTON_WIDTH = 170;
        private const int ELEMENT_HEIGHT = 40;

        public event EventHandler OrganismsAdded;
        public event EventHandler FoodsAdded;
        public event EventHandler DrawingSettingChanged;
        public event EventHandler TimeSettingChanged;
        public event EventHandler WeatherSettingChanged;

        private Panel topPanel;
        private Panel middlePanel;
        private Panel bottomPanel;

        private TextInput editSpeciesValue;
        private TextInput editHungerValue;
        private TextInput editAgeValue;
        private TextInput editIntelligenceValue;
        private TextInput editStrengthValue;
        private TextInput editSpeedValue;

        private Button saveChanges;


        private Organism selectedOrganism;
        private Paragraph stateDisplay;

        public Overlay()
        {
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.CursorScale = 0.5f;

            // A panel which sits behind the bottom panel to prevent flickering when it's hidden or unhidden
            var backgroundPanel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, Anchor.TopRight);
            backgroundPanel.SetPosition(Anchor.TopRight, new Vector2(0, 0)); // Offset it so it's positioned below the top panel
            backgroundPanel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(backgroundPanel);

            this.CreateTopPanel();
            this.CreateMiddlePanel();
            this.CreateBottomPanel();
        }

        private void CreateTopPanel()
        {
            this.topPanel = new Panel(new Vector2(PANEL_WIDTH, TOP_PANEL_HEIGHT), PanelSkin.None, Anchor.TopRight);
            this.topPanel.Padding = new Vector2(10);
            UserInterface.Active.AddEntity(this.topPanel);

            // Add organism/food through text input

            var addObjectsText = new Paragraph("Randomly Add Objects");

            var organismCountInput = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            organismCountInput.PlaceholderText = "0";

            var organismCreateButton = new Button("Organism", ButtonSkin.Default, Anchor.AutoInline, new Vector2(BUTTON_WIDTH, ELEMENT_HEIGHT));
            organismCreateButton.OnClick = (Entity btn) =>
            {
                if (int.TryParse(organismCountInput.Value, out var input))
                {
                    if (input > MAX_ITEM_INPUT)
                    {
                        input = MAX_ITEM_INPUT;
                    }

                    OrganismsAdded?.Invoke(this, new CreationArgs(input));
                }
            };

            var foodCountInput = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            foodCountInput.PlaceholderText = "0";

            var foodCreateButton = new Button("Food", ButtonSkin.Default, Anchor.AutoInline, new Vector2(BUTTON_WIDTH, ELEMENT_HEIGHT));
            foodCreateButton.OnClick = (Entity btn) =>
            {
                if (int.TryParse(foodCountInput.Value, out var input))
                {
                    if (input > MAX_ITEM_INPUT)
                    {
                        input = MAX_ITEM_INPUT;
                    }

                    FoodsAdded?.Invoke(this, new CreationArgs(input));
                }
            };
            
            this.topPanel.AddChild(addObjectsText);
            this.topPanel.AddChild(organismCountInput);
            this.topPanel.AddChild(organismCreateButton);
            this.topPanel.AddChild(foodCountInput);
            this.topPanel.AddChild(foodCreateButton);
        }

        private void CreateMiddlePanel()
        {
            this.middlePanel = new Panel(new Vector2(PANEL_WIDTH, MIDDLE_PANEL_EXPANDED_HEIGHT), PanelSkin.None, Anchor.TopRight);
            this.middlePanel.Padding = new Vector2(10);
            this.middlePanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
            this.middlePanel.SetPosition(Anchor.TopRight, new Vector2(0, TOP_PANEL_HEIGHT)); // Offset it so it's positioned below the middle panel
            UserInterface.Active.AddEntity(this.middlePanel);

            // Terrain/object drawing

            var addAtCursorText = new Paragraph("Add Objects At Cursor");
            
            var nothingRadio = new RadioButton("None", Anchor.AutoCenter);
            nothingRadio.SpaceBefore = Vector2.Zero;
            nothingRadio.SpaceAfter = Vector2.Zero;
            nothingRadio.OnClick = (Entity btn) => DrawingSettingChanged?.Invoke(this, new DrawingArgs(DrawingSettings.Grass));

            var mountainRadio = new RadioButton("Mountain", Anchor.AutoCenter);
            mountainRadio.SpaceBefore = Vector2.Zero;
            mountainRadio.SpaceAfter = Vector2.Zero;
            mountainRadio.OnClick = (Entity btn) => DrawingSettingChanged?.Invoke(this, new DrawingArgs(DrawingSettings.Mountain));

            var waterRadio = new RadioButton("Water", Anchor.AutoCenter);
            waterRadio.SpaceBefore = Vector2.Zero;
            waterRadio.SpaceAfter = Vector2.Zero;
            waterRadio.OnClick = (Entity btn) => DrawingSettingChanged?.Invoke(this, new DrawingArgs(DrawingSettings.Water));

            var organismRadio = new RadioButton("Organism", Anchor.AutoCenter);
            organismRadio.SpaceBefore = Vector2.Zero;
            organismRadio.SpaceAfter = Vector2.Zero;
            organismRadio.OnClick = (Entity btn) => DrawingSettingChanged?.Invoke(this, new DrawingArgs(DrawingSettings.Organism));

            var foodRadio = new RadioButton("Food", Anchor.AutoCenter);
            foodRadio.SpaceBefore = Vector2.Zero;
            foodRadio.SpaceAfter = Vector2.Zero;
            foodRadio.OnClick = (Entity btn) => DrawingSettingChanged?.Invoke(this, new DrawingArgs(DrawingSettings.Food));

            nothingRadio.Checked = true;

            // Simulation speed manipulation

            var simulationSpeedText = new Paragraph("Set Simulation Speed");

            var normalRadio = new RadioButton("Normal", Anchor.AutoCenter);
            normalRadio.SpaceBefore = Vector2.Zero;
            normalRadio.SpaceAfter = Vector2.Zero;
            normalRadio.OnClick = (Entity btn) => TimeSettingChanged?.Invoke(this, new TimeArgs(TimeSettings.Normal));

            var fastRadio = new RadioButton("Fast", Anchor.AutoCenter);
            fastRadio.SpaceBefore = Vector2.Zero;
            fastRadio.SpaceAfter = Vector2.Zero;
            fastRadio.OnClick = (Entity btn) => TimeSettingChanged?.Invoke(this, new TimeArgs(TimeSettings.Fast));

            var pauseRadio = new RadioButton("Pause", Anchor.AutoCenter);
            pauseRadio.SpaceBefore = Vector2.Zero;
            pauseRadio.SpaceAfter = Vector2.Zero;
            pauseRadio.OnClick = (Entity btn) => TimeSettingChanged?.Invoke(this, new TimeArgs(TimeSettings.Paused));

            normalRadio.Checked = true;

            // Weather manipulation

            var WeatherText = new Paragraph("Set Weather");

            var warmRadio = new RadioButton("Warm", Anchor.AutoCenter);
            warmRadio.SpaceBefore = Vector2.Zero;
            warmRadio.SpaceAfter = Vector2.Zero;
            warmRadio.OnClick = (Entity btn) => WeatherSettingChanged?.Invoke(this, new WeatherArgs(WeatherSettings.Warm));

            var coldRadio = new RadioButton("Cold", Anchor.AutoCenter);
            coldRadio.SpaceBefore = Vector2.Zero;
            coldRadio.SpaceAfter = Vector2.Zero;
            coldRadio.OnClick = (Entity btn) => WeatherSettingChanged?.Invoke(this, new WeatherArgs(WeatherSettings.Cold));

            var hotRadio = new RadioButton("Hot", Anchor.AutoCenter);
            hotRadio.SpaceBefore = Vector2.Zero;
            hotRadio.SpaceAfter = Vector2.Zero;
            hotRadio.OnClick = (Entity btn) => WeatherSettingChanged?.Invoke(this, new WeatherArgs(WeatherSettings.Hot));

            warmRadio.Checked = true;
            
            // Draw order

            this.middlePanel.AddChild(addAtCursorText);
            this.middlePanel.AddChild(nothingRadio);
            this.middlePanel.AddChild(mountainRadio);
            this.middlePanel.AddChild(waterRadio);
            this.middlePanel.AddChild(organismRadio);
            this.middlePanel.AddChild(foodRadio);

            this.middlePanel.AddChild(simulationSpeedText);
            this.middlePanel.AddChild(normalRadio);
            this.middlePanel.AddChild(fastRadio);
            this.middlePanel.AddChild(pauseRadio);

            this.middlePanel.AddChild(WeatherText);
            this.middlePanel.AddChild(warmRadio);
            this.middlePanel.AddChild(coldRadio);
            this.middlePanel.AddChild(hotRadio);
        }

        private void CreateBottomPanel()
        {
            this.bottomPanel = new Panel(new Vector2(PANEL_WIDTH, BOTTOM_PANEL_HEIGHT), PanelSkin.Simple, Anchor.TopRight);
            this.bottomPanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
            this.bottomPanel.SetPosition(Anchor.TopRight, new Vector2(0, TOP_PANEL_HEIGHT + MIDDLE_PANEL_CONTRACTED_HEIGHT - BOTTOM_PANEL_OFFSET)); // Offset it so it's positioned below the middle panel
            this.bottomPanel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(this.bottomPanel);

            // Create elements

            var editAttributesText = new Paragraph("Edit Attributes");

            var editSpeciesText = new Paragraph("Species:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editSpeciesValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);

            var editHungerText = new Paragraph("Hunger:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editHungerValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            this.editHungerValue.OnValueChange = (Entity btn) =>
            {
                var button = (TextInput)btn;
                if (float.TryParse(button.Value, out var input))
                {
                    if (input > MAX_ATTRIBUTE_INPUT)
                    {
                        input = MAX_ATTRIBUTE_INPUT;
                    }

                    this.selectedOrganism.Hunger = input;
                }
            };

            var editAgeText = new Paragraph("Age:", Anchor.AutoInline, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editAgeValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            this.editAgeValue.OnValueChange = (Entity btn) =>
            {
                var button = (TextInput)btn;
                if (int.TryParse(button.Value, out var input))
                {
                    if (input > MAX_AGE_INPUT)
                    {
                        input = MAX_AGE_INPUT;
                    }

                    this.selectedOrganism.Age = input;
                }
            };

            var editStrengthText = new Paragraph("Strength:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editStrengthValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            this.editStrengthValue.OnValueChange = (Entity btn) =>
            {
                var button = (TextInput)btn;
                if (float.TryParse(button.Value, out var input))
                {
                    if (input > MAX_ATTRIBUTE_INPUT)
                    {
                        input = MAX_ATTRIBUTE_INPUT;
                    }

                    var attributes = this.selectedOrganism.Attributes;
                    attributes.Strength = input;
                }
            };

            var editSpeedText = new Paragraph("Speed:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editSpeedValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            this.editSpeedValue.OnValueChange = (Entity btn) =>
            {
                var button = (TextInput)btn;
                if (float.TryParse(button.Value, out var input))
                {
                    if (input > MAX_ATTRIBUTE_INPUT)
                    {
                        input = MAX_ATTRIBUTE_INPUT;
                    }

                    var attributes = this.selectedOrganism.Attributes;
                    attributes.Speed = input;
                }
            };

            var editIntelligenceText = new Paragraph("Intel:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editIntelligenceValue = new TextInput(false, new Vector2(EDIT_TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            this.editIntelligenceValue.OnValueChange = (Entity btn) =>
            {
                var button = (TextInput)btn;
                if (float.TryParse(button.Value, out var input))
                {
                    if (input > MAX_ATTRIBUTE_INPUT)
                    {
                        input = MAX_ATTRIBUTE_INPUT;
                    }

                    var attributes = this.selectedOrganism.Attributes;
                    attributes.Intelligence = input;
                }
            };

             this.stateDisplay = new Paragraph("", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));

            this.saveChanges = new Button("Save Changes", anchor: Anchor.AutoCenter);
            this.saveChanges.OnClick = (Entity btn) =>
            {
                try { 
                Attributes attrib = new Attributes()
                {
                   
                    Species = this.editSpeciesValue.Value,
                    Intelligence = (float)Convert.ToDouble(this.editIntelligenceValue.Value),
                    MaxHealth = this.selectedOrganism.Attributes.MaxHealth,
                    ResistCold = this.selectedOrganism.Attributes.ResistCold,
                    ResistHeat = this.selectedOrganism.Attributes.ResistHeat,
                    Speed = (float)Convert.ToDouble(this.editSpeedValue.Value),
                    Strength = (float)Convert.ToDouble(this.editStrengthValue.Value),
                    Texture = this.selectedOrganism.Texture,
                    DietType = this.selectedOrganism.Attributes.DietType
                };
                this.selectedOrganism.Attributes = attrib;
            }
            catch (Exception e)
            {

            }
                finally
                {
                    
                    RefreshOrganismStats();
                }

            };
   


            // Draw order

            this.bottomPanel.AddChild(editAttributesText);
            this.bottomPanel.AddChild(editSpeciesText);
            this.bottomPanel.AddChild(this.editSpeciesValue);
            this.bottomPanel.AddChild(editHungerText);
            this.bottomPanel.AddChild(this.editHungerValue);
            this.bottomPanel.AddChild(editAgeText);
            this.bottomPanel.AddChild(this.editAgeValue);
            this.bottomPanel.AddChild(editStrengthText);
            this.bottomPanel.AddChild(this.editStrengthValue);
            this.bottomPanel.AddChild(editSpeedText);
            this.bottomPanel.AddChild(this.editSpeedValue);
            this.bottomPanel.AddChild(editIntelligenceText);
            this.bottomPanel.AddChild(this.editIntelligenceValue);
            this.bottomPanel.AddChild(stateDisplay);
            this.bottomPanel.AddChild(saveChanges);


        }


        private void RefreshOrganismStats()
        {


            this.editSpeciesValue.Value = "";
            this.editHungerValue.Value = "";
            this.editAgeValue.Value = "";
            this.editStrengthValue.Value = "";
            this.editSpeedValue.Value = "";
            this.editIntelligenceValue.Value = "";
            this.stateDisplay.Text = "State: " + selectedOrganism.State.ToString();



            this.middlePanel.Size = new Vector2(this.middlePanel.Size.X, MIDDLE_PANEL_CONTRACTED_HEIGHT);
            this.bottomPanel.Visible = true;
        }
        /// <summary>
        /// Take input from input devices
        /// </summary>
        /// <param name="gameTime">Time elapsed since last update call</param>
        public void Update(GameTime gameTime, Organism uiSelectedOrganism)
        {

            UserInterface.Active.Update(gameTime);
            
            if (uiSelectedOrganism == null)
            {
                this.selectedOrganism = null;
                this.middlePanel.Size = new Vector2(this.middlePanel.Size.X, MIDDLE_PANEL_EXPANDED_HEIGHT);
                this.bottomPanel.Visible = false;
            }
            else 
            {
                this.selectedOrganism = uiSelectedOrganism;

                this.editSpeciesValue.PlaceholderText = selectedOrganism.Attributes.Species;
                this.editHungerValue.PlaceholderText = Math.Round(selectedOrganism.Hunger, 2).ToString();
                this.editAgeValue.PlaceholderText = selectedOrganism.Age.ToString();
                this.editStrengthValue.PlaceholderText = Math.Round(selectedOrganism.Attributes.Strength, 2).ToString();
                this.editSpeedValue.PlaceholderText = Math.Round(selectedOrganism.Attributes.Speed, 2).ToString();
                this.editIntelligenceValue.PlaceholderText = Math.Round(selectedOrganism.Attributes.Intelligence, 2).ToString();
                this.stateDisplay.Text = "State: " + selectedOrganism.State.ToString();

               

                this.middlePanel.Size = new Vector2(this.middlePanel.Size.X, MIDDLE_PANEL_CONTRACTED_HEIGHT);
                this.bottomPanel.Visible = true;
            }
        }

        /// <summary>
        /// Draw the UI to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch within which this should be drawn</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
