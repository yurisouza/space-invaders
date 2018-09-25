using SpaceInvaders.YuriSouza.Entities;
using SpaceInvaders.YuriSouza.Repository;
using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza
{
    public partial class SpaceInvaderScreen : Form
    {
        private bool goLeft, goRight, isPressed, goLeftInvader, goRightInvader = true;
        private int invaderSpeed = 7, totalEnemies = 0, playerSpeed = 5, invaderToMove = 14, totalShoot = 0;
        private Enemy[] enemies;
        private Wall[] walls;
        private Random random;
        private AirShip airShip;
        private Gamer gamer;
        private IRepository repository;

        public SpaceInvaderScreen()
        {
            repository = new RepositoryMemory();
            random = new Random();
            InitializeComponent();
        }

        //private void InitializeInimigos(int baseStart)
        //{
        //    enemies = new Enemy[Variables.TotalEnemies];
        //    totalEnemies = 0;

        //    var left = 78;
        //    for (int coluna = 0; coluna < 7; coluna++)
        //    {
        //        left += 42;
        //        var top = baseStart < 150 ? 150 : baseStart;
        //        for (int line = 0; line < 4; line++)
        //        {
        //            top -= 35;

        //            var inimigo = ScreenFactory.NewEnemy(totalEnemies.ToString(), left, top);
        //            enemies[inimigo.Id] = inimigo;

        //            this.Controls.Add(inimigo.ElementScreen());
        //            totalEnemies++;
        //        }
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            //MakeWalls();
            //InitializeInimigos(150);
            //airShip = new AirShip(new ElementControl(player, DirectionEnum.STOP));
            gamer = new Gamer(Controls, player, timer1);
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                gamer.EventHandler(EventActionEnum.DOWN, sender, e);
            else if (e.KeyCode == Keys.Space)
                gamer.EventHandler(EventActionEnum.SPACE, sender, e);
            else
                gamer.EventHandler(EventActionEnum.RESTART, sender, e);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            gamer.EventHandler(EventActionEnum.UP, sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (airShip.CanMoveToLeft())
                airShip.MoveToLeft();

            if (airShip.CanMoveToRight())
                airShip.MoveToRight();


            //MOVER INIMIGOS PARA DIREITA
            for (var inimigoId = enemies.Last().Id; inimigoId >= 0; inimigoId--)
            {
                var inimigo = enemies[inimigoId];

                if (inimigo.IsLive == false)
                    continue;
                else if (inimigo.CanMoveToRight())
                    inimigo.MoveToRight();
                else if (inimigo.GetDirection() == DirectionEnum.RIGHT)
                {
                    foreach (var i in enemies)
                    {
                        i.ChangeDirection(DirectionEnum.LEFT);
                        i.MoveToDown();
                    }
                    break;
                }
            }

            //MOVER INIMIGOS PARA ESQUERDA
            for (var inimigoId = enemies.First().Id; inimigoId < Variables.TotalEnemies; inimigoId++)
            {
                var inimigo = enemies[inimigoId];

                if (inimigo.IsLive == false)
                    continue;
                else if (inimigo.CanMoveToLeft())
                    inimigo.MoveToLeft();
                else if (inimigo.GetDirection() == DirectionEnum.LEFT)
                {
                    foreach (var i in enemies)
                    {
                        i.ChangeDirection(DirectionEnum.RIGHT);
                        i.MoveToDown();
                    }
                    break;
                }
            }

            //TIRO INIMIGOS
            var enemiesLives = enemies.Where(enemy => enemy.IsLive == true);
            foreach (var enemy in enemiesLives)
            {
                var probability = random.Next(0, 28 * (int)Math.Floor(Convert.ToDecimal((Variables.TotalEnemies - enemiesLives.Count()) / 2))) >= random.Next(1, 2800);
                if (enemy.CanShoot && probability && totalShoot <= 3)
                {
                    totalShoot++;
                    enemy.Shoot(Controls);
                }
            }

            foreach (Control control in Controls)
            {

                //movimento tiro da nave
                if (control is PictureBox && control.Tag == "bullet")
                {
                    control.Top -= 6;

                    if (control.Top < Height - 490)
                    {
                        Controls.Remove(control);
                    }
                }

                //movimento tiro dos fantasmas
                if (control is PictureBox && control.Tag == "invaderBullet")
                {
                    if (control.Bounds.IntersectsWith(player.Bounds))
                    {
                        Controls.Remove(control);
                        gameOver();
                        MessageBox.Show("You LOSE");
                    }

                    control.Top += 3;

                    if (control.Top > 380)
                    {
                        Controls.Remove(control);
                        //enemies[Convert.ToInt32(control.AccessibleName)].CanShoot = true;
                        totalShoot--;
                    }
                }


                //ENEMY TOCAR NA NAVE
                if (control is PictureBox && control.Tag.ToString() == "invader")
                {
                    if (control.Bounds.IntersectsWith((Rectangle)airShip.ElementScreen().Bounds))
                    {
                        gameOver();
                        MessageBox.Show("You LOSER");
                    }
                }


                foreach (System.Windows.Forms.Control controlInternal in Controls)
                {
                    //COLISÃO TIRO DA NAVE COM O INIMIGO
                    if (control is PictureBox && control.Tag.ToString() == "invader")
                    {
                        if (controlInternal is PictureBox && controlInternal.Tag.ToString() == "bullet")
                        {
                            if (control.Bounds.IntersectsWith(controlInternal.Bounds))
                            {
                                gamer.Score++;
                                Controls.Remove(control);
                                Controls.Remove(controlInternal);

                                var ini = enemies[Convert.ToInt32(control.AccessibleName)];
                                ini.IsLive = false;

                                var inimigosVivos = enemies.Count(inimigo => inimigo.IsLive == true);

                                if (inimigosVivos == 0)
                                {
                                    InitializeInimigos((int)(ini.ElementScreen().Top + 45));
                                }
                                else if (inimigosVivos % 9 == 0)
                                {
                                    foreach (var i in enemies)
                                    {
                                        i.IncreaseSpeed();
                                    }
                                }

                                repository.Insert(gamer);
                            }
                        }



                    }

                    if (control is PictureBox && control.Tag.ToString() == "shield")
                    {
                        if (controlInternal is PictureBox && controlInternal.Tag.ToString() == "invaderBullet")
                        {
                            if (control.Bounds.IntersectsWith(controlInternal.Bounds))
                            {
                                Controls.Remove(control);
                                Controls.Remove(controlInternal);
                                totalShoot--;
                                //enemies[Convert.ToInt32(controlInternal.AccessibleName)].CanShoot = true;
                            }


                        }

                        if (controlInternal is PictureBox && controlInternal.Tag.ToString() == "bullet")
                        {
                            if (control.Bounds.IntersectsWith(controlInternal.Bounds))
                            {
                                var ids = control.AccessibleName.Split('/');
                                walls[Convert.ToInt32(ids[0])].Shield(ids[1]).IsLive = false;
                                Controls.Remove(control);
                                Controls.Remove(controlInternal);
                            }
                        }
                    }
                }

                label1.Text = $"Score: {gamer.Score}";

                //if (score > totalEnemies - 1)
                //{
                //    gameOver();
                //    MessageBox.Show("You Win");
                //}
            }
        }



        //private void MakeWalls()
        //{
        //    walls = new Wall[Variables.TotalWalls];
        //    var shieldId = 0;
        //    var intervalBetweenWalls = 0;
        //    for (int wallId = 0; wallId < Variables.TotalWalls; wallId++)
        //    {
        //        var wall = ScreenFactory.NewWall((39 * (wallId + 1)) + intervalBetweenWalls, 288);
        //        walls[wallId] = wall;
        //        foreach(var shield in wall.Shields)
        //        {
        //            shield.Id = shieldId++;
        //            shield.WallId = wallId;
        //            shield.ElementScreen().AccessibleName = $"{wallId}/{shield.Id}";
        //            Controls.Add(shield.ElementScreen());
        //        }
        //        intervalBetweenWalls += 70;
        //    }
        //}


        private void gameOver()
        {
            timer1.Stop();
            label1.Text += " Game Over";
        }
    }
}
