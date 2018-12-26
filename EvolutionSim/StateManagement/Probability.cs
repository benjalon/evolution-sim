using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.TileGrid;
using EvolutionSim.TileGrid.GridItems;

/// <summary>
/// This class is to return probabilites based on the distribution of organisms on screen 
/// This way we can dynamically update thresholds to accomamte for the organisms in simulation
/// </summary>
namespace EvolutionSim.StateManagement
{

    /// <summary>
    /// this is needed for crossover as well
    /// </summary>
    class Probability
    {

        private Organism mother;
        private Organism father;

        //histogram to hold species distribution
        private int speciesDistribution;

        //these are arbituary values used 
        //for crossover calculation
        private const int crossoverMean = 0;
        private const int stdDev = 1;

        Random random = new Random();

        //need to call the probability constructor to return a 
        //probability object to give the chances of mutation
        public Probability(Organism organismFirst, Organism organismSecond)
        {

            mother = organismFirst;
            father = organismSecond;



        }

        public Probability()
        {




        }

        /// <summary>
        /// This method generates a guassian variable which lies between the range
        /// of -3 and 3
        /// </summary>
        public double generateCrossoverMutationValue()
        {
            double crossoverMean = 0;
            double stdDev = 1;

            Random rand = new Random();
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                Math.Sin(2.0 * Math.PI * u2);

            double randNormal =
             crossoverMean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;

        }



    }
}