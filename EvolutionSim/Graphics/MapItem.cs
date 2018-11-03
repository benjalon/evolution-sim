using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim
{
    /// <summary>
    /// A combination of a texture and rectangle form a sprite, where the texture is drawn on top of the rectangle.
    /// This class is purely for static sprites such as banners, background textures or sprites not requiring any
    /// kind of updates.
    /// </summary>
    public abstract class MapItem
    {
        protected Texture2D _texture;
        public Color Color { get; set; }

        public Point GridPosition;


            
        protected Rectangle _rectangle;
        public Rectangle Rectangle
        {

            get => _rectangle;
        }

        public Tile ParentTile { get; private set; }
        
        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the MapItem</param>
        public MapItem(Texture2D texture)
        {
            _texture = texture;
            Color = Color.White;
        }

        /// <summary>
        /// Create a static sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the MapItem</param>
        /// <param name="rectangle">The position and size of the MapItem</param>
        public MapItem(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
            Color = Color.White;
        }

        /// <summary>
        /// Draw the texture at the position of the rectangle
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw this sprite within</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color);
        }

        /// <summary>
        /// Reparents the sprite to the given tile (i.e. makes it an inhabitant of the tile).
        /// </summary>
        /// <param name="tile">The tile to move to</param>
        public void MoveToTile(Tile tile)
        {
            GridPosition.X = tile.GridPositionX;
            GridPosition.Y = tile.GridPositionY;
            ParentTile = tile;
            _rectangle = tile.Rectangle;
            
        }
    }
}
