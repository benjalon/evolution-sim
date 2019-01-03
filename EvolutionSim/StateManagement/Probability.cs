using System;
using EvolutionSim.Sprites;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionSim.TileGrid;

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

        //histogram to hold species distribution
        private int speciesDistribution;

        //these are arbituary values used 
        //for crossover calculation
        private const int crossoverMean = 0;
        private const int stdDev = 1;

        Random random = new Random();

        /// <summary>
        /// default constructor
        /// </summary>
        public Probability()
        {

     
        }

        /// <summary>
        /// Code used from: https://stackoverflow.com/questions/218060/random-gaussian-variables
        /// 
        /// This method generates a guassian variable which lies between the range
        /// of -3 and 3, 
        /// Works by performing a Box-Muller transform (single dimension) on a pair of 
        /// randomly generated doubles
        /// 
        /// </summary>
        public double generateCrossoverMutationValue()
        {
            double crossoverMean = 0;
            double stdDev = 1;

            Random rand = new Random();
            double sample1 = 1.0 - rand.NextDouble();
            double sample2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(sample1)) *
                Math.Sin(2.0 * Math.PI * sample2);

            double randNormal =
             crossoverMean + stdDev * randStdNormal; 

            return randNormal;

        }



    }
}