
using SpaceInvaders.YuriSouza.Repository;
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
        private ListView _listScores;
        private Timer _timer;
        private ControlCollection _controls;
        private Random _random;
        private IRepository _repository;
        private int _totalShots = 0;

        public int Score { get; set; }
        public AirShip AirShip { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Enemy> Enemies { get; set; }

        public Gamer()
        {
            _repository = new RepositoryMemory();
            _random = new Random();
        }

        public Gamer(ControlCollection controls, Control player, Timer timer, ListView listScores)
        {
            _controls = controls;
            _player = player;
            _timer = timer;
            _listScores = listScores;
            BuildListScores();
            _repository = new RepositoryMemory();
            _random = new Random();

        }

        private void BuildListScores()
        {
            _listScores.Columns.Add("Posição", 50);
            _listScores.Columns.Add("Pontuação", 150);
            _listScores.View = View.Details;
            _listScores.GridLines = true;
            _listScores.FullRowSelect = true;
        }

        public void LoadGamer()
        {
            _listScores.Hide();
            ClearScreen();
            BuildAirShip();
            BuildWalls();
            BuildEnemies();
        }

        private void BuildEnemies(int positionStartTop = 150)
        {
            Enemies = new List<Enemy>(Variables.EnemiesPerLine * Variables.EnemiesPerCollumn);
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
            Walls = new List<Wall>(Variables.TotalWalls);
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
            AirShip = new AirShip(new ElementControl(new ControlImplementation(_player), DirectionEnum.STOP));
        }

        public void EventHandler(EventActionEnum eventAction, object sender, KeyEventArgs e)
        {
            if (eventAction == EventActionEnum.DOWN)
                EventDown(sender, e);
            else if (eventAction == EventActionEnum.UP)
                EventUp(sender, e);
            else if (eventAction == EventActionEnum.SCORE)
                EventScore(sender, e);
            else if (eventAction == EventActionEnum.SPACE)
                EventSpace(sender, e);
            else
                EventRestart(sender, e);
        }

        private void EventScore(object sender, KeyEventArgs e)
        {
            if (_listScores.Visible)
            {
                _listScores.Hide();
                _listScores.Items.Clear();
            }
            else
            {
                var scores = _repository.Get();
                if (scores != null)
                {

                    scores.ConvertAll<string>(x => x.ToString()).OrderByDescending(i => i).ToList().ForEach(item =>
                    {
                        string[] scoreView = new string[2];
                        scoreView[0] = (scores.IndexOf(Convert.ToInt32(item)) + 1).ToString();
                        scoreView[1] = item;
                        var score = new ListViewItem(scoreView);
                        _listScores.Items.Add(score);
                    });

                    _listScores.Show();
                }
            }
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
            if (Keys.R == e.KeyCode)
                _timer.Stop();

            if (Keys.D1 == e.KeyCode)
            {
                Restart(_repository.Get(1));
                _timer.Start();
            }
            else if (Keys.D2 == e.KeyCode)
            {
                Restart(_repository.Get(2));
                _timer.Start();
            }
            else if (Keys.D3 == e.KeyCode)
            {
                Restart(_repository.Get(3));
                _timer.Start();
            }
            else if (Keys.D4 == e.KeyCode)
            {
                Restart(_repository.Get(4));
                _timer.Start();
            }
            else if (Keys.D5 == e.KeyCode)
            {
                Restart(_repository.Get(5));
                _timer.Start();
            }
        }

        public void Restart(Gamer newGamer)
        {
            if (newGamer != null)
            {
                AirShip = newGamer.AirShip;
                Walls = newGamer.Walls;
                Enemies = newGamer.Enemies;
                Score = newGamer.Score;

                ClearScreen(true);

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
        }

        public void GamerManager()
        {
            MoveAirShip();
            MoveEnemies();
            EnemiesShots();

            foreach (Control control in _controls)
            {
                MoveShotOfAirShip(control);
                MoveShotOfEnemy(control);
                VerifyColisions(control);
                UpdateScore(control);
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
                var indexMultiply = (int)Math.Floor(Convert.ToDecimal((totalEnemies - enemiesLives.Count()) / 2));
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
            if (control is PictureBox && control.Tag == Variables.ShotOfAirship)
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
            if (control is PictureBox && control.Tag == Variables.ShotOfEnemy)
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

        private void VerifyColisions(Control control)
        {
            VerifyColisionWithEnemies(control);
            VerifyColisionWithShields(control);
            VerifyColisionBetweenShots(control);
        }

        private void VerifyColisionWithShields(Control shot)
        {
            Walls.ForEach(wall =>
            {
                foreach (var shield in wall.Shields)
                {
                    if ((shot.Tag == Variables.ShotOfEnemy || shot.Tag == Variables.ShotOfAirship) && shield.IsLive && shot.Bounds.IntersectsWith(shield.ElementScreen().Bounds))
                    {
                        _controls.Remove(shot);
                        _controls.Remove(shield.ElementScreen());
                        shield.IsLive = false;

                        if (shot.Tag == Variables.ShotOfEnemy)
                            _totalShots--;
                    }
                }
            });
        }

        private void VerifyColisionWithEnemies(Control shotOfAirShip)
        {
            foreach (var enemy in Enemies)
            {
                if (shotOfAirShip.Tag == Variables.ShotOfAirship && enemy.IsLive && shotOfAirShip.Bounds.IntersectsWith(enemy.ElementScreen().Bounds))
                {
                    _repository.Insert(this);

                    Score++;
                    _controls.Remove(shotOfAirShip);
                    _controls.Remove(enemy.ElementScreen());
                    enemy.IsLive = false;

                    var survivors = Enemies.Count(e => e.IsLive);
                    if (survivors == 0)
                        NextLevelEnemy(enemy.ElementScreen().Top + 45);
                    else if (survivors % 9 == 0)
                        Enemies.ForEach(e => e.IncreaseSpeed());
                }

                if (enemy.IsLive)
                    VerifyColisionWithAirShip(enemy.ElementScreen());

            };
        }

        private void VerifyColisionWithAirShip(Control control)
        {
            if (control.Bounds.IntersectsWith(_player.Bounds))
            {
                GamerOver();
            }
        }

        private void VerifyColisionBetweenShots(Control shotOfEnemy)
        {
            if (shotOfEnemy.Tag == Variables.ShotOfEnemy)
            {
                VerifyColisionWithAirShip(shotOfEnemy);

                foreach (Control shotAirShip in _controls)
                {
                    if (shotAirShip.Tag == Variables.ShotOfAirship && shotOfEnemy.Bounds.IntersectsWith(shotAirShip.Bounds))
                    {
                        _controls.Remove(shotOfEnemy);
                        _controls.Remove(shotAirShip);
                    }
                }
            }
        }

        private void NextLevelEnemy(int positionStartTop)
        {
            BuildEnemies(positionStartTop);
        }

        public void UpdateScore(Control control)
        {
            control.Text = $"Score: {Score}";
        }

        private void ClearScreen(bool removeAirShip = false)
        {
            if (Enemies != null)
            {
                Enemies.ForEach(enemy =>
                {
                    _controls.Remove(enemy.ElementScreen());
                });
            }

            if (Walls != null)
            {
                Walls.ForEach(wall =>
                {
                    foreach (var shield in wall.Shields)
                    {
                        _controls.Remove(shield.ElementScreen());
                    }
                });
            }


            foreach (Control control in _controls)
            {
                if (control.Tag == Variables.BorderName || control.Tag == Variables.ScoreName || control.Tag == Variables.ListScoresName)
                    continue;

                if (control == _player && removeAirShip == false)
                    continue;

                _controls.Remove(control);
            }
        }

        private void GamerOver()
        {
            _timer.Stop();
            _repository.Insert(Score);
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

            return new Gamer(_controls, _player, _timer, _listScores) { AirShip = airShip, Walls = walls, Enemies = enemies, Score = Score };
        }
    }
}
