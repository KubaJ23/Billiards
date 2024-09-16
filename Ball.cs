using System.Drawing;

namespace billard
{
    public class Ball
    {
        public double X;
        public double Y;
        public Velocity velocity;
        public int mass = 1;
        public int radius;
        public Color colour;
        public bool enabled;

        public Ball(int x, int y, int radius, Color colour)
        {
            this.X = x;
            this.Y = y;
            velocity = new(0, 0);
            this.radius = radius;
            this.colour = colour;
            enabled = true;
        }
        public void move()
        {
            X += velocity.x;
            Y += velocity.y;
        }
    }
}
