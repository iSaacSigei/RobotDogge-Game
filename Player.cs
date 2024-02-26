using SplashKitSDK;

namespace PlayerClass
{
    public class Player
    {
        private Bitmap _playerBitmap;
        private Bitmap _heartBitmap; 
        private int _lives;
        private int _score;
        private DateTime _lastScoreUpdateTime;

        public bool ShootBullet { get; set; }
        private double _targetX;
        private double _targetY;
        // public bool ShootBullet { get; set; }

        public double X { get; private set; }
        public double Y { get; private set; }
        public event EventHandler PlayerLivesDepleted;
        //read only property Width and Height
        public int Width
        {
            get { return _playerBitmap.Width; }
        }

        public int Height
        {
            get { return _playerBitmap.Height; }
        }

        public bool Quit { get; private set; }

        private const int SPEED = 5;


        public int Lives
        {
            get { return _lives; }
        }

        public Player(Window GameWindow)
        {
            _lives = 5;
            _score = 0;
            _lastScoreUpdateTime = DateTime.Now;
            //load PLayer using splashkit

            _playerBitmap = SplashKit.BitmapNamed("Player");
            if (_playerBitmap == null)
            {
                _playerBitmap = new Bitmap("Player", "Resources/images/Player.png");
            }

            _heartBitmap = SplashKit.LoadBitmap("Heart", "Resources/images/heart(1).png");

            //pass the window object to position the player
            X = (GameWindow.Width - Width) / 2;
            Y = (GameWindow.Height - Height) / 2;
        }

        private void HandleMouseClick(MouseButton button, double x, double y)
        {
            if (button == MouseButton.LeftButton)
            {
                ShootBullet = true;
                _targetX = x;
                _targetY = y;
            }
        }

        public Bullet CreateBullet()
        {
            return new Bullet(X + Width / 2, Y + Height / 2, _targetX, _targetY);
        }
        
        public void DrawLives()
        {
            const int heartSize = 1;
            const int gap = 70;
            int x = 450;
            int y = 0;

            for (int i = 0; i < _lives; i++)
            {
                SplashKit.DrawBitmap(_heartBitmap, x, y);

                x += heartSize + gap;
            }
        }

        public void DrawScore()
        {
            SplashKit.DrawText($"Score: {_score}", Color.Black, "Arial", 30, 30, 30);
        }

        public void UpdateScore()
        {
            TimeSpan timeSinceLastUpdate = DateTime.Now - _lastScoreUpdateTime;
            if (timeSinceLastUpdate.TotalSeconds >= 1)
            {
                _score += (int)timeSinceLastUpdate.TotalSeconds;
                _lastScoreUpdateTime = DateTime.Now;
            }
        }

        public void DecreaseLives()
        {
            _lives--;

            // Notify RobotDodge class when lives are exhausted
            if (_lives <= 0)
            {
                PlayerLivesDepleted?.Invoke(this, EventArgs.Empty);
            }
        }


        //draw the players' X any Y values
        public void Draw()
        {
            _playerBitmap.Draw(X, Y);
        }

        public void HandleInput()
        {
            if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                X -= SPEED;
            }
            if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                X += SPEED;
            }
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                Y -= SPEED;
            }
            if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                Y += SPEED;
            }
            if (SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                Quit = true;
            }
                // Check for mouse click
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                ShootBullet = true;
                _targetX = SplashKit.MousePosition().X;
                _targetY = SplashKit.MousePosition().Y;
            }
        }

        public void StayOnWindow(Window gameWindow)
        {
            const int GAP = 10;

            X = Math.Clamp(X, GAP, gameWindow.Width - Width - GAP);
            Y = Math.Clamp(Y, GAP, gameWindow.Height - Height - GAP);
        }


        public bool CollidedWith(Robot other)
        {
            // Use CircleCollision to check for collision
            return _playerBitmap.CircleCollision(X, Y, other.CollisionCircle);
        }
    }
}
