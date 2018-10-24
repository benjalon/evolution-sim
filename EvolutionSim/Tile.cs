using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    enum TerrainTypes
    {
        Grass
    }

    public class Tile : Sprite
    {
        public const int TILE_SIZE = 16;

        public Sprite Inhabitant { get; set; }
        private TerrainTypes _terrain = TerrainTypes.Grass; // For the time being, everything is standard grass
        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }

        public Tile(ref Texture2D texture, Rectangle rectangle) : base(ref texture, rectangle)
        {
            GridPositionX = rectangle.X / TILE_SIZE;
            GridPositionY = rectangle.Y / TILE_SIZE;
        }

        public void AddInhabitant(Sprite sprite)
        {
            Inhabitant = sprite;
            Inhabitant.Rectangle = _rectangle;
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
            Inhabitant.Rectangle = endPosition.Rectangle;
            endPosition.Inhabitant = Inhabitant;
            Inhabitant = null;
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
