using EvolutionSim.Logic;
using EvolutionSim.UI;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    public class TileHighlight : Sprite
    {
        public Tile HighlightedTile { get; private set; }
        public Organism SelectedOrganism { get; private set; }

        private MouseManager mouseManager = new MouseManager();

        public TileHighlight(Texture2D texture) : base(texture, new Rectangle(0, 0, Tile.TILE_SIZE, Tile.TILE_SIZE)) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.mouseManager.IsWithinGrid)
            {
                base.Draw(spriteBatch);
            }
        }

        public void Update(Simulation simulation, Grid grid, RadioItems selectedRadioItem)
        {
            this.mouseManager.Update();
            
            // If a click occurs within the grid, deselect any selected organisms
            if (this.SelectedOrganism != null && this.mouseManager.IsClickedWithinGrid)
            {
                this.SelectedOrganism.IsSelected = false;
                this.SelectedOrganism = null;
            }
            else if (this.mouseManager.IsWithinGrid)
            {
                this.UpdateHighlightedTile(grid);

                if (this.mouseManager.IsClickedWithinGrid)
                {
                    if (this.HighlightedTile.HasOrganismInhabitant)
                    {
                        this.UpdateOrganismSelection();  // An organism was clicked, so select it
                    }
                    else
                    {
                        this.PlaceGridItem(simulation, grid, selectedRadioItem); // An organism was not clicked, so draw whatever is selected at the clicked tile (e.g. mountains or water)
                    }
                }
            }
        }

        private void UpdateHighlightedTile(Grid grid)
        {
            this.HighlightedTile = grid.GetTileAt(this.mouseManager.TileIndexX, this.mouseManager.TileIndexY);
            this.rectangle.X = this.HighlightedTile.ScreenPositionX;
            this.rectangle.Y = this.HighlightedTile.ScreenPositionY;
        }

        private void UpdateOrganismSelection()
        {
            if (this.SelectedOrganism != null)
            {
                this.SelectedOrganism.IsSelected = false; // clean up old reference
            }

            this.SelectedOrganism = (Organism)this.HighlightedTile.Inhabitant;
            this.SelectedOrganism.IsSelected = true;
        }

        private void PlaceGridItem(Simulation simulation, Grid grid, RadioItems selectedRadioItem)
        {
            switch (selectedRadioItem)
            {
                case RadioItems.Grass:
                case RadioItems.Mountain:
                case RadioItems.Water:
                    // Draw terrain in a 3 by 3 around the mouse position tile
                    for (var x = -1; x <= 1; x++)
                    {
                        for (var y = -1; y <= 1; y++)
                        {
                            grid.SetTerrainAt(selectedRadioItem, this.HighlightedTile.GridIndex.X + x, this.HighlightedTile.GridIndex.Y + y);
                        }
                    }
                    break;
                case RadioItems.Organism:
                    simulation.AddOrganism(this.HighlightedTile.GridIndex.X, this.HighlightedTile.GridIndex.Y);
                    break;
                case RadioItems.Food:
                    simulation.AddFood(this.HighlightedTile.GridIndex.X, this.HighlightedTile.GridIndex.Y);
                    break;
                default:
                    break;
            }
        }
    }
}
