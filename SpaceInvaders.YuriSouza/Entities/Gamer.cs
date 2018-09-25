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
        private Random _random;
        private int _totalShots = 0;

        public int Score { get; set; }
        public AirShip AirShip { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Enemy> Enemies { get; set; }

        public Gamer(ControlCollection controls, Control player, Timer timer)
        {
            _controls = controls;
            _player = player;
            _timer = timer;
            _random = new Random();
            Walls = new List<Wall>(Variables.TotalWalls);
            Enemies = new List<Enemy>(Variables.EnemiesPerLine * Variables.EnemiesPerCollumn);
            LoadGamer();
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

                Walls.Add(wall);
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
            //_timer.Stop();
            ////Restart(repository.Get());
            //_timer.Interval = 500;
            //_timer.Start();
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

        public void GamerManager()
        {
            MoveAirShip();
            MoveEnemies();
            EnemiesShots();

            foreach(Control control in _controls)
            {
                MoveShotOfAirShip(control);
                MoveShotOfEnemy(control);
                VerifyColisionShotsAirShipWithEnemies(control);
            }
        }

        private void MoveAirShip()
        {
            if (AirShip.CanMoveToLeft())
                AirShip.MoveToLeft();

            if (AirShip.CanMoveToRight())
                AirShip.MoveToRight();
        }

        private void MoveEnemies()
        {
            MoveEnemyToRight();
            MoveEnemyToLeft();
        }

        private void MoveEnemyToRight()
        {
            for (var enemyId = Enemies.Last().Id; enemyId >= 0; enemyId--)
            {
                var enemy = Enemies[enemyId];

                if (enemy.IsLive == false)
                    continue;
                else if (enemy.CanMoveToRight())
                {
                    enemy.MoveToRight();
                    VerifyColisionWithAirShip(enemy.ElementScreen());
                }
                else if (enemy.GetDirection() == DirectionEnum.RIGHT)
                {
                    foreach (var i in Enemies)
                    {
                        i.ChangeDirection(DirectionEnum.LEFT);
                        i.MoveToDown();
                    }
                    break;
                }
            }
        }

        private void EnemiesShots()
        {
            var enemiesLives = Enemies.Where(enemy => enemy.IsLive == true);
            foreach (var enemy in enemiesLives)
            {
                var totalEnemies = Variables.EnemiesPerLine * Variables.EnemiesPerCollumn;
                var indexMultiply = (int) Math.Floor(Convert.ToDecimal((totalEnemies - enemiesLives.Count()) / 2));
                var probability = _random.Next(0, 28 * indexMultiply) >= _random.Next(1, 2800);
                if (enemy.CanShoot && probability && _totalShots < Variables.ShotsEnemiesInScreen)
                {
                    _totalShots++;
                    enemy.Shoot(_controls);
                }
            }
        }

        private void MoveEnemyToLeft()
        {
            var totalEnemies = Variables.EnemiesPerCollumn * Variables.EnemiesPerLine;
            for (var enemyId = Enemies.First().Id; enemyId < totalEnemies; enemyId++)
            {
                var enemy = Enemies[enemyId];

                if (enemy.IsLive == false)
                    continue;
                else if (enemy.CanMoveToLeft())
                {
                    enemy.MoveToLeft();
                    VerifyColisionWithAirShip(enemy.ElementScreen());
                }
                else if (enemy.GetDirection() == DirectionEnum.LEFT)
                {
                    foreach (var i in Enemies)
                    {
                        i.ChangeDirection(DirectionEnum.RIGHT);
                        i.MoveToDown();
                    }
                    break;
                }
            }
        }

        private void MoveShotOfAirShip(Control control)
        {
            if (control is PictureBox && control.Tag == Variables.ShootNameAirShip)
            {
                control.Top -= Variables.AirShipShotSpeed;

                if (control.Top < control.Height - 490)
                {
                    _controls.Remove(control);
                }
            }
        }

        private void MoveShotOfEnemy(Control control)
        {
            if (control is PictureBox && control.Tag == Variables.ShootNameEnemy)
            {
                //VerifyColisionWithAirShip(control);

                control.Top += Variables.EnemyShotSpeed;

                if (control.Top > 380)
                {
                    _controls.Remove(control);
                    _totalShots--;
                }
            }
        }

        private void VerifyColisionShotsAirShipWithEnemies(Control control)
        {
            //VERIFICAR COLISÃO COM O TIRO...
        }

        private void VerifyColisionWithAirShip(Control control)
        {
            if (control.Bounds.IntersectsWith(_player.Bounds))
            {
                GamerOver();
                //_controls.Remove(control);
            }
        }

        public void NextLevelEnemy(int positionStartTop)
        {
            BuildEnemies(positionStartTop);
        }

        private void GamerOver()
        {
            _timer.Stop();
            MessageBox.Show("You LOSE");
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
                walls.Add(new Wall(wall.Id, shields));
            });
            var enemies = new List<Enemy>();
            Enemies.ForEach(enemy =>
            {
                enemies.Add(ScreenFactory.CloneElment<Enemy>(enemy));
            });

            return new Gamer(_controls, _player, _timer);
        }
    }
}
