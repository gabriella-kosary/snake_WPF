using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Persistence;

namespace Snake.Model
{
    public class GenerateTableEventArgs : EventArgs
    {
        private int _tableSize;
        private List<PointP> _walls;

        public int TableSize { get { return _tableSize;} }
        public List<PointP> Walls { get { return _walls; } }
    
        public GenerateTableEventArgs(int tableSize,  List<PointP> walls)
        {
            _tableSize = tableSize;
            _walls = walls;
        }
    }
}
