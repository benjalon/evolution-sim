using EvolutionSim.Data;
using EvolutionSim.Sprites;
using GeonBit.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EvolutionSim.UI
{
    class About
    {
        private Panel mainPanel;
        private Paragraph aboutParagraph;
        private Button backButton;
        public Boolean backButtonPressed = false;

        public About()
        {
            UserInterface.Active.Clear();

            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.CursorScale = 0.5f;
            this.mainPanel = new Panel(new Vector2(Graphics.WINDOW_WIDTH / 2, Graphics.WINDOW_HEIGHT / 1.25f));
            this.aboutParagraph = new Paragraph("YEEEEEEEEEEEEEET");
            this.backButton = new Button("Back", anchor: Anchor.AutoCenter,size: new Vector2(Graphics.WINDOW_WIDTH / 6, 50));
            this.backButton.OnClick = (Entity btn) =>
            {
                Console.Write("YAY");
                UserInterface.Active.Clear();
                backButtonPressed = true;
            };
            mainPanel.AddChild(aboutParagraph);

            UserInterface.Active.AddEntity(mainPanel);
            UserInterface.Active.AddEntity(backButton);



        }


    }
}
