using EvolutionSim.UI;
using EvolutionSim.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.TileGrid.GridItems
{
    public class Terrain : Sprite
    {
        private RadioAddSprites currentTerrain = RadioAddSprites.Grass;

        private readonly Texture2D[] textures;

        public int MovementDifficulty { get; private set; }
        private const int DIFFICULTY_MODIFIER = 2;

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
        public void SetTerrain(RadioAddSprites terrainType)
        {
            this.currentTerrain = terrainType;

            var terrainIndex = (int)terrainType;
            this.MovementDifficulty = terrainIndex * DIFFICULTY_MODIFIER;
            this.texture = this.textures[terrainIndex];
        }
    }
}
