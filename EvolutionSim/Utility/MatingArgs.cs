using EvolutionSim.TileGrid.GridItems;
using System;
using EvolutionSim.StateManagement;

namespace EvolutionSim.Utility
{
    public class MatingArgs : EventArgs
    {

        /// <summary>
        /// this enum is used to show the severity of mutations based on the return from the probability class
        /// </summary>
        public enum Severity
        {
            ExtremelyBad,
            MiddleBad,
            MidBad,
            MildGood,
            MiddleGood,
            ExtremelyGood,

        }

        public Organism Father { get; private set; }
        public Organism Mother { get; private set; }

        public Severity Mutation {get; private set; }

        public MatingArgs(Organism father, Organism organism) : base()
        {
            Father = organism;
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
            if(mutationRating < -2)
            {
                this.Mutation = Severity.ExtremelyBad;


            }
            else if(mutationRating >= -2 && mutationRating < -1)
            {



                this.Mutation = Severity.MiddleBad;



            }

            else if(mutationRating >= 0 && mutationRating < 1)
            {


                this.Mutation = Severity.MildGood;


            }

            else if(mutationRating >= 1 && mutationRating < 2)
            {



                this.Mutation = Severity.MiddleGood;


            }

            else
            {

                this.Mutation = Severity.ExtremelyGood;

            }


        }


    }
}
