using SplashKitSDK;

namespace PlayerClass
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Robot Dodge Game", 800, 600);
            RobotDodge Game = new RobotDodge(gameWindow);
            Player player = new Player(gameWindow);

        while (!gameWindow.CloseRequested && !Game.Quit)
            {
                SplashKit.ProcessEvents();
                Game.HandleInput();
                Game.Update();
                Game.Draw();

                //display the number of robots
                int robotCount = Game.GetRobotCount();
                Console.WriteLine("Number of robots: " + robotCount);
            }
        }
    }
}
