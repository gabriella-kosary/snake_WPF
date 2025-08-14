using Snake.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Model
{
    public class GameOverEventArgs : EventArgs
    {
        private int _seconds;
        private int _eggCount;

        public int Seconds { get { return _seconds; } }
        public int EggCount { get { return _eggCount; } }

        public GameOverEventArgs(int seconds, int eggCount)
        {
            _seconds = seconds;
            _eggCount = eggCount;
        }
    }
}
