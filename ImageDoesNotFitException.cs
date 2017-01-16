using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class ImageDoesNotFitException : Exception
    {
        public ImageDoesNotFitException() : base()
        { }

        public ImageDoesNotFitException(string message) : base(message)
        { }
    }
}
