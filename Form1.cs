using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace billard
{

    public partial class BillardGame : Form
    {
        // form bounds are 600,800
        Rectangle visibleTableBounds;
        Ball[] balls;
        Point[] holes;
        private double decelerationConstant = 0.04;
        const int ballDiameter = 25;
        Graphics g;
        Color[] colours;
        int numberOfBalls;
        Pen blackpen;
        Brush holeBrush;
        float holeradius = 27;
        int ballsScored;
        //use for formatting string (only use this a few times such as when writing number into balls)
        StringFormat format;
        bool placeWhiteBall;
        bool player1Turn;

        int player1_Score;
        int player2_Score;
        public BillardGame()
        {
            InitializeComponent();
        }

        private void BillardGame_Load(object sender, EventArgs e)
        {
            player1_Score = 0;
            player2_Score = 0;

            player1Turn = true;
            placeWhiteBall = false;
            DoubleBuffered = true;
            format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            numberOfBalls = 16;
            CreatColourList();
            holeBrush = new SolidBrush(Color.FromArgb(100, 70,70,250));
            blackpen = new Pen(Color.Black, 2);
            visibleTableBounds = new Rectangle(100, 100, 800, 400);
            ballsScored = 0;

            holes = new Point[6];
            balls = new Ball[numberOfBalls];

            balls[0] = new Ball(visibleTableBounds.Left + visibleTableBounds.Width / 4, visibleTableBounds.Top + visibleTableBounds.Height / 2, ballDiameter / 2 , colours[0]);
            int ballnum = 1;
            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k <= i; k++)
                {
                    balls[ballnum] = new Ball(
                                     visibleTableBounds.Left + (visibleTableBounds.Width * 3 / 4) + (int)(3 + Math.Cos(Math.PI / 6) * ballDiameter) * i
                                   , visibleTableBounds.Top + visibleTableBounds.Height / 2 + i * (ballDiameter / 2) - (k * ballDiameter)
                                   , ballDiameter / 2
                                   , colours[ballnum]
                                       ) ;
                    ballnum++;
                }
            }
            for (int i = 0; i < holes.Length; i++)
            {
                holes[i] = new Point((i % 3) * (visibleTableBounds.Width / 2) + visibleTableBounds.Left, visibleTableBounds.Top + (i / 3) * visibleTableBounds.Height);
            }
        }

        private void BillardGame_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            //table bounds:
            g.DrawRectangle(Pens.Black, visibleTableBounds);
            // aiming dot:
            g.FillEllipse(Brushes.Purple, PointToClient(MousePosition).X - 2, PointToClient(MousePosition).Y - 2, 4, 4);

            //draws all balls:
            for (int i = 0; i < balls.Length; i++)
            {
                Brush brush = new SolidBrush(balls[i].colour);
                g.DrawEllipse(blackpen, (int)balls[i].X - (ballDiameter / 2), (int)balls[i].Y - (ballDiameter / 2), ballDiameter, ballDiameter);
                g.FillEllipse(brush, (int)balls[i].X - (ballDiameter / 2), (int)balls[i].Y - (ballDiameter / 2), ballDiameter, ballDiameter);
                g.DrawString(Convert.ToString(i), this.Font , Brushes.White, (float)balls[i].X, (float)balls[i].Y , format);
            }
            //draws holes:
            for (int i = 0; i < holes.Length; i++)
            {
                g.FillEllipse(holeBrush, (int)holes[i].X - holeradius, (int)holes[i].Y - holeradius,  2*holeradius, 2*holeradius);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Length; i++)
            {
                if (balls[i].enabled)
                {
                    balls[i].move();
                    //balls[i].velocity.minus(decelerationConstant);
                    balls[i].velocity.x = 0.985f * balls[i].velocity.x;
                    balls[i].velocity.y = 0.985f * balls[i].velocity.y;

                    WallCollisionResolution(balls[i]);

                    for (int k = 0; k < balls.Length; k++)
                    {
                        if (balls[k].enabled)
                        {
                            if (intersect(balls[i], balls[k]) && i != k)
                            {

                                resolveCollision(balls[i], balls[k]);
                                //moveBallApart(balls[i], balls[k]);
                            }
                        }
                    }

                    if (checkForHole(balls[i]))
                    {
                        if (i == 0)
                        {
                            if (player1Turn)
                            {
                                player1Turn = false;
                            }
                            else
                            {
                                player1Turn = true;
                            }

                            balls[i].X = 50;
                            balls[i].Y = 50;
                            balls[i].velocity.SetMagnitude(0);

                            placeWhiteBall = true;
                        }
                        else
                        {
                            balls[i].enabled = false;
                            balls[i].X = visibleTableBounds.Left + visibleTableBounds.Width + 50;
                            balls[i].Y = visibleTableBounds.Top + (ballsScored * (ballDiameter + 5));
                            balls[i].velocity.x = 0;
                            balls[i].velocity.y = 0;

                            ballsScored++;

                            if (player1Turn)
                            {
                                player1_Score++;
                            }
                            else
                            {
                                player2_Score++;
                            }

                            if (ballsScored == (numberOfBalls - 1))
                            {
                                MessageBox.Show("Game over\nplayer 1 score: " + player1_Score + "\nplayer 2 score: " + player2_Score);
                            }
                        }
                    }
                }
            }
        }

        private void BillardGame_Click(object sender, EventArgs e)
        {
            if (!placeWhiteBall)
            {
                if (/*balls[0].velocity.x == 0 && balls[0].velocity.y == 0*/ true)
                {
                    balls[0].velocity.x = (PointToClient(MousePosition).X - balls[0].X) / 10;
                    balls[0].velocity.y = (PointToClient(MousePosition).Y - balls[0].Y) / 10;

                    if (balls[0].velocity.GetMagnitude() > 30)
                    {
                        balls[0].velocity.x = (30 / balls[0].velocity.GetMagnitude()) * balls[0].velocity.x;
                        balls[0].velocity.y = (30 / balls[0].velocity.GetMagnitude()) * balls[0].velocity.y;
                    }
                }
            }
            else
            {
                bool validPos = true;
                for (int i = 0; i < balls.Length; i++)
                {
                    if (intersect(balls[i], new Ball(PointToClient(MousePosition).X, PointToClient(MousePosition).Y, ballDiameter/2 , colours[0])))
                    {
                        validPos = false;
                    }
                }
                if (PointToClient(MousePosition).X <= (visibleTableBounds.Left + balls[0].radius) 
                    || PointToClient(MousePosition).X >= (visibleTableBounds.Left + visibleTableBounds.Width - balls[0].radius)
                    || PointToClient(MousePosition).Y <= visibleTableBounds.Top + balls[0].radius
                    || PointToClient(MousePosition).Y >= visibleTableBounds.Top + visibleTableBounds.Height - balls[0].radius
                    )
                {
                    validPos = false;
                }

                if (validPos)
                {
                    balls[0].X = PointToClient(MousePosition).X;
                    balls[0].Y = PointToClient(MousePosition).Y;
                    placeWhiteBall = false;
                }
                else
                {
                    MessageBox.Show("Cannot place here");
                }
                
            }
        }

        private void refresh_Timer_Tick(object sender, EventArgs e)
        {
            Invalidate(true);
        }
        private bool intersect(Ball ball1, Ball ball2)
        {
            return Math.Sqrt(Math.Pow(ball1.X - ball2.X, 2) + Math.Pow(ball1.Y - ball2.Y, 2)) <= (ball1.radius + ball2.radius);
        }
        private Velocity rotate(Velocity velocity, double angle)
        {
            Velocity rotatedVelocities = new(velocity.x * Math.Cos(angle) - velocity.y * Math.Sin(angle), velocity.x * Math.Sin(angle) + velocity.y * Math.Cos(angle));
            return rotatedVelocities;
        }
        public void resolveCollision(Ball particle, Ball otherParticle)
        {
            double xVelocityDiff = particle.velocity.x - otherParticle.velocity.x;
            double yVelocityDiff = particle.velocity.y - otherParticle.velocity.y;

            int xDist = (int)(otherParticle.X - particle.X);
            int yDist = (int)(otherParticle.Y - particle.Y);

            // Prevent accidental overlap of particles
            if (xVelocityDiff * xDist + yVelocityDiff * yDist >= 0)
            {
                // Grab angle between the two colliding particles
                double angle = -Math.Atan2(otherParticle.Y - particle.Y, otherParticle.X - particle.X);

                // Store mass in var for better readability in collision equation
                int m1 = particle.mass;
                int m2 = otherParticle.mass;

                // Velocity before equation
                Velocity u1 = rotate(particle.velocity, angle);
                Velocity u2 = rotate(otherParticle.velocity, angle);

                // Velocity after 1d collision equation
                Velocity v1 = new(u2.x, u1.y);  /*new(u1.x * (m1 - m2) / (m1 + m2) + u2.x * 2 * m2 / (m1 + m2), u1.y);*/
                Velocity v2 = new(u1.x, u2.y);  /*new(u2.x * (m2 - m1) / (m1 + m2) + u1.x * 2 * m1 / (m1 + m2), u2.y);*/

                // Final velocity after rotating axis back to original location
                Velocity vFinal1 = rotate(v1, -angle);
                Velocity vFinal2 = rotate(v2, -angle);

                // Swap particle velocities for realistic bounce effect
                particle.velocity.x = vFinal1.x;
                particle.velocity.y = vFinal1.y;

                otherParticle.velocity.x = vFinal2.x;
                otherParticle.velocity.y = vFinal2.y;
            }
        }
        public void moveBallApart(Ball ball1, Ball ball2)
        {
            double xdiff = ball1.X - ball2.X;
            double ydiff = ball1.Y - ball2.Y;
            double ballDistance = Math.Sqrt(Math.Pow(ball1.X - ball2.X, 2) + Math.Pow(ball1.Y - ball2.Y, 2));

            double vectorMAG = (ball1.radius + ball2.radius) - (Math.Sqrt(Math.Pow(ball1.X - ball2.X, 2) + Math.Pow(ball1.Y - ball2.Y, 2)));

            double vectorx = xdiff * (vectorMAG / ballDistance);
            double vectory = ydiff * (vectorMAG / ballDistance);

            ball2.X = ball2.X - vectorx;
            ball2.Y = ball2.Y - vectory;
        }

        public void CreatColourList()
        {
            colours = new Color[] { Color.White, Color.FromArgb(200,170,0), Color.Blue, Color.Red, Color.Purple, Color.DarkOrange, Color.Green, Color.Brown, Color.Black, Color.OrangeRed, Color.DeepSkyBlue, Color.IndianRed, Color.MediumPurple, Color.Orange, Color.ForestGreen, Color.SaddleBrown };
        }
        public void WallCollisionResolution(Ball ball)
        {

            if (ball.X >= (visibleTableBounds.Left + visibleTableBounds.Width - (ballDiameter / 2)))
            {
                if (ball.velocity.x > 0)
                {
                    ball.velocity.x = -ball.velocity.x;
                }
            }
            else if (ball.X <= visibleTableBounds.Left + (ballDiameter / 2))
            {
                if (ball.velocity.x < 0)
                {
                    ball.velocity.x = -ball.velocity.x;
                }
            }

            if (ball.Y >= visibleTableBounds.Top + visibleTableBounds.Height - (ballDiameter / 2))
            {
                if (ball.velocity.y > 0)
                {
                    ball.velocity.y = -ball.velocity.y;
                }
            }
            else if (ball.Y <= visibleTableBounds.Top + (ballDiameter / 2))
            {
                if (ball.velocity.y < 0)
                {
                    ball.velocity.y = -ball.velocity.y;
                }
            }
        }
        public bool checkForHole(Ball ball)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                if (Math.Sqrt(Math.Pow(ball.X - holes[i].X, 2) + Math.Pow(ball.Y - holes[i].Y, 2)) <= holeradius)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
