using EvolutionSim.TileGrid.GridItems;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim.UI
{
    public enum RadioItems
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
        public static int PANEL_WIDTH { get; } = 300;

        public TextInput OrganismCountInput { get; private set; }
        public Button OrganismCreateButton { get; private set; }
        public TextInput FoodCountInput { get; private set; }
        public Button FoodCreateButton { get; private set; }

        public RadioButton NothingRadio { get; private set; }
        public RadioButton MountainRadio { get; private set; }
        public RadioButton WaterRadio { get; private set; }
        public RadioButton OrganismRadio { get; private set; }
        public RadioButton FoodRadio { get; private set; }

        public RadioButton PauseRadio { get; private set; }
        public RadioButton NormalRadio { get; private set; }
        public RadioButton FastRadio { get; private set; }

        private Paragraph editAttributesText;
        private Paragraph organismSpeciesText;
        public TextInput OrganismSpeciesValue { get; private set; }
        private Paragraph organismHungerText;
        public TextInput OrganismHungerValue { get; private set; }
        private Paragraph organismAgeText;
        public TextInput OrganismAgeValue { get; private set; }
        private Paragraph organismStrengthText;
        public TextInput OrganismStrengthValue { get; private set; }
        private Paragraph organismSpeedText;
        public TextInput OrganismSpeedValue { get; private set; }

        public Overlay()
        {
            // All temporary
            var panel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, Anchor.CenterRight);
            panel.Padding = new Vector2(10);
            panel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(panel);

            var addObjectsText = new Paragraph("Randomly Add Objects");

            OrganismCountInput = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);
            OrganismCountInput.PlaceholderText = "0";
            OrganismCreateButton = new Button("Organism", ButtonSkin.Default, Anchor.AutoInline, new Vector2(170, 40));

            FoodCountInput = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);
            FoodCountInput.PlaceholderText = "0";
            FoodCreateButton = new Button("Food", ButtonSkin.Default, Anchor.AutoInline, new Vector2(170, 40));

            var addAtCursorText = new Paragraph("Add Objects At Cursor");

            NothingRadio = new RadioButton("None", Anchor.AutoCenter);
            NothingRadio.Checked = true;
            MountainRadio = new RadioButton("Mountain", Anchor.AutoCenter);
            WaterRadio = new RadioButton("Water", Anchor.AutoCenter);
            OrganismRadio = new RadioButton("Organism", Anchor.AutoCenter);
            FoodRadio = new RadioButton("Food", Anchor.AutoCenter);

            var simulationSpeedText = new Paragraph("Set Simulation Speed");

            NormalRadio = new RadioButton("Normal", Anchor.AutoCenter);
            NormalRadio.Checked = true;
            FastRadio = new RadioButton("Fast", Anchor.AutoCenter);
            PauseRadio = new RadioButton("Pause", Anchor.AutoCenter);

            panel.AddChild(addObjectsText);
            panel.AddChild(OrganismCountInput);
            panel.AddChild(OrganismCreateButton);
            panel.AddChild(FoodCountInput);
            panel.AddChild(FoodCreateButton);

            panel.AddChild(addAtCursorText);
            panel.AddChild(NothingRadio);
            panel.AddChild(MountainRadio);
            panel.AddChild(WaterRadio);
            panel.AddChild(OrganismRadio);
            panel.AddChild(FoodRadio);

            panel.AddChild(simulationSpeedText);
            panel.AddChild(NormalRadio);
            panel.AddChild(FastRadio);
            panel.AddChild(PauseRadio);

            this.editAttributesText = new Paragraph("Edit Attributes");

            this.organismSpeciesText = new Paragraph("Species:", Anchor.AutoInline, new Vector2(110, 40));
            this.OrganismSpeciesValue = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);

            this.organismHungerText = new Paragraph("Hunger:", Anchor.AutoInline, new Vector2(110, 40));
            this.OrganismHungerValue = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);

            this.organismAgeText = new Paragraph("Age:", Anchor.AutoInline, new Vector2(110, 40));
            this.OrganismAgeValue = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);

            this.organismStrengthText = new Paragraph("Strength:", Anchor.AutoInline, new Vector2(110, 40));
            this.OrganismStrengthValue = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);

            this.organismSpeedText = new Paragraph("Speed:", Anchor.AutoInline, new Vector2(110, 40));
            this.OrganismSpeedValue = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);

            panel.AddChild(this.editAttributesText);
            panel.AddChild(this.organismSpeciesText);
            panel.AddChild(this.OrganismSpeciesValue);
            panel.AddChild(this.organismHungerText);
            panel.AddChild(this.OrganismHungerValue);
            panel.AddChild(this.organismAgeText);
            panel.AddChild(this.OrganismAgeValue);
            panel.AddChild(this.organismStrengthText);
            panel.AddChild(this.OrganismStrengthValue);
            panel.AddChild(this.organismSpeedText);
            panel.AddChild(this.OrganismSpeedValue);
        }

        /// <summary>
        /// Take input from input devices
        /// </summary>
        /// <param name="gameTime">Time elapsed since last update call</param>
        public void Update(GameTime gameTime, TileHighlight tileHighlight)
        {
            UserInterface.Active.Update(gameTime);
            
            if (tileHighlight.SelectedOrganism == null)
            {
                this.OrganismHungerValue.Value = "";
                this.editAttributesText.Visible = false;
                this.organismSpeciesText.Visible = false;
                this.OrganismSpeciesValue.Visible = false;
                this.organismHungerText.Visible = false;
                this.OrganismHungerValue.Visible = false;
                this.organismAgeText.Visible = false;
                this.OrganismAgeValue.Visible = false;
                this.organismStrengthText.Visible = false;
                this.OrganismStrengthValue.Visible = false;
                this.organismSpeedText.Visible = false;
                this.OrganismSpeedValue.Visible = false;
            }
            else
            {
                this.OrganismSpeciesValue.PlaceholderText = tileHighlight.SelectedOrganism.Attributes.Species;
                this.OrganismHungerValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Hunger, 2).ToString();
                this.OrganismAgeValue.PlaceholderText = tileHighlight.SelectedOrganism.Attributes.Age.ToString();
                this.OrganismStrengthValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Strength, 2).ToString();
                this.OrganismSpeedValue.PlaceholderText = Math.Round(tileHighlight.SelectedOrganism.Attributes.Speed, 2).ToString();

                this.editAttributesText.Visible = true;
                this.organismSpeciesText.Visible = true;
                this.OrganismSpeciesValue.Visible = true;
                this.organismHungerText.Visible = true;
                this.OrganismHungerValue.Visible = true;
                this.organismAgeText.Visible = true;
                this.OrganismAgeValue.Visible = true;
                this.organismStrengthText.Visible = true;
                this.OrganismStrengthValue.Visible = true;
                this.organismSpeedText.Visible = true;
                this.OrganismSpeedValue.Visible = true;
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
