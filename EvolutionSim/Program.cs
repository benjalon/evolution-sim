namespace EvolutionSim
{
    public class Program
    {
        enum GameState
        {
            StartMenu,
            Exit,
            Running
        }
        private static GameState state = GameState.StartMenu;
        static void Main()
        {

            switch (state) {
                case GameState.StartMenu:
                    var startMenu = new StartMenu();
                    startMenu.Run();
                    break;
                case GameState.Running:
                    var graphics = new Graphics();
                    graphics.Run();
                    //System.Console.WriteLine("YEET");
                    break;
                case GameState.Exit:
                    break;
                
        }   
        }
    }
}
