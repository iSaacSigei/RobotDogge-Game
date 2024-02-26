using SplashKitSDK;

namespace PlayerClass
{
    public abstract class Robot
    {
        private double _X;
        private double _Y;
        private Color _MainColor;
        private Vector2D _Velocity;

        public int Width
        {
            get { return 50; }
        }

        public int Height
        {
            get { return 50; }
        }

        public Robot(Window gameWindow, Player player)
        {
            // Randomly pick top/bottom or left/right
            if (SplashKit.Rnd() < 0.5)
            {
                X = SplashKit.Rnd(gameWindow.Width);

                // check if we are top or bottom
                if (SplashKit.Rnd() < 0.5)
                    Y = -Height; // Top
                else
                    Y = gameWindow.Height; // Bottom
            }
            else
            {
                Y = SplashKit.Rnd(gameWindow.Height);
                if (SplashKit.Rnd() < 0.5)
                    X = -Width; // Left
                else
                    X = gameWindow.Width; // Right
            }

            // Calculate the direction to head
            Point2D fromPt = new Point2D() { X = X, Y = Y };
            Point2D toPt = new Point2D() { X = player.X, Y = player.Y };
            Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

            // Set the speed and assign to the Velocity
            const int SPEED = 4;
            Velocity = SplashKit.VectorMultiply(dir, SPEED);
            MainColor = Color.RandomRGB(200);
        }
        public void Update()
        {
            X += Velocity.X;
            Y += Velocity.Y;
        }

        public abstract void Draw();
       
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public Color MainColor
        {
            get { return _MainColor; }
            set { _MainColor = value; }
        }

        public Circle CollisionCircle
        {
            // Set collision radius to be 20
            get { return SplashKit.CircleAt(X + Width / 2, Y + Height / 2, 20); }
        }

        public Vector2D Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }
        public bool IsOffscreen(Window screen)
        {
            return X < -Width || X > screen.Width || Y < -Height || Y > screen.Height;
        }
    }
    public class Boxy : Robot
    {
        public Boxy(Window gameWindow, Player player) : base(gameWindow, player)
        {
            // Constructor
        }

        public override void Draw()
        {
            double leftX, rightX;
            double eyeY, mouthY;

            leftX = X + 12;
            rightX = X + 27;
            eyeY = Y + 10;
            mouthY = Y + 30;

            SplashKit.FillRectangle(Color.Gray, X, Y, Width, Height);
            SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
            SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
            SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
            SplashKit.FillRectangle(MainColor, leftX + 2, mouthY + 2, 21, 6);
        }
    }

    public class Roundy : Robot
    {
        public Roundy(Window gameWindow, Player player) : base(gameWindow, player)
        {
            // Constructor
        }

        public override void Draw()
        {
            double leftX, midX, rightX;
            double midY, eyeY, mouthY;

            leftX = X + 17;
            midX = X + 25;
            rightX = X + 33;
            midY = Y + 25;
            eyeY = Y + 20;
            mouthY = Y + 35;

            SplashKit.FillCircle(Color.White, midX, midY, 25);
            SplashKit.DrawCircle(Color.Gray, midX, midY, 25);
            SplashKit.FillCircle(MainColor, leftX, eyeY, 5);
            SplashKit.FillCircle(MainColor, rightX, eyeY, 5);
            SplashKit.FillEllipse(Color.Gray, X, eyeY, 50, 30);
            SplashKit.DrawLine(Color.Black, X, mouthY, X + 50, Y + 35);
        }
    }

    public class Cyborg : Robot
    {
        public Cyborg(Window gameWindow, Player player) : base(gameWindow, player)
        {
            // Constructor
        }

        public override void Draw()
        {
            double bodyTopX = X + 20;
            double bodyTopY = Y + 15;
            double bodyWidth = 30;
            double bodyHeight = 40;
            double headWidth = 20;
            double headHeight = 20;

            SplashKit.FillRectangle(MainColor, bodyTopX, bodyTopY, bodyWidth, bodyHeight);

            SplashKit.FillEllipse(MainColor, X + 25, Y, headWidth, headHeight);

            SplashKit.FillCircle(Color.Red, X + 20, Y + 5, 5); 
            SplashKit.FillCircle(Color.Red, X + 35, Y + 5, 5); 

            SplashKit.FillRectangle(MainColor, X + 10, Y + 10, 10, 30);
            SplashKit.FillRectangle(MainColor, X + 40, Y + 10, 10, 30);

            SplashKit.FillRectangle(MainColor, X + 20, Y + 50, 10, 20);
            SplashKit.FillRectangle(MainColor, X + 35, Y + 50, 10, 20);
        }
    }

    
}
