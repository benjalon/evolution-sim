﻿using System;

namespace EvolutionSim
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using (var graphics = new Graphics())
                graphics.Run();
        }
    }
}
