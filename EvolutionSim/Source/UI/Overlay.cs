using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Source.UI
{
    /// <summary>
    /// The overlay which is displayed within the simulation itself.
    /// </summary>
    public class Overlay
    {
        private const int PANEL_WIDTH = 200;

        public Button Button { get; private set; }

        public Overlay()
        {
            // All temporary
            var panel = new Panel(new Vector2(PANEL_WIDTH, 0), PanelSkin.Simple, anchor: Anchor.CenterLeft);
            UserInterface.Active.AddEntity(panel);

            var testText = new Paragraph("Click button to create organisms");
            panel.AddChild(testText);

            var list = new SelectList(new Vector2(0, PANEL_WIDTH));
            list.AddItem("Dog");
            list.AddItem("Human");
            list.AddItem("Rat");
            panel.AddChild(list);
            
            Button = new Button("Test");
            panel.AddChild(Button);
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
