using Newtonsoft.Json;
using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class ElementControl
    {
        private Control _elementScreen { get; set; }

        private Position _position;

        public ElementControl(IControl elementSreen)
        {
            _elementScreen = (Control) elementSreen.GetControl();
            _position = new Position(_elementScreen.Left, _elementScreen.Top);
        }

        public ElementControl(IControl elementSreen, DirectionEnum directionEnum)
        {
            _elementScreen = (Control) elementSreen.GetControl();
            _position = new Position(_elementScreen.Left, _elementScreen.Top, directionEnum);
        }

        public Control ElementScreen()
        {
            UpdateElementInScreen();
            return _elementScreen;
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
            UpdateElementInScreen();
        }

        public void MoveToRight(int speed)
        {
            _position.Left += speed;
            UpdateElementInScreen();
        }

        public void MoveToDown(int speed)
        {
            _position.Top += speed;
            UpdateElementInScreen();
        }

        private void UpdateElementInScreen()
        {
            _elementScreen.Left = _position.Left;
            _elementScreen.Top = _position.Top;
        }
    }
}
