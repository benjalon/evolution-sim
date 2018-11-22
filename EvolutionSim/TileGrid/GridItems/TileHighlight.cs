using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSim.TileGrid.GridItems
{
    public class TileHighlight : Sprite
    {
        private Tile highlightedTile;
        public bool IsHighlighting { get; private set; } = false;

        public TileHighlight(Texture2D texture) : base(texture, new Rectangle(0, 0, Tile.TILE_SIZE, Tile.TILE_SIZE)) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsHighlighting)
            {
                base.Draw(spriteBatch);
            }
        }

        public void Update(MouseManager mouseManager, Grid grid, TerrainTypes selectedTerrain)
        {
            this.IsHighlighting = Grid.InBounds(mouseManager.TileIndexX, mouseManager.TileIndexY);

            if (IsHighlighting)
            {
                this.highlightedTile = grid.GetTileAt(mouseManager.TileIndexX, mouseManager.TileIndexY);
                this.rectangle.X = this.highlightedTile.ScreenPositionX;
                this.rectangle.Y = this.highlightedTile.ScreenPositionY;

                if (mouseManager.IsClicked)
                {
                    grid.SetTerrainAt(selectedTerrain, this.highlightedTile.GridIndex.X, this.highlightedTile.GridIndex.Y);
                }
            }
        }
    }
}
