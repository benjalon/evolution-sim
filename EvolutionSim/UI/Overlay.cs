using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.UI
{
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class Overlay
    {
        public static int PANEL_WIDTH = 300;

        public TextInput OrganismCountInput { get; private set; }
        public Button OrganismCreateButton { get; private set; }
        public TextInput FoodCountInput { get; private set; }
        public Button FoodCreateButton { get; private set; }
        public int SelectedTerrain { get; private set; }

        public Overlay()
        {
            // All temporary
            var panel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, Anchor.CenterRight);
            panel.Padding = new Vector2(10, 10);
            panel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(panel);
            
            //var list = new SelectList(new Vector2(0, PANEL_WIDTH), Anchor.Auto, null, PanelSkin.Simple);
            //list.AddItem("Dog");
            //list.AddItem("Human");
            //list.AddItem("Rat");
            //panel.AddChild(list);

            var addItemsText = new Paragraph("Add Items");

            OrganismCountInput = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);
            OrganismCountInput.PlaceholderText = "10";
            OrganismCreateButton = new Button("Organism", ButtonSkin.Default, Anchor.AutoInline, new Vector2(170, 40));

            FoodCountInput = new TextInput(false, new Vector2(110, 40), Anchor.AutoInline, null, PanelSkin.Fancy);
            FoodCountInput.PlaceholderText = "10";
            FoodCreateButton = new Button("Food", ButtonSkin.Default, Anchor.AutoInline, new Vector2(170, 40));

            var terrainDrawText = new Paragraph("Draw Terrain");

            var noTerrainButton = new RadioButton("None", Anchor.AutoCenter);
            noTerrainButton.OnValueChange = (Entity e) => { SelectedTerrain = 0; };
            var mountainButton = new RadioButton("Mountain", Anchor.AutoCenter);
            mountainButton.OnValueChange = (Entity e) => { SelectedTerrain = 1; };
            var waterButton = new RadioButton("Water", Anchor.AutoCenter);
            waterButton.OnValueChange = (Entity e) => { SelectedTerrain = 2; };

            var editAttributesText = new Paragraph("Edit Attributes");

            panel.AddChild(addItemsText);
            panel.AddChild(OrganismCountInput);
            panel.AddChild(OrganismCreateButton);
            panel.AddChild(FoodCountInput);
            panel.AddChild(FoodCreateButton);
            panel.AddChild(terrainDrawText);
            panel.AddChild(noTerrainButton);
            panel.AddChild(mountainButton);
            panel.AddChild(waterButton);
            panel.AddChild(editAttributesText);
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
    }
}
