using EvolutionSim.Source;
using EvolutionSim.Source.UI;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    public class Graphics : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Overlay _overlay;

        private Texture2D _organismTexture;
        private List<Sprite> _organisms = new List<Sprite>();

        private Random _random = new Random();

        public Graphics()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            _overlay = new Overlay();
            _overlay.Button.OnClick = (Entity btn) => createOrganism();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _organismTexture = Content.Load<Texture2D>("face");
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

        private void createOrganism()
        {
            _organisms.Add(new Sprite(
                ref _organismTexture, 
                new Rectangle(_random.Next(0, 801), _random.Next(0, 801), 16, 16), 
                Color.FromNonPremultiplied(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256), 255)));
        }
    }
}
