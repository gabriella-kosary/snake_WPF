using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.Model
{
    public class DrawSnakeEventArgs : EventArgs
    {
        private List<PointP> _snake;
        private PointP _tail;

        public List<PointP> Snake { get {  return _snake; } }
        public PointP Tail { get { return _tail; } }

        public DrawSnakeEventArgs(List<PointP> snake, PointP tail)
        {
            _snake = snake;
            _tail = tail;
        }
    }
}
