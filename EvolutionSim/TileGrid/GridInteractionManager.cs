using EvolutionSim.Data;
using EvolutionSim.Sprites;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid
{
    public class GridInteractionManager
    {
        public Organism SelectedOrganism { get; private set; }
        public Tile HighlightedTile { get; private set; }
        private readonly Sprite tileHighlight;

        public DrawingSettings DrawingSetting { get; set; }

        private readonly MouseStateManager mouseStateManager = new MouseStateManager();
        private int TileIndexX { get => this.mouseStateManager.ScreenPositionX / Tile.TILE_SIZE; }
        private int TileIndexY { get => this.mouseStateManager.ScreenPositionY / Tile.TILE_SIZE; }
        private bool IsWithinGrid { get => TileIndexX >= 0 && TileIndexX < Grid.TILE_COUNT_X && TileIndexY >= 0 && TileIndexY < Grid.TILE_COUNT_Y; }
        private bool IsClickedWithinGrid { get => this.mouseStateManager.IsClicked && IsWithinGrid; }
        private bool IsHeldWithinGrid { get => this.mouseStateManager.IsHeld && IsWithinGrid; }

        public GridInteractionManager(Texture2D texture)
        {
            this.tileHighlight = new Sprite(texture, new Rectangle(0, 0, Tile.TILE_SIZE, Tile.TILE_SIZE));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsWithinGrid)
            {
                this.tileHighlight.Draw(spriteBatch);
            }
        }

        public void Update(Simulation simulation, Grid grid)
        {
            this.mouseStateManager.Update();

            // If a click occurs within the grid, deselect any selected organisms
            if (this.SelectedOrganism != null && IsClickedWithinGrid)
            {
                this.SelectedOrganism.IsSelected = false;
                this.SelectedOrganism = null;
            }
            else if (IsWithinGrid)
            {
                this.UpdateHighlightedTile(grid);

                if (IsHeldWithinGrid)
                {
                    if (this.HighlightedTile.HasOrganismInhabitant)
                    {
                        this.UpdateOrganismSelection();  // An organism was clicked, so select it
                    }
                    else
                    {
                        this.PlaceGridItem(simulation, grid); // An organism was not clicked, so draw whatever is selected at the clicked tile (e.g. mountains or water)
                    }
                }
            }
        }

        private void UpdateHighlightedTile(Grid grid)
        {
            this.HighlightedTile = grid.GetTileAt(TileIndexX, TileIndexY);
            this.tileHighlight.SetScreenPosition(this.HighlightedTile.ScreenPositionX, this.HighlightedTile.ScreenPositionY);
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

        private void PlaceGridItem(Simulation simulation, Grid grid)
        {
            switch (DrawingSetting)
            {
                case DrawingSettings.Grass:
                    PlaceTerrain(grid, TerrainTypes.Grass);
                    break;
                case DrawingSettings.Mountain:
                    PlaceTerrain(grid, TerrainTypes.Mountain);
                    break;
                case DrawingSettings.Water:
                    PlaceTerrain(grid, TerrainTypes.Water);
                    break;
                case DrawingSettings.Organism:
                    simulation.AddOrganism(this.HighlightedTile.GridIndex.X, this.HighlightedTile.GridIndex.Y);
                    break;
                case DrawingSettings.Food:
                    simulation.AddFood(this.HighlightedTile.GridIndex.X, this.HighlightedTile.GridIndex.Y);
                    break;
                default:
                    break;
            }
        }

        private void PlaceTerrain(Grid grid, TerrainTypes terrainType)
        {
            // Draw terrain in a 3 by 3 around the mouse position tile
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    grid.SetTerrainAt(terrainType, this.HighlightedTile.GridIndex.X + x, this.HighlightedTile.GridIndex.Y + y);
                }
            }
        }
    }
}
