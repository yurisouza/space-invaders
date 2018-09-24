using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class ElementControl
    {
        public Control ElementScreen { get; set; }
        private Position _position;

        public ElementControl(Control elementSreen)
        {
            ElementScreen = elementSreen;
            _position = new Position(ElementScreen.Left, ElementScreen.Top);
        }

        public ElementControl(Control elementSreen, DirectionEnum directionEnum)
        {
            ElementScreen = elementSreen;
            _position = new Position(ElementScreen.Left, ElementScreen.Top, directionEnum);
        }

        public bool CanMoveToLeft()
        {
            return _position.Direction == DirectionEnum.LEFT && _position.Left > 2;
        }

        public bool CanMoveToRight()
        {
            return _position.Direction == DirectionEnum.RIGHT && _position.Left < 472;
        }

        public void ChangeDirection(DirectionEnum direction)
        {
            _position.Direction = direction;
        }

        public DirectionEnum GetDirection()
        {
            return _position.Direction;
        }

        public void MoveToLeft(int speed)
        {
            _position.Left -= speed;
            Move(_position);
        }

        public void MoveToRight(int speed)
        {
            _position.Left += speed;
            Move(_position);
        }

        public void MoveToDown(int speed)
        {
            _position.Top += speed;
            Move(_position);
        }

        private void Move(Position position)
        {
            ElementScreen.Left = position.Left;
            ElementScreen.Top = position.Top;
        }
    }
}
