using SpaceInvaders.YuriSouza.Utility;
using System;
using static System.Windows.Forms.Control;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class Enemy : Element
    {
        public int Id { get; set; }
        public bool CanShoot { get; set; }
        public bool IsLive { get; set; }

        private int _speed = Variables.SpeedEnemy;
         
        public Enemy()
        {
            CanShoot = true;
            IsLive = true;
        }

        public Enemy(ElementControl controle) : this()
        {
            Id = Convert.ToInt32(controle.ElementScreen.AccessibleName);
            _controle = controle;
        }

        public void IncreaseSpeed()
        {
            _speed++;
        }

        private void RestartSpeed()
        {
            _speed = Variables.SpeedEnemy;
        }

        public override void MoveToLeft()
        {
            _controle.MoveToLeft(_speed);
        }

        public void MoveToDown()
        {
            _controle.MoveToDown(10);
        }

        public override void MoveToRight()
        {
            _controle.MoveToRight(_speed);
        }

        public void Shoot(ControlCollection controls)
        {
            CanShoot = false;
            var shoot = ScreenFactory.NewShoot("invaderBullet", _controle);
            controls.Add(shoot);
            CanShoot = true;
        }
    }
}
