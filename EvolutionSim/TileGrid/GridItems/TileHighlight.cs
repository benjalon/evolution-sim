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
        public Tile HighlightedTile;
        public Organism SelectedOrganism { get; private set; }
        public bool IsHighlighting { get; private set; } = false;

        private MouseManager mouseManager = new MouseManager();

        public TileHighlight(Texture2D texture) : base(texture, new Rectangle(0, 0, Tile.TILE_SIZE, Tile.TILE_SIZE)) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsHighlighting)
            {
                base.Draw(spriteBatch);
            }
        }

        public void Update(Grid grid, TerrainTypes selectedTerrain)
        {
            this.mouseManager.Update();

            this.IsHighlighting = Grid.InBounds(this.mouseManager.TileIndexX, this.mouseManager.TileIndexY);

            if (IsHighlighting)
            {
                this.HighlightedTile = grid.GetTileAt(this.mouseManager.TileIndexX, this.mouseManager.TileIndexY);
                this.rectangle.X = this.HighlightedTile.ScreenPositionX;
                this.rectangle.Y = this.HighlightedTile.ScreenPositionY;

                if (this.mouseManager.IsClicked)
                {
                    if (this.HighlightedTile.HasOrganismInhabitant())
                    {
                        if (this.SelectedOrganism != null)
                        {
                            this.SelectedOrganism.IsSelected = false; // clean up old reference
                        }

                        this.SelectedOrganism = (Organism)this.HighlightedTile.Inhabitant;
                        this.SelectedOrganism.IsSelected = true;
                    }
                    else
                    {
                        grid.SetTerrainAt(selectedTerrain, this.HighlightedTile.GridIndex.X, this.HighlightedTile.GridIndex.Y);
                    }
                }
            }
            else if (this.SelectedOrganism != null && this.mouseManager.IsClicked)
            {
                this.SelectedOrganism.IsSelected = false;
                this.SelectedOrganism = null;
            }
        }
    }
}
