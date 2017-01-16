using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Breakout.GameObjects
{
    public class Ball : GameObject
    {
        public delegate void changeDirection(Ball ball);
        public int xd;
        public int yd;
        public event EventHandler BallWasMissed;

        public Ball(Form1 form)
        {
            try
            {
                theImage = Image.FromFile(Environment.CurrentDirectory + "\\Sprites\\ball.png");
                if ((theImage.Height > form.Height) || (theImage.Width > form.Width))
                    throw new ImageDoesNotFitException("Ball image does not fit to main window!");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Ball image file was not found.");
            }
            catch (ImageDoesNotFitException e)
            {
                throw;
            }
            theImage = new Bitmap(theImage, new Size(30, 30));
            this.destroyed = false;
            this.form = form;
            this.height = theImage.Height;
            this.width = theImage.Width;
            xd = 2;
            yd = -2;
        }

        public void Draw(PaintEventArgs e)
        {
            var rect = GetRect(this);
            if (e.ClipRectangle.IntersectsWith(rect) && destroyed == false)
            {
                e.Graphics.DrawImage(theImage, rect);
            }
        }

        public void Move()
        {
            x += xd;
            y += yd;
            if (y >= form.Height)
                BallWasMissed(this, new EventArgs());
        }

        public void changeDir(changeDirection method)
        {
            method(this);
        }
    }
}
