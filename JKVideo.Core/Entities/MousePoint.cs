using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Entities
{
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
