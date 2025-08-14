using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Persistence
{
    public class TableCell
    {
        private Boolean _isWall;
        private Boolean _isEgg;
        private Boolean _isSnake;
        public Boolean IsWall
        {
            get => _isWall; 
            set => _isWall = value;
        }
        public Boolean IsEgg
        {
            get => _isEgg;
            set => _isEgg = value;
        }
        public Boolean IsSnake
        {
            get => _isSnake;
            set => _isSnake = value;
        }
        public TableCell()
        {

        }
    }
}
