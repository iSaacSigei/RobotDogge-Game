using SplashKitSDK;

namespace PlayerClass
{
    public class Bullet
    {
        private Bitmap _bulletBitmap;
        private double _x;
        private double _y;
        private Vector2D _velocity;
        private const double SPEED = 8;

        public Bullet(double startX, double startY, double targetX, double targetY)
        {
            _bulletBitmap = SplashKit.BitmapNamed("Bullet");
            if (_bulletBitmap == null)
            {
                _bulletBitmap = new Bitmap("Bullet", "Resources/images/bullet-removebg-preview(1).png");
            }

            _x = startX;
            _y = startY;

            Point2D target = new Point2D() { X = targetX, Y = targetY };
            Point2D origin = new Point2D() { X = startX, Y = startY };
            Vector2D direction = SplashKit.VectorPointToPoint(origin, target);
            _velocity = SplashKit.VectorMultiply(SplashKit.UnitVector(direction), SPEED);
        }

        public void Update()
        {
            _x += _velocity.X;
            _y += _velocity.Y;
        }

        public void Draw()
        {
            _bulletBitmap.Draw(_x, _y);
        }
        public bool CollidedWith(Robot robot)
        {
            return _bulletBitmap.CircleCollision(_x + _bulletBitmap.Width / 2, _y + _bulletBitmap.Height / 2, robot.CollisionCircle);
        }


        public bool IsOffscreen(Window screen)
        {
            return _x < 0 || _x > screen.Width || _y < 0 || _y > screen.Height;
        }

        public Circle CollisionCircle
        {
            // Set collision radius to be 5
            get { return SplashKit.CircleAt(_x + _bulletBitmap.Width / 2, _y + _bulletBitmap.Height / 2, 5); }
        }
    }
}
