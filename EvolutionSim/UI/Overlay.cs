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
        public static int PANEL_WIDTH = 200;
        
        public Overlay()
        {
            // All temporary
            var panel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, Anchor.CenterLeft);
            panel.Padding = new Vector2(10, 10);
            panel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(panel);
            
            //var list = new SelectList(new Vector2(0, PANEL_WIDTH), Anchor.Auto, null, PanelSkin.Simple);
            //list.AddItem("Dog");
            //list.AddItem("Human");
            //list.AddItem("Rat");
            //panel.AddChild(list);

            var addItemsText = new Paragraph("Add Items");

            var organismCountInput = new TextInput(false, new Vector2(60, 40), Anchor.AutoInline, null, PanelSkin.Simple);
            organismCountInput.Value = "0";
            var organismCreateButton = new Button("Organism", ButtonSkin.Fancy, Anchor.AutoInline, new Vector2(120, 40));

            var foodCountInput = new TextInput(false, new Vector2(60, 40), Anchor.AutoInline, null, PanelSkin.Simple);
            foodCountInput.Value = "0";
            var foodCreateButton = new Button("Food", ButtonSkin.Fancy, Anchor.AutoInline, new Vector2(120, 40));

            var waterCountInput = new TextInput(false, new Vector2(60, 40), Anchor.AutoInline, null, PanelSkin.Simple);
            waterCountInput.Value = "0";
            var waterCreateButton = new Button("Water", ButtonSkin.Fancy, Anchor.AutoInline, new Vector2(120, 40));

            var mountainCountInput = new TextInput(false, new Vector2(60, 40), Anchor.AutoInline, null, PanelSkin.Simple);
            mountainCountInput.Value = "0";
            var mountainCreateButton = new Button("Mountain", ButtonSkin.Fancy, Anchor.AutoInline, new Vector2(120, 40));
            
            var editAttributesText = new Paragraph("Edit Attributes");

            panel.AddChild(addItemsText);
            panel.AddChild(organismCountInput);
            panel.AddChild(organismCreateButton);
            panel.AddChild(foodCountInput);
            panel.AddChild(foodCreateButton);
            panel.AddChild(mountainCountInput);
            panel.AddChild(mountainCreateButton);
            panel.AddChild(waterCountInput);
            panel.AddChild(waterCreateButton);
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
