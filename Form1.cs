using Breakout.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace Breakout
{
    public partial class Form1 : Form
    {
        Thread brickRecreationThread1;
        Thread brickRecreationThread2;
        Lazy<Form> aboutForm;
        bool finished;
        Paddle paddle;
        Ball ball;
        Brick[] bricks;
        System.Threading.Timer gameTimer;
        int score;
        private delegate bool CheckCollision(GameObject obj, Ball b);
        CheckCollision checkTopBottomRight = (obj, b) => (obj.Intersects(b) == BrickPaddleSidesEnum.TopBottomRight);
        CheckCollision checkTopBottomLeft = (obj, b) => (obj.Intersects(b) == BrickPaddleSidesEnum.TopBottomLeft);
        CheckCollision checkLeftRight = (obj, b) => (obj.Intersects(b) == BrickPaddleSidesEnum.RightLeft);
        private delegate void InitializeGame(Form1 form);
        InitializeGame initializeGame = delegate(Form1 form)        //anoniminis
        {
            form.finished = false;
            Random rnd = new Random();
            CrackedBrickLoader cracked = new CrackedBrickLoader();
            SimpleBrickLoader simple = new SimpleBrickLoader();
            try
            {
                form.paddle = new Paddle(form, 600, 650);

                form.ball = new Ball(form)
                {
                    x = 655,
                    y = 600
                };
                form.bricks = new Brick[44];
                for (int j = 0; j < 4; j++)
                    for (int i = 0; i < 11; i++)
                        if(rnd.Next(0, 2) != 0)
                            form.bricks[j * 11 + i] = new Brick(form, cracked)
                            {
                                x = 30 + i * 120,
                                y = 50 + j * 50
                            };
                        else
                            form.bricks[j * 11 + i] = new Brick(form, simple)
                            {
                                x = 30 + i * 120,
                                y = 50 + j * 50
                            };
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        };
        private delegate void PrintMessage(string message, Form1 form);
        PrintMessage printMessage = delegate (string message, Form1 form)       //anoniminis
        {
            Graphics formGraphics = form.CreateGraphics();
            string drawString = "Sorry." + message;
            Font drawFont = new Font("Arial", 25);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = 400.0F;
            float y = 0.0F;
            StringFormat drawFormat = new StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        };

        public void brickRecreation1()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(40000);
                for (int j = 0; j < 44; j++)
                {
                    if (bricks[j].destroyed)
                    {
                        bricks[j].destroyed = false;
                    }
                }
            }
        }

        public void brickRecreation2()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(20000);
                for (int j = 0; j < 44; j++)
                {
                    if (bricks[j].destroyed)
                    {
                        bricks[j].destroyed = false;
                    }
                        
                }
            }
        }

        public Form1()
        {
            aboutForm = new Lazy<Form>();
            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);
            InitializeComponent();
        }

        private void BallWasMissedHandler(object sender, EventArgs e)
        {
            StopGame(false);
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            this.startGameButton.Hide();
            this.button1.Hide();
            try
            {
                initializeGame(this);
            }
            catch (FileNotFoundException exc)
            {
                printMessage(exc.Message, this);
                return;
            }
            catch (ImageDoesNotFitException exc)
            {
                printMessage(exc.Message, this);
                return;
            }
            ball.BallWasMissed += BallWasMissedHandler;
            Invalidate();
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            this.Focus();
            gameTimer = new System.Threading.Timer(callback, autoEvent, 0, 15);
            brickRecreationThread1 = new Thread(new ThreadStart(this.brickRecreation1));
            brickRecreationThread2 = new Thread(new ThreadStart(this.brickRecreation2));
            brickRecreationThread1.Start();
            brickRecreationThread2.Start();
        }

        private void callback(object state)
        {
            bool dest = false;
            score = 0;
            ball.Move();
            paddle.Move();
            var ballRect = ball.GetRect(ball);
            ballRect.Inflate(10, 10);
            Invalidate(ballRect);
            var paddleRect = paddle.GetRect(paddle);
            paddleRect.Inflate(10, 10);
            Invalidate(paddleRect);
            for(int i = 0; i < 44; i++)
            {
                if (dest)
                    break;
                if(!bricks[i].destroyed)
                {
                    if(checkTopBottomRight(bricks[i], ball) || checkTopBottomLeft(bricks[i], ball))
                    {
                        bricks[i].destroyed = true;
                        ball.changeDir(changeYD);
                        dest = true;
                    }
                    else if(checkLeftRight(bricks[i], ball))
                    {
                        bricks[i].destroyed = true;
                        ball.changeDir(changeXD);
                        dest = true;
                    }
                }
                else
                {
                    Invalidate(bricks[i].GetRect(bricks[i]));
                    score++;
                }
            }
            if (paddle.Intersects(ball) != BrickPaddleSidesEnum.NoIntersection)
            {
                if (checkTopBottomRight(paddle, ball))
                {
                    ball.changeDir(changeYD);
                    if (ball.xd < 0)
                        ball.changeDir(changeXD);
                }
                else
                {
                    ball.changeDir(changeYD);
                    if (ball.xd > 0)
                        ball.changeDir(changeXD);
                }
            }
            if ((ball.x <= 0)||(ball.x >= (this.DisplayRectangle.Width- 30)))
                ball.changeDir(changeXD);
            if (ball.y <= 0)
                ball.changeDir(changeYD);
            if (score == 44)
            {
                lock(bricks)
                {
                    StopGame(true);
                }
                return;
            }
        }

        public void changeXD(Ball b)
        {
            b.xd *= -1;
        }

        public void changeYD(Ball b)
        {
            b.yd *= -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(paddle != null)
                paddle.Draw(e);
            if (ball != null)
                ball.Draw(e);
            if (bricks != null)
                for (int i = 0; i < 44; i++)
                    if(bricks[i] != null)
                        bricks[i].Draw(e);
        }

        public void StopGame(bool won)
        {
            brickRecreationThread1.Abort();
            brickRecreationThread2.Abort();
            gameTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ball.destroyed = true;
            paddle.destroyed = true;
            if (won)
            {
                if(!finished)
                    MessageBox.Show(ConfigurationManager.AppSettings["CongratulationMessage"]);
                finished = true;
            }
            else
            {
                for (int i = 0; i < bricks.Length; i++)
                {
                    bricks[i].destroyed = true;
                }
                if(!finished)
                    MessageBox.Show(ConfigurationManager.AppSettings["LoseMessage"]);
                finished = true;
            }
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                paddle.right = true;
            if (e.KeyCode == Keys.Left)
                paddle.left = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                paddle.right = false;
            if (e.KeyCode == Keys.Left)
                paddle.left = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aboutForm.Value.Show();
            aboutForm.Value.Text = "About";
            Graphics formGraphics = aboutForm.Value.CreateGraphics();
            string drawString = "Breakout game\nCreated by Povilas Zvirblis.";
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = 0.0F;
            float y = 0.0F;
            StringFormat drawFormat = new StringFormat();
            this.Focus();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            aboutForm.Value.Focus();
            drawFont.Dispose();
            drawBrush.Dispose();
        }
    }
}