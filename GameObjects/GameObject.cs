using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.GameObjects
{
    public class GameObject
    {
        public Image theImage;
        public int x;
        public int y;
        protected int width;
        protected int height;
        protected Form1 form;
        public bool destroyed;
        public Func<GameObject, Rectangle> GetRect = delegate (GameObject obj)
        {
            if (obj.destroyed)
                return Rectangle.Empty;
            return new Rectangle(
              obj.x,
              obj.y,
              obj.width,
              obj.height);
        };

        public BrickPaddleSidesEnum Intersects(GameObject obj)
        {
            var ballRect = obj.GetRect(obj);
            if (!GetRect(this).IntersectsWith(ballRect) || destroyed)
                return BrickPaddleSidesEnum.NoIntersection;
            if ((obj.y + 3 > y) && (obj.y < y + height - 3))
                return BrickPaddleSidesEnum.RightLeft;
            if ((ballRect.X - this.GetRect(this).X) < (this.GetRect(this).Width / 2))
            {
                return BrickPaddleSidesEnum.TopBottomLeft;
            }
            else
            {
                return BrickPaddleSidesEnum.TopBottomRight;
            }
        }
    }
}
