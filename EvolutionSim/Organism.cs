using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvolutionSim
{
    public class Organism : MapItem
    {
        public static int TOTAL_POPULATION = 0;
        public OrganismAttributes _attributes;
        private String _speciesName;

        public Tile DestinationTile;

        public const int MS_PER_DIRECTION_CHANGE = 500;

        public int MilliSecondsSinceLastMovement;
        //what state is the organism currently in
        public PotentialStates OrganismState { get; set; }
        public Boolean MovingOnPath { get; set; }
        public List<Tile> Path { get; set; }

        private Rectangle collisionBox;

        // private OrganismState _state;

        public Organism(Texture2D texture)
            : base(texture)
        {
            _attributes = new OrganismAttributes(0, 0.6, 500, 50, FoodType.Carnivore);
            TOTAL_POPULATION++;
            OrganismState = PotentialStates.Roaming;
            Path = new List<Tile>();

            this.collisionBox = new Rectangle(Rectangle.X - _attributes._DetectionRadius, Rectangle.Y - _attributes._DetectionRadius, _attributes._DetectionDiameter, _attributes._DetectionDiameter);
        }


        public void UpdateCollisionBox(int x, int y)
        {
            this.collisionBox.X = x - _attributes._DetectionRadius;
            this.collisionBox.Y = y - _attributes._DetectionRadius;
        }

        public override String ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Species Name: ").Append(_speciesName).Append(Environment.NewLine);
            stringBuilder.Append("Age: ").Append(_attributes._age).Append(Environment.NewLine);
            stringBuilder.Append("Speed: ").Append(_attributes._speed).Append(Environment.NewLine);
            return stringBuilder.ToString();
        }
    }
}
