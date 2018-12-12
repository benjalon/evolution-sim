using EvolutionSim.Logic;
using EvolutionSim.TileGrid.GridItems;
using EvolutionSim.Utility;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.UI
{
    public enum RadioAddSprites
    {
        Grass,
        Mountain,
        Water,
        Organism,
        Food
    }
    
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class Overlay
    {
        public const int PANEL_WIDTH = 300;

        private const int TOP_PANEL_HEIGHT = 130;
        private const int MIDDLE_PANEL_EXPANDED_HEIGHT = Graphics.WINDOW_HEIGHT - TOP_PANEL_HEIGHT;
        private const int MIDDLE_PANEL_CONTRACTED_HEIGHT = 590;
        private const int BOTTOM_PANEL_OFFSET = 30;
        private const int BOTTOM_PANEL_HEIGHT = Graphics.WINDOW_HEIGHT - TOP_PANEL_HEIGHT - MIDDLE_PANEL_CONTRACTED_HEIGHT + BOTTOM_PANEL_OFFSET;
        private const int TEXT_WIDTH = 110;
        private const int BUTTON_WIDTH = 170;
        private const int ELEMENT_HEIGHT = 40;

        private Panel topPanel;
        private Panel middlePanel;
        private Panel bottomPanel;

        private TextInput editSpeciesValue;
        private TextInput editHungerValue;
        private TextInput editAgeValue;
        private TextInput editStrengthValue;
        private TextInput editSpeedValue;

        public Overlay(Simulation simulation)
        {
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.CursorScale = 0.5f;

            // A panel which sits behind the bottom panel to prevent flickering when it's hidden or unhidden
            var backgroundPanel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, Anchor.TopRight);
            backgroundPanel.SetPosition(Anchor.TopRight, new Vector2(0, 0)); // Offset it so it's positioned below the top panel
            backgroundPanel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(backgroundPanel);

            this.CreateTopPanel(simulation);
            this.CreateMiddlePanel(simulation);
            this.CreateBottomPanel(simulation);
        }

        private void CreateTopPanel(Simulation simulation)
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
                    simulation.AddOrganisms(input);
                }
            };

            var foodCountInput = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            foodCountInput.PlaceholderText = "0";

            var foodCreateButton = new Button("Food", ButtonSkin.Default, Anchor.AutoInline, new Vector2(BUTTON_WIDTH, ELEMENT_HEIGHT));
            foodCreateButton.OnClick = (Entity btn) =>
            {
                if (int.TryParse(foodCountInput.Value, out var input))
                {
                    simulation.AddFoods(input);
                }
            };
            
            this.topPanel.AddChild(addObjectsText);
            this.topPanel.AddChild(organismCountInput);
            this.topPanel.AddChild(organismCreateButton);
            this.topPanel.AddChild(foodCountInput);
            this.topPanel.AddChild(foodCreateButton);
        }

        private void CreateMiddlePanel(Simulation simulation)
        {
            this.middlePanel = new Panel(new Vector2(PANEL_WIDTH, MIDDLE_PANEL_EXPANDED_HEIGHT), PanelSkin.None, Anchor.TopRight);
            this.middlePanel.Padding = new Vector2(10);
            this.middlePanel.PanelOverflowBehavior = PanelOverflowBehavior.Clipped;
            this.middlePanel.SetPosition(Anchor.TopRight, new Vector2(0, TOP_PANEL_HEIGHT)); // Offset it so it's positioned below the middle panel
            UserInterface.Active.AddEntity(this.middlePanel);

            // Terrain/object drawing

            var addAtCursorText = new Paragraph("Add Objects At Cursor");

            var nothingRadio = new RadioButton("None", Anchor.AutoCenter);
            nothingRadio.SpaceBefore = Vector2.Zero;
            nothingRadio.SpaceAfter = Vector2.Zero;
            nothingRadio.OnClick = (Entity btn) => simulation.SelectedRadioItem = RadioAddSprites.Grass;

            var mountainRadio = new RadioButton("Mountain", Anchor.AutoCenter);
            mountainRadio.SpaceBefore = Vector2.Zero;
            mountainRadio.SpaceAfter = Vector2.Zero;
            mountainRadio.OnClick = (Entity btn) => simulation.SelectedRadioItem = RadioAddSprites.Mountain;

            var waterRadio = new RadioButton("Water", Anchor.AutoCenter);
            waterRadio.SpaceBefore = Vector2.Zero;
            waterRadio.SpaceAfter = Vector2.Zero;
            waterRadio.OnClick = (Entity btn) => simulation.SelectedRadioItem = RadioAddSprites.Water;

            var organismRadio = new RadioButton("Organism", Anchor.AutoCenter);
            organismRadio.SpaceBefore = Vector2.Zero;
            organismRadio.SpaceAfter = Vector2.Zero;
            organismRadio.OnClick = (Entity btn) => simulation.SelectedRadioItem = RadioAddSprites.Organism;

            var foodRadio = new RadioButton("Food", Anchor.AutoCenter);
            foodRadio.SpaceBefore = Vector2.Zero;
            foodRadio.SpaceAfter = Vector2.Zero;
            foodRadio.OnClick = (Entity btn) => simulation.SelectedRadioItem = RadioAddSprites.Food;

            nothingRadio.Checked = true;

            // Simulation speed manipulation

            var simulationSpeedText = new Paragraph("Set Simulation Speed");

            var normalRadio = new RadioButton("Normal", Anchor.AutoCenter);
            normalRadio.SpaceBefore = Vector2.Zero;
            normalRadio.SpaceAfter = Vector2.Zero;
            normalRadio.OnClick = (Entity btn) => simulation.TimeManager.SetSpeed(1);

            var fastRadio = new RadioButton("Fast", Anchor.AutoCenter);
            fastRadio.SpaceBefore = Vector2.Zero;
            fastRadio.SpaceAfter = Vector2.Zero;
            fastRadio.OnClick = (Entity btn) => simulation.TimeManager.SetSpeed(4);

            var pauseRadio = new RadioButton("Pause", Anchor.AutoCenter);
            pauseRadio.SpaceBefore = Vector2.Zero;
            pauseRadio.SpaceAfter = Vector2.Zero;
            pauseRadio.OnClick = (Entity btn) => simulation.TimeManager.Paused = true;

            normalRadio.Checked = true;

            // Weather manipulation

            var WeatherText = new Paragraph("Set Weather");

            var warmRadio = new RadioButton("Warm", Anchor.AutoCenter);
            warmRadio.SpaceBefore = Vector2.Zero;
            warmRadio.SpaceAfter = Vector2.Zero;
            warmRadio.OnClick = (Entity btn) => simulation.WeatherManager.SetWeather(WeatherSettings.Warm);

            var coldRadio = new RadioButton("Cold", Anchor.AutoCenter);
            coldRadio.SpaceBefore = Vector2.Zero;
            coldRadio.SpaceAfter = Vector2.Zero;
            coldRadio.OnClick = (Entity btn) => simulation.WeatherManager.SetWeather(WeatherSettings.Cold);

            var hotRadio = new RadioButton("Hot", Anchor.AutoCenter);
            hotRadio.SpaceBefore = Vector2.Zero;
            hotRadio.SpaceAfter = Vector2.Zero;
            hotRadio.OnClick = (Entity btn) => simulation.WeatherManager.SetWeather(WeatherSettings.Hot);

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

        private void CreateBottomPanel(Simulation simulation)
        {
            this.bottomPanel = new Panel(new Vector2(PANEL_WIDTH, BOTTOM_PANEL_HEIGHT), PanelSkin.Simple, Anchor.TopRight);
            this.bottomPanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
            this.bottomPanel.SetPosition(Anchor.TopRight, new Vector2(0, TOP_PANEL_HEIGHT + MIDDLE_PANEL_CONTRACTED_HEIGHT - BOTTOM_PANEL_OFFSET)); // Offset it so it's positioned below the middle panel
            this.bottomPanel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(this.bottomPanel);

            // Create elements

            var editAttributesText = new Paragraph("Edit Attributes");

            var editSpeciesText = new Paragraph("Species:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editSpeciesValue = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);

            var editHungerText = new Paragraph("Hunger:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editHungerValue = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);
            editHungerValue.OnValueChange = (Entity btn) =>
            {
                if (int.TryParse(((TextInput)btn).Value, out var input))
                {
                    simulation.TileHighlight.SelectedOrganism.Attributes.Hunger = input;
                }
            };

            var editAgeText = new Paragraph("Age:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editAgeValue = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);

            var editStrengthText = new Paragraph("Strength:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editStrengthValue = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);

            var editSpeedText = new Paragraph("Speed:", Anchor.AutoInline, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT));
            this.editSpeedValue = new TextInput(false, new Vector2(TEXT_WIDTH, ELEMENT_HEIGHT), Anchor.AutoInline, null, PanelSkin.Fancy);

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
        }

        /// <summary>
        /// Take input from input devices
        /// </summary>
        /// <param name="gameTime">Time elapsed since last update call</param>
        public void Update(GameTime gameTime, DrawingManager tileHighlight)
        {
            UserInterface.Active.Update(gameTime);
            
            if (tileHighlight.SelectedOrganism == null)
            {
                this.editHungerValue.Value = "";
                this.middlePanel.Size = new Vector2(this.middlePanel.Size.X, MIDDLE_PANEL_EXPANDED_HEIGHT);
                this.middlePanel.PanelOverflowBehavior = PanelOverflowBehavior.Clipped;
                this.bottomPanel.Visible = false;
            }
            else
            {
                this.editSpeciesValue.PlaceholderText = tileHighlight.SelectedOrganism.Attributes.Species;
                this.editHungerValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Hunger, 2).ToString();
                this.editAgeValue.PlaceholderText = tileHighlight.SelectedOrganism.Attributes.Age.ToString();
                this.editStrengthValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Strength, 2).ToString();
                this.editSpeedValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Speed, 2).ToString();

                this.middlePanel.Size = new Vector2(this.middlePanel.Size.X, MIDDLE_PANEL_CONTRACTED_HEIGHT);
                this.middlePanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
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
