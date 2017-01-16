using Breakout.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    public interface IImageLoader
    {
        void LoadImage(GameObject obj);
    }
}
