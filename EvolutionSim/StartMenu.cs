using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.Data;
using EvolutionSim.UI;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace EvolutionSim
{
    public class StartMenu : Game
    {
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public StartMenu()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            this.graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            this.graphics.ApplyChanges();
        }
        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            base.Initialize();
        }
        // create a panel and position in center of screen
        

        protected override void LoadContent()
        {

            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            Paragraph header = new Header("Welcome to Evolution-Sim", Anchor.TopCenter,new Vector2(0,WINDOW_HEIGHT/8));
            header.FillColor = Color.White;
            header.Scale = 5.0f;

            HorizontalLine horizontalLine = new HorizontalLine(Anchor.TopCenter, new Vector2(0, WINDOW_HEIGHT /4));
            horizontalLine.Padding = new Vector2(10, 10);

            Button create = new Button("Create", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(WINDOW_WIDTH / 8, 50), new Vector2(0, WINDOW_HEIGHT / 8));
            Button exit = new Button("Exit", ButtonSkin.Default, Anchor.AutoCenter,new Vector2(WINDOW_WIDTH/8,50));
           


            UserInterface.Active.AddEntity(header);
            UserInterface.Active.AddEntity(horizontalLine);
            UserInterface.Active.AddEntity(create);
            UserInterface.Active.AddEntity(exit);


            // add title and text
            //panel.AddChild(new Header(""));
            //panel.AddChild(new HorizontalLine());
            //panel.AddChild(new Paragraph("This is a simple panel with a button."));

            // add a button at the bottom

        }
        protected override void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);
            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw graphical elements to screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkKhaki); // Set background color
            UserInterface.Active.Draw(spriteBatch); // Draw UI elements (doesn't affect draw order because it draws to a render target

            //this.WriteFPS(gameTime);

            // Draw graphical elements
            this.spriteBatch.Begin();
            
            this.spriteBatch.End();

            // Draw UI elements on top
            UserInterface.Active.DrawMainRenderTarget(spriteBatch);

            base.Draw(gameTime);
        }


    }
}
