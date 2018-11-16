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

        public Button CreateOrganismButton { get; private set; }
        public Button CreateFoodButton { get; private set; }
        public Button CreateMountainButton { get; private set; }
        public Button CreateWaterButton { get; private set; }

        public Overlay()
        {
            // All temporary
            var panel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, anchor: Anchor.CenterLeft);
            panel.SetStyleProperty("Opacity", new StyleProperty(100));
            UserInterface.Active.AddEntity(panel);

            var testText = new Paragraph("Click button to create organisms");
            panel.AddChild(testText);

            var list = new SelectList(new Vector2(0, PANEL_WIDTH), Anchor.Auto, null, PanelSkin.Simple);
            list.AddItem("Dog");
            list.AddItem("Human");
            list.AddItem("Rat");
            panel.AddChild(list);
            
            CreateOrganismButton = new Button("Organism");
            CreateFoodButton = new Button("Food");
            CreateMountainButton = new Button("Mountain");
            CreateWaterButton = new Button("Water");

            panel.AddChild(CreateOrganismButton);
            panel.AddChild(CreateFoodButton);
            panel.AddChild(CreateMountainButton);
            panel.AddChild(CreateWaterButton);

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
