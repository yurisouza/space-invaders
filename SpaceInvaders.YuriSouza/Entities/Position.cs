using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class Position
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public DirectionEnum Direction { get; set; }

        public Position(int left, int top)
        {
            Left = left;
            Top = top;
            Direction = DirectionEnum.RIGHT;
        }

        public Position(int left, int top, DirectionEnum directionEnum)
        {
            Left = left;
            Top = top;
            Direction = directionEnum;
        }
    }
}
