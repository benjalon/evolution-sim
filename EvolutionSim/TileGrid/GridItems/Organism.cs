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




        public const int matingCd = 10000;

        public int MilliSecondsSinceLastMovement;

        public int MilliSecondsSinceLastMate = 10001;

        //what state is the organism currently in
        public PotentialStates OrganismState { get; set; }
        public Boolean MovingOnPath { get; set; }
        public List<Tile> Path { get; set; }
        
        // private OrganismState _state;

        public Organism(Texture2D texture)
            : base(texture)
        {
            this.attributes = new OrganismAttributes(0, 8, 500, 50);
            TOTAL_POPULATION++;
            OrganismState = PotentialStates.Roaming;
            Path = new List<Tile>();
        }

        /// <summary>
        /// Signal to a mate to stop
        /// </summary>
        public void PingMate()
        {
            this.attributes.WaitingForMate = true;
        }

        /// <summary>
        /// signal to waiting organism they can move
        /// </summary>
        public void pingFinished()
        {
            this.attributes.WaitingForMate = false;

        }

        /// <summary>
        /// Check if orgaism is ready to mate
        /// </summary>
        /// <returns></returns>
        public bool readyToMate()
        {
            MilliSecondsSinceLastMate += Graphics.ELAPSED_TIME;

            if (this.MilliSecondsSinceLastMate < matingCd)
            {

                return false;

            }

            else
            {
                MilliSecondsSinceLastMate = 0;
                return true;

            }
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
        public bool WaitingForMate { get; set; }
        public bool MateFound { get; set; }
        public bool JustMated { get; set; }

        public OrganismAttributes(int age,
                                  double hunger,
                                  double speed,
                                  double strength)
        {
            DetectionRadius = 3;
            DetectionDiameter = DetectionRadius * 2;
            Age = age;
            Hunger = hunger;
            Speed = speed;
            Strength = strength;
            JustMated = false;

        }
    }
}
