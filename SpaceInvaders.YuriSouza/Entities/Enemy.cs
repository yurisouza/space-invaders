using SpaceInvaders.YuriSouza.Utility;
using System;
using static System.Windows.Forms.Control;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class Enemy : Element
    {
        public int Id { get; set; }
        public bool CanShoot { get; set; }
        public bool IsLive { get; set; }

        private int _speed = Variables.EnemySpeed;
         
        public Enemy(ElementControl controle) : base(controle)
        {
            CanShoot = true;
            IsLive = true;
            Id = Convert.ToInt32(_controle.ElementScreen().AccessibleName);
        }

        public void IncreaseSpeed()
        {
            _speed++;
        }

        private void RestartSpeed()
        {
            _speed = Variables.EnemySpeed;
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
            var shoot = ScreenFactory.NewShoot(Variables.ShotOfEnemy, _controle);
            controls.Add(shoot);
            CanShoot = true;
        }
    }
}
