using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Source.UI
{
    public class Overlay
    {
        public Button Button { get; private set; }

        public Overlay()
        {
            var panel = new Panel(new Vector2(200, 0), PanelSkin.Simple, anchor: Anchor.CenterLeft);
            UserInterface.Active.AddEntity(panel);

            var testText = new Paragraph("Title");
            panel.AddChild(testText);

            var list = new SelectList(new Vector2(0, 280));
            list.AddItem("Dog");
            list.AddItem("Human");
            list.AddItem("Rat");
            panel.AddChild(list);
            
            Button = new Button("Test");
            panel.AddChild(Button);
        }

        public void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
