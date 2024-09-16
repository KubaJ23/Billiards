using System;

namespace billard
{
    public struct Velocity
    {
        public double x;
        public double y;
        private double magnitude;
        public Velocity(double X, double Y)
        {
            this.x = X;
            this.y = Y;
            magnitude = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        public void multiply(float constant)
        {
            x = x * constant;
            y = y * constant;
        }
        public void minus(double value)
        {
            updateMAG();
            double oldMAG = this.magnitude;
            if (magnitude >= value)
            {
                magnitude -= value;
                x = x * (magnitude / oldMAG);
                y = y * (magnitude / oldMAG);
            }
            else
            {
                magnitude = 0;
                x = 0;
                y = 0;
            }
        }
        public void updateMAG()
        {
            magnitude = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        public double GetMagnitude()
        {
            updateMAG();
            return magnitude;
        }
        public void SetMagnitude(double newMagnitude)
        {
            if (this.magnitude != 0)
            {
                this.x = (newMagnitude / this.magnitude) * this.x;
                this.y = (newMagnitude / this.magnitude) * this.y;
                this.magnitude = newMagnitude;
            }
        }
    }

}
