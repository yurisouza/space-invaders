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
    public abstract class Element : ICloneable
    {
        protected ElementControl _controle { get; set; }

        public Element(){}

        public Element(ElementControl controle)
        {
            _controle = controle;
        }

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
            return _controle.ElementScreen();
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

        public object Clone()
        {
            var element = (Element) MemberwiseClone();
            element._controle = new ElementControl(new ControlImplementation(element.ElementScreen()));
            return element;
        }
    }
}
