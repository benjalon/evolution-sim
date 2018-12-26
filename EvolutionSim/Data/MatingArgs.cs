using EvolutionSim.Sprites;
using System;
using EvolutionSim.StateManagement;

namespace EvolutionSim.Data
{
    public class MatingArgs : EventArgs
    {
        private const double TWO_STANDARD_DEVIATION = 2.0;
        private const double ONE_STANDARD_DEVIATION = 1.0;

        /// <summary>
        /// this enum is used to show the severity of mutations based on the return from the probability class
        /// </summary>
        public enum Severity
        {
            ExtremelyBad,
            MiddleBad,
            MildBad,
            MildGood,
            MiddleGood,
            ExtremelyGood,

        }

        //could be used later on to get rid of "magic numbers in code"
        private int StdDeviation;

        public Organism Father { get; private set; }
        public Organism Mother { get; private set; }

        public Severity Mutation { get; private set; }

        public MatingArgs(Organism father, Organism organism) : base()
        {
            Father = father;
            Mother = organism;

            //Generate a probability variable
            //then calculate the severity of the mutation based on a guassian value
            Probability generateMutation = new Probability();
            double guassianValue = generateMutation.generateCrossoverMutationValue();
            calculateSeverity(guassianValue);


        }

        /// <summary>
        /// This method calculates how drastic the childs stats should be relative to the parents
        /// </summary>
        /// <param name="mutationRating"></param>
        private void calculateSeverity(double mutationRating)
        {
            //this is the tail end of our distribution, less than 5% of cases should appear with 
            //this sort of mutation
            if (mutationRating <= -TWO_STANDARD_DEVIATION)
            {
                this.Mutation = Severity.ExtremelyBad;


            }
            else if (mutationRating >= -TWO_STANDARD_DEVIATION && mutationRating < -ONE_STANDARD_DEVIATION)
            {

                this.Mutation = Severity.MiddleBad;

            }

            if(mutationRating >= -ONE_STANDARD_DEVIATION && mutationRating < 0)
            {

                this.Mutation = Severity.MildBad;

            }

            //If within a single positive standard devation of the mean
            else if (mutationRating >= 0 && mutationRating < ONE_STANDARD_DEVIATION)
            {


                this.Mutation = Severity.MildGood;


            }

            //otherwise if the mutation is between 1 and 2 positive standard deviations of the mean
            else if (mutationRating >= ONE_STANDARD_DEVIATION && mutationRating < TWO_STANDARD_DEVIATION)
            {



                this.Mutation = Severity.MiddleGood;


            }

            //else must be over 2 postitive standard deviations
            else
            {

                this.Mutation = Severity.ExtremelyGood;

            }


        }


    }
}
