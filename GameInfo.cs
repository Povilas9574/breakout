using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    public struct GameInfo
    {
        public DateTime GameDate
        { get; set; }
        public string GameStatus
        { get; set; }

        public GameInfo(bool won)
        {
            GameDate = DateTime.Now;
            if (won)
                GameStatus = "Game won";
            else
                GameStatus = "Game lost";
        }
    }
}
