using EvolutionSim.Source;
using EvolutionSim.Source.UI;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Graphics : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Overlay _overlay;
       
        private List<Sprite> _organisms = new List<Sprite>();

        public Graphics()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            _overlay = new Overlay();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var organismTexture = Content.Load<Texture2D>("face");
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(200, 80, 16, 16)));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(400, 100, 16, 16)));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(300, 16, 16, 16), Color.Yellow));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(680, 500, 16, 16), Color.Red));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(780, 800, 16, 16), Color.Blue));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(600, 516, 16, 16), Color.Yellow));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(580, 300, 16, 16), Color.Red));
            _organisms.Add(new Sprite(ref organismTexture, new Rectangle(380, 400, 16, 16), Color.Blue));
        }
        
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _overlay.Update(gameTime);

            foreach (var organism in _organisms)
            {
                organism.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            _spriteBatch.Begin();
            foreach (var organism in _organisms)
            {
                organism.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            _overlay.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
