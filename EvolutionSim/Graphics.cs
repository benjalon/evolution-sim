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
        private const int WINDOW_SIZE = 800;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Overlay _overlay;

        private Texture2D _organismTexture;
        private List<Sprite> _organisms = new List<Sprite>();

        private Random _random = new Random(); // Temporary

        public Graphics()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = WINDOW_SIZE;
            _graphics.PreferredBackBufferHeight = WINDOW_SIZE;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Initial setup.
        /// </summary>
        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            _overlay = new Overlay();
            _overlay.Button.OnClick = (Entity btn) => createOrganism();
            
            base.Initialize();
        }

        /// <summary>
        /// Asset loading (textures, sounds etc.)
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            _organismTexture = Content.Load<Texture2D>("face");
        }
        
        /// <summary>
        /// Asset unloading
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Update the application
        /// </summary>
        /// <param name="gameTime">Delta - time since last update call</param>
        protected override void Update(GameTime gameTime)
        {
            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                Exit();
            }

            // Update graphical elements
            foreach (var organism in _organisms)
            {
                organism.Update(gameTime);
            }

            // Update UI elements
            _overlay.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw graphical elements to screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen); // Set background color

            // Draw graphical elements
            _spriteBatch.Begin();
            foreach (var organism in _organisms)
            {
                organism.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            // Draw UI elements on top
            _overlay.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        
        private void createOrganism()
        {
            // Temporary
            _organisms.Add(new Sprite(ref _organismTexture, 
                                     new Rectangle(_random.Next(0, 801), _random.Next(0, 801), 16, 16), 
                                     Color.FromNonPremultiplied(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256), 255)));
        }
    }
}
