using System;
using System.Timers;

/**
 * Main class for handling evolution sim, currently just contains a globally accessable timer with fixed intervals of one second 
 **/
namespace EvolutionSim
{

    class Program
    {

        //need a global timer to keep track of time passing within the system
       static Timer genericTimer = new Timer();

        static void Main(string[] args)

        {
       

            genericTimer = new System.Timers.Timer();

            //sets the interval to 1 seconds
            genericTimer.Interval = 1000;


  
            Console.WriteLine("Hello World");

            Console.ReadLine();
        }


        /**
         * When the simulation is ran begin the timer
        **/ 
        static void Begin()
        {
            genericTimer.Enabled = true;

        }


        public int updateTime()
        {


            return 0;
        }
    }
}
