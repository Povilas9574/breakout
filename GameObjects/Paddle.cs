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
    public class Paddle : GameObject
    {
        public bool right;
        public bool left;
        private int xLeft;

        public Paddle(Form1 form)
        {
            try
            {
                theImage = Image.FromFile(Environment.CurrentDirectory + "\\Sprites\\paddle.png");
                if ((theImage.Height > form.Height) || (theImage.Width > form.Width))
                    throw new ImageDoesNotFitException("Ball image does not fit to main window!");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Paddle image file was not found.");
            }
            catch (ImageDoesNotFitException e)
            {
                throw;
            }
            this.form = form;
            this.destroyed = false;
            this.height = theImage.Height;
            this.width = theImage.Width;
            left = false;
            right = false;
            xLeft = form.Width - theImage.Width;
        }
        
        public Paddle(Form1 form, int x, int y)
        {
            theImage = Image.FromFile(Environment.CurrentDirectory + "\\Sprites\\paddle.png");
            this.form = form;
            this.destroyed = false;
            this.height = theImage.Height;
            this.width = theImage.Width;
            this.x = x;
            this.y = y;
            xLeft = form.Width - theImage.Width;
        }

        public void Draw(PaintEventArgs e)
        {
            var rect = GetRect(this);
            if (e.ClipRectangle.IntersectsWith(rect) && destroyed == false)
            {
                e.Graphics.DrawImage(theImage, rect.Location);
            }
        }

        public void Move()
        {
            if (right && x < xLeft)
                x += 5;
            else if (left && x > 0)
                x -= 5;
        }
    }
}
