using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Persistence
{
    public class PointP
    {
        public int x {  get; private set; }
        public int y { get; private set; }

        public PointP()
        {
            x = 0;
            y = 0;
        }
        public PointP(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PointP AddPoint(PointP p)
        {
            return new PointP(this.x + p.x, this.y + p.y);
        }
        public bool Equals(PointP p)
        {
            if (this.x == p.x && this.y == p.y) return true;
            return false;
        }
        public bool IsInList(List<PointP> points)
        {
            foreach (PointP p in points)
            {
                if (this.Equals(p)) return true;
            }
            return false;
        }
    }
}
