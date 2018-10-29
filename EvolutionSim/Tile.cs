using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EvolutionSim
{
    enum TerrainTypes
    {
        Grass
    }

    public class Tile : MapItem
    {
        public const int TILE_SIZE = 10;
        private DateTime startDate = DateTime.Now;

        private double _lastUpdate = 0;

        public int MyProperty { get; set; }
        public MapItem Inhabitant { get; private set; }
        private TerrainTypes _terrain = TerrainTypes.Grass; // For the time being, everything is standard grass
        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }

        public Tile(Texture2D texture, Rectangle rectangle) : base(ref texture, rectangle)
        {
            GridPositionX = rectangle.X / TILE_SIZE;
            GridPositionY = rectangle.Y / TILE_SIZE;
        
        }

        public void AddInhabitant(MapItem sprite)
        {
            Inhabitant = sprite;
            sprite.MoveToTile(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (HasInhabitant())
            {
                Inhabitant.Draw(spriteBatch);
            }
        }
    
        public void MoveInhabitant(Tile endPosition)
{
            //double elapsedMs = DateTime.Now.Subtract(startDate).TotalMilliseconds;

            //if (elapsedMs > 2000)
            
                Inhabitant.MoveToTile(endPosition);
                endPosition.Inhabitant = Inhabitant;
                Inhabitant = null;
            //    startDate = DateTime.Now;
            

           
        }

        public bool HasInhabitant()
        {
            return Inhabitant != null;
        }
    }

    //Roam
    //find mate
    //find food

}
