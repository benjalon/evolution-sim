using System;

namespace EvolutionSim
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            var graphics = new Graphics();
            graphics.Run();
        }
    }
}
