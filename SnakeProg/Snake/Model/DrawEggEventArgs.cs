using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.Model
{
    public class DrawEggEventArgs : EventArgs
    {
        private PointP _egg;
        private int _eggCount;

        public PointP Egg { get { return _egg; } }
        public int EggCount {  get { return _eggCount; } }

        public DrawEggEventArgs(PointP egg, int eggCount)
        {
            _egg = egg;
            _eggCount = eggCount;
        }
    }
}
