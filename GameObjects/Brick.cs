using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Breakout.GameObjects
{
    public class Brick : GameObject
    {
        public Brick(Form1 form, IImageLoader loader)
        {
            this.destroyed = false;
            this.form = form;
            try
            {
                loader.LoadImage(this);
                if ((theImage.Height > form.Height) || (theImage.Width > form.Width))
                    throw new ImageDoesNotFitException("Brick image does not fit to main window!");
                this.height = theImage.Height;
                this.width = theImage.Width;
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Brick image file was not found.");
            }
            catch (ImageDoesNotFitException e)
            {
                throw;
            }
        }

        public void Draw(PaintEventArgs e)
        {
            var rect = GetRect(this);
            if (e.ClipRectangle.IntersectsWith(rect) && destroyed == false)
            {
                e.Graphics.DrawImage(theImage, rect.Location);
            }
        }
    }
}
