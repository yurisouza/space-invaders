using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class AirShip : Element
    {
        public int Lives { get; set; }
        public bool CanShoot { get; set; }

        private int _speed = Variables.AirShipSpeed;

        private AirShip()
        {
            Lives = 3;
            CanShoot = true;
        }

        public AirShip(ElementControl controle) : this()
        {
            _controle = controle;
        }

        public override void MoveToLeft()
        {
            _controle.MoveToLeft(_speed);
        }

        public override void MoveToRight()
        {
            _controle.MoveToRight(_speed);
        }

        public void Shoot(ControlCollection controls)
        {
            CanShoot = false;
            var shoot = ScreenFactory.NewShoot(Variables.ShotOfAirship, _controle);
            controls.Add(shoot);
            CanShoot = true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
