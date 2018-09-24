using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza.Entities
{
    public abstract class Element
    {
        protected ElementControl _controle { get; set; }

        public void ChangeDirection(DirectionEnum direction)
        {
            _controle.ChangeDirection(direction);
        }

        public DirectionEnum GetDirection()
        {
            return _controle.GetDirection();
        }

        public Control ElementScreen()
        {
            return _controle.ElementScreen;
        }

        public bool CanMoveToLeft()
        {
            return _controle.CanMoveToLeft();
        }

        public bool CanMoveToRight()
        {
            return _controle.CanMoveToRight();
        }

        public abstract void MoveToLeft();

        public abstract void MoveToRight();
    }
}
