using SplashKitSDK;

namespace PlayerClass
{
    public class RobotDodge
    {
        private Player _Player;
        private Window _GameWindow;
        private List<Robot> _Robots;
        private Bitmap _gameOverBitmap;
        private DateTime _gameOverStartTime;
        private bool _gameOverDisplayed;
        private List<Bullet> _Bullets;
        public bool Quit
        {
            get { return _Player.Quit; }
        }
        public RobotDodge(Window gameWindow)
        {
            _GameWindow = gameWindow;
            _Player = new Player(_GameWindow);
            _Robots = new List<Robot>();
            _Bullets = new List<Bullet>();
            _Player.PlayerLivesDepleted += HandlePlayerLivesDepleted;
            _gameOverBitmap = SplashKit.LoadBitmap("GameOver", "Resources/images/gameover.jpeg");
            _gameOverDisplayed = false;
        }

        private void HandlePlayerLivesDepleted(object sender, EventArgs e)
        {
            _gameOverDisplayed = true;
            _gameOverStartTime = DateTime.Now;
        }
        public void HandleInput()
        {
            _Player.HandleInput();
            _Player.StayOnWindow(_GameWindow);
        }

        public void Draw()
        {
            _GameWindow.Clear(Color.White);
            foreach (var robot in _Robots)
            {
                robot.Draw();
            }
            // Draw bullets
            foreach (var bullet in _Bullets)
            {
                bullet.Draw();
            }
            _Player.Draw();
            _Player.DrawLives();
            _Player.DrawScore();

            // Display game over image if the game is over
            if (_gameOverDisplayed)
            {
                float gameOverX = (_GameWindow.Width - _gameOverBitmap.Width) / 2;
                float gameOverY = (_GameWindow.Height - _gameOverBitmap.Height) / 2;

                _gameOverBitmap.Draw(gameOverX, gameOverY);

                if ((DateTime.Now - _gameOverStartTime).TotalSeconds >= 3)
                {
                    _GameWindow.Close();
                }
            }

            _GameWindow.Refresh(60);
        }



        public void Update()
        {
            CheckCollisions();
            foreach (var robot in _Robots.ToList())
            {
                robot.Update();
            }

            if (SplashKit.Rnd() < 0.01)
            {
                _Robots.Add(RandomRobot(_Player));
            }
            _Player.UpdateScore();

            // Handle bullet shooting
            if (_Player.ShootBullet)
            {
                _Bullets.Add(_Player.CreateBullet());
                _Player.ShootBullet = false;
            }
            foreach (var bullet in _Bullets.ToList())
            {
                bullet.Update();
                if (bullet.IsOffscreen(_GameWindow))
                {
                    _Bullets.Remove(bullet);
                }
                else
                {
                    foreach (var robot in _Robots.ToList())
                    {
                        if (bullet.CollidedWith(robot))
                        {
                            _Robots.Remove(robot);
                            _Bullets.Remove(bullet);
                            break; 
                        }
                    }
                }
            }
        }


        private void CheckCollisions()
        {
            List<Robot> robotsToRemove = new List<Robot>();
            bool playerHit = false;

            foreach (var robot in _Robots)
            {
                if (_Player.CollidedWith(robot) || robot.IsOffscreen(_GameWindow))
                {
                    robotsToRemove.Add(robot);

                    if (_Player.CollidedWith(robot))
                    {
                        playerHit = true;
                    }
                }
            }

            foreach (var robotToRemove in robotsToRemove)
            {
                _Robots.Remove(robotToRemove);
            }

            if (playerHit)
            {
                _Player.DecreaseLives();
            }

            if (_Player.Quit)
            {
                _GameWindow.Close();
            }
        }

        public int GetRobotCount()
        {
            return _Robots.Count;
        }

        private Robot RandomRobot(Player player)
        {
            double randValue = SplashKit.Rnd();

            if (randValue < 0.25)
            {
                return new Boxy(_GameWindow, player);
            }
            else if (randValue < 0.5)
            {
                return new Roundy(_GameWindow, player);
            }
            else
            {
                return new Cyborg(_GameWindow, player);
            }
        }
    }
}
