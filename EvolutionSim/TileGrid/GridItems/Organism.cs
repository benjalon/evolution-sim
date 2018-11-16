using EvolutionSim.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSim.TileGrid.GridItems
{
    public class Organism : GridItem
    {
        public static int TOTAL_POPULATION = 0;
        public OrganismAttributes attributes;

        public Tile DestinationTile;
        public float MovementSpeed = 0.0000002f;
        public const int MS_PER_DIRECTION_CHANGE = 600;




        public int MilliSecondsSinceLastMovement;
        //what state is the organism currently in
        public PotentialStates OrganismState { get; set; }
        public Boolean MovingOnPath { get; set; }
        public List<Tile> Path { get; set; }
        
        // private OrganismState _state;

        public Organism(Texture2D texture)
            : base(texture)
        {
            this.attributes = new OrganismAttributes(0, 0.6, 500, 50);
            TOTAL_POPULATION++;
            OrganismState = PotentialStates.Roaming;
            Path = new List<Tile>();
        }
    }

    public class OrganismAttributes
    {
        public string Species { get; set; }
        public int Age { get; set; }
        public double Hunger { get; set; }
        public double Speed { get; set; }
        public double Strength { get; set; }
        public int DetectionRadius { get; set; }
        public int DetectionDiameter { get; set; }
        
        public OrganismAttributes(int age,
                                  double hunger,
                                  double speed,
                                  double strength)
        {
            DetectionRadius = 3;
            DetectionDiameter = DetectionRadius * 2;
            Age = age;
            Hunger = 0;
            Speed = speed;
            Strength = strength;

        }
    }
}
