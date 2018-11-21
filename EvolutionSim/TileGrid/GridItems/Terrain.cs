using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    public enum TerrainTypes
    {
        Grass,
        Mountain,
        Water
    }

    public class Terrain : Sprite
    {
        private TerrainTypes currentTerrain = TerrainTypes.Grass;

        private Texture2D[] textures;

        public int MovementDifficulty { get; private set; }
        private int difficultyModifier = 2;

        public Terrain(Texture2D[] textures, Rectangle rectangle) : base(null, rectangle)
        {
            this.textures = textures;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                base.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Set the terrain of this tile to the given type.
        /// </summary>
        /// <param name="terrainType">The type of terrain to set.</param>
        public void SetTerrain(TerrainTypes terrainType)
        {
            this.currentTerrain = terrainType;

            var terrainIndex = (int)terrainType;
            this.MovementDifficulty = terrainIndex * this.difficultyModifier;
            this.texture = this.textures[terrainIndex];
        }
    }
}
