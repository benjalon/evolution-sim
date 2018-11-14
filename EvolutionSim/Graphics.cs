using EvolutionSim.Logic;
using EvolutionSim.UI;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EvolutionSim
{
    public class Graphics : Game
    {
        public static int WINDOW_SIZE = 800;
        public static int ELAPSED_TIME; 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Overlay _overlay;

        private Texture2D _organismTexture;
        private Texture2D _foodTexture;
        private Texture2D _tileTexture;

        private Grid _grid;
        private Brain _brain;

        private double _fpsOld = 0;

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
            _foodTexture = Content.Load<Texture2D>("pizza");
            _tileTexture = Content.Load<Texture2D>("tile");

            var screenWidth = GraphicsDevice.Viewport.Bounds.Width;
            var screenHeight = GraphicsDevice.Viewport.Bounds.Height;

            _grid = new Grid(_tileTexture, screenWidth, screenHeight);
            _brain = new Brain(_grid);
            _overlay.Button.OnClick = (Entity btn) =>
            {
                for (var i = 0; i < 30; i++) _brain.AddOrganism(new Organism(_organismTexture), _grid);
            };

            _overlay.Button_Two.OnClick = (Entity btn) =>
            {
                for (var i = 0; i < 30; i++) _brain.AddFood(new Food(_foodTexture, FoodType.Carnivore), _grid);
            };

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
            ELAPSED_TIME = gameTime.ElapsedGameTime.Milliseconds;
            // Take updates from input devices
            var escapeClicked = Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (escapeClicked)
            {
                Exit();
            }
            
            // Update UI elements
            _overlay.Update(gameTime);


            _brain.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw graphical elements to screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            var fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            if (_fpsOld != fps)
            {
                Console.WriteLine(fps);
                _fpsOld = fps;
            }

            GraphicsDevice.Clear(Color.LightGreen); // Set background color

            // Draw graphical elements
            _spriteBatch.Begin();
            _grid.Draw(_spriteBatch);
            _spriteBatch.End();

            // Draw UI elements on top
            _overlay.Draw(_spriteBatch);
            
            base.Draw(gameTime);
        }


        //for each organism on the grid we need to check if we need to change state,
        //then we call a method which determines how each organism behaves
        private void handle_organism(GameTime gameTime)
        {
 


        }

        
        //private void createOrganism()
        //{
        //    var newOrganism = new Organism(_organismTexture, new Rectangle(_random.Next(0, WINDOW_SIZE + 1), _random.Next(0, WINDOW_SIZE + 1), 16, 16));
            
        //    // TODO Delete this when we don't want random colors anymore
        //    newOrganism.Color = Color.FromNonPremultiplied(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256), 255);

        //    _grid.AddInhabitant(newOrganism);
        //}
    }
}
