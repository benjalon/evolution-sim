namespace EvolutionSim
{
    public class Program
    {

        public static Utility.GameState state = Utility.GameState.StartMenu;
        static void Main()
        {
            var graphics = new Graphics();
            graphics.Run();
        }
    }
}
