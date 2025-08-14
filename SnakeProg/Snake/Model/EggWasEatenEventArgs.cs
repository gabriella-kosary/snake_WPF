using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.Model
{
    public class EggWasEatenEventArgs : EventArgs
    {
        private PointP _egg;
        private List<PointP> _snake;
        private PointP _move;
        private int _eggCount;

        public PointP Egg { get { return _egg; } }
        public List<PointP> Snake { get {  return _snake; } }
        public PointP Move { get { return _move; } }
        public int EggCount {  get { return _eggCount; } }

        public EggWasEatenEventArgs(PointP egg, List<PointP> snake, PointP move, int eggCount)
        {
            _egg = egg;
            _snake = snake;
            _move = move;
            _eggCount = eggCount;
        }
    }
}
