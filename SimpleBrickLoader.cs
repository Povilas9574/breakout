using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.GameObjects;
using System.Drawing;

namespace Breakout
{
    class SimpleBrickLoader : IImageLoader
    {
        public void LoadImage(GameObject obj)
        {
            obj.theImage = Image.FromFile(Environment.CurrentDirectory + "\\Sprites\\brick.png");
            obj.theImage = new Bitmap(obj.theImage, new Size(100, 35));
        }
    }
}
