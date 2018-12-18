using EvolutionSim.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EvolutionSim.Sprites
{
    public class Terrain : Sprite
    {
        private TerrainTypes terrainType = TerrainTypes.Grass;
        public TerrainTypes Type
        {
            get => this.terrainType;
            set
            {
                this.terrainType = value;

                var terrainIndex = (int)value;
                this.MovementDifficulty = terrainIndex * DIFFICULTY_MODIFIER;
                this.Texture = this.textures[terrainIndex];
            }
        }

        private readonly Texture2D[] textures;

        public int MovementDifficulty { get; private set; }
        private const int DIFFICULTY_MODIFIER = 2;

        public Terrain(Texture2D[] textures, Rectangle rectangle) : base(null, rectangle)
        {
            this.textures = textures;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
