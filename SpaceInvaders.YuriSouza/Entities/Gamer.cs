using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class Gamer
    {
        private Control _player;
        private Timer _timer;
        private ControlCollection _controls;

        public int Score { get; set; }
        public AirShip AirShip { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Enemy> Enemies { get; set; }

        //public Gamer(ControlCollection controls, AirShip airShip, List<Wall> walls, List<Enemy> enemies)
        //{
        //    _controls = controls;
        //    AirShip = airShip;
        //    Walls = walls;
        //    Enemies = enemies;
        //}

        public Gamer(ControlCollection controls, Control player, Timer timer)
        {
            _controls = controls;
            _player = player;
            Walls = new List<Wall>(Variables.TotalWalls);
            Enemies = new List<Enemy>(Variables.EnemiesPerLine * Variables.EnemiesPerCollumn);
        }

        private void LoadGamer()
        {
            BuildAirShip();
            BuildWalls();
            BuildEnemies();
        }

        private void BuildEnemies(int positionStartTop = 150)
        {
            var enemyId = 0;
            var positionStartLeft = Variables.EnemyPositionStartLeft;

            for (int coluna = 0; coluna < Variables.EnemiesPerLine; coluna++)
            {
                positionStartLeft += 42;

                if (positionStartTop < Variables.MinPositionTop)
                    positionStartTop = Variables.MinPositionTop;

                for (int line = 0; line < 4; line++)
                {
                    positionStartTop -= 35;

                    var enemy = ScreenFactory.NewEnemy(enemyId.ToString(), positionStartLeft, positionStartTop);

                    Enemies.Add(enemy);
                    _controls.Add(enemy.ElementScreen());

                    enemyId++;
                }
            }
        }

        private void BuildWalls()
        {
            var shieldId = 0;
            var intervalBetweenWalls = 0;

            for (int wallId = 0; wallId < Variables.TotalWalls; wallId++)
            {
                var wall = ScreenFactory.NewWall(wallId, (39 * (wallId + 1)) + intervalBetweenWalls, 288);

                foreach (var shield in wall.Shields)
                {
                    shield.Id = shieldId++;
                    shield.WallId = wallId;
                    shield.ElementScreen().AccessibleName = $"{wallId}/{shield.Id}";

                    _controls.Add(shield.ElementScreen());
                }

                intervalBetweenWalls += 70;
            }
        }

        private void BuildAirShip()
        {
            //PERCORRER O _controls e pegar o primeiro (player)
            AirShip = new AirShip(new ElementControl(_player, DirectionEnum.STOP));
        }

        public void EventHandler(EventActionEnum eventAction, object sender, KeyEventArgs e)
        {
            if (eventAction == EventActionEnum.DOWN)
                EventDown(sender, e);
            else if (eventAction == EventActionEnum.UP)
                EventUp(sender, e);
            else if (eventAction == EventActionEnum.SPACE)
                EventSpace(sender, e);
            else
                EventRestart(sender, e);
        }

        private void EventDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                AirShip.ChangeDirection(DirectionEnum.LEFT);

            if (e.KeyCode == Keys.Right)
                AirShip.ChangeDirection(DirectionEnum.RIGHT);
        }

        private void EventUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                AirShip.ChangeDirection(DirectionEnum.STOP);
        }

        private void EventSpace(object sender, KeyEventArgs e)
        {
            if (AirShip.CanShoot)
                AirShip.Shoot(_controls);
        }

        private void EventRestart(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            //Restart(repository.Get());
            _timer.Interval = 500;
            _timer.Start();
        }

        public void Restart(Gamer newGamer)
        {
            AirShip = newGamer.AirShip;
            Walls = newGamer.Walls;
            Enemies = newGamer.Enemies;
            Score = newGamer.Score;

            _controls.Clear();

            _controls.Add(AirShip.ElementScreen());

            Walls.ForEach(wall =>
            {
                foreach (var shield in wall.Shields)
                {
                    if (shield.IsLive == true)
                    {
                        _controls.Add(shield.ElementScreen());
                    }
                }
            });

            Enemies.ForEach(enemy =>
            {
                if (enemy.IsLive == true)
                {
                    _controls.Add(enemy.ElementScreen());
                };
            });
        }

        public Gamer Clone()
        {
            var airShip = ScreenFactory.CloneElment<AirShip>(AirShip);
            airShip.ChangeDirection(DirectionEnum.STOP);

            var walls = new List<Wall>();
            Walls.ForEach(wall =>
            {
                var shields = new List<Shield>();
                foreach (var shield in wall.Shields)
                {
                    shields.Add(ScreenFactory.CloneElment<Shield>(shield));
                }
                walls.Add(new Wall(shields));
            });
            var enemies = new List<Enemy>();
            Enemies.ForEach(enemy =>
            {
                enemies.Add(ScreenFactory.CloneElment<Enemy>(enemy));
            });

            return new Gamer(_controls, airShip, walls, enemies);
        }
    }
}
