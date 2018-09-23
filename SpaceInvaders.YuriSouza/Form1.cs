using SpaceInvaders.YuriSouza.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza
{
    public partial class Form1 : Form
    {
        private bool goLeft, goRight, isPressed, goLeftInvader, goRightInvader = true, canShoot = true;
        private int invaderSpeed = 5, score = 0, totalEnemies = 15, playerSpeed = 5, invaderToMove = 15, totalShoot = 0;
        private int[] invadersLive;
        private Inimigo[] inimigos;
        private Random random;

        public Form1()
        {
            random = new Random();
            inimigos = new Inimigo[15];
            InitializeInimigos();
            invadersLive = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            InitializeComponent();
        }

        private void InitializeInimigos()
        {
            for(int i=0; i < totalEnemies; i++)
            {
                inimigos[i] = new Inimigo(i);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            makeShield();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = true;

            if (e.KeyCode == Keys.Right)
                goRight = true;

            if (e.KeyCode == Keys.Space && !isPressed)
            {
                isPressed = true;
                
                makeBullet();
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = false;

            if (e.KeyCode == Keys.Right)
                goRight = false;

            if (isPressed)
                isPressed = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (goLeft && player.Left > 0)
                player.Left -= playerSpeed;

            if (goRight && player.Left < 470)
                player.Left += playerSpeed;

            foreach (Control control in Controls)
            {
                if (control is PictureBox && control.Tag == "invader" && invaderToMove.ToString() == control.AccessibleName)
                {
                    if (goLeftInvader)
                        control.Left -= invaderSpeed;
                    else if (goRightInvader)
                        control.Left += invaderSpeed;

                    if (invaderToMove >= 15)
                        invaderToMove = 1;
                    else
                        invaderToMove++;

                    if (control.Left >= 470)
                    {
                        control.Left -= invaderSpeed;
                        goLeftInvader = true;
                        goRightInvader = false;
                        invaderToMove = 1;
                    }
                    else if (control.Left < 0)
                    {
                        control.Left += invaderSpeed;
                        goLeftInvader = false;
                        goRightInvader = true;
                        invaderToMove = 15;
                    }

                    //tiro dos fantasmas
                    //var inimigo = inimigos[random.Next(0, 15)];
                    //var valor = random.Next(1, 15) >= random.Next(1, 10);
                    //if (inimigo.CanShoot && inimigo.Id.ToString() == control.AccessibleName && valor && totalShoot < 5)
                    //{
                    //    totalShoot++;
                    //    inimigo.CanShoot = false;
                    //    makeBulletInvader(control);
                    //}
                }
                else if (invadersLive.Contains(invaderToMove) == false)
                {
                    if (invaderToMove >= 15)
                        invaderToMove = 1;
                    else
                        invaderToMove++;
                }

            }

            //TIRO DOS FANTASMAS
            foreach (Control control in Controls)
            {
                if (control is PictureBox && control.Tag == "invader")
                {
                    var inimigo = inimigos[random.Next(0, 15)];
                    var valor = random.Next(1, 15) >= random.Next(1, 10);
                    if (inimigo.CanShoot && inimigo.Id.ToString() == control.AccessibleName && valor && totalShoot < 5)
                    {
                        totalShoot++;
                        inimigo.CanShoot = false;
                        makeBulletInvader(control);
                    }
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
                        MessageBox.Show("You LOSER");
                    }

                    control.Top += 3;

                    if (control.Top > 380)
                    {
                        Controls.Remove(control);
                        totalShoot--;
                        inimigos[Convert.ToInt32(control.AccessibleName)].CanShoot = true;
                    }
                }

                
                foreach (Control controlInternal in Controls)
                {
                    //COLISÃO TIRO DA NAVE COM O INIMIGO
                    if (control is PictureBox && control.Tag.ToString() == "invader")
                    {
                        if (controlInternal is PictureBox && controlInternal.Tag.ToString() == "bullet")
                        {
                            if (control.Bounds.IntersectsWith(controlInternal.Bounds))
                            {
                                score++;
                                Controls.Remove(control);
                                Controls.Remove(controlInternal);
                                invadersLive = invadersLive.Where(a => a != Convert.ToInt32(control.AccessibleName)).ToArray();
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
                                inimigos[Convert.ToInt32(controlInternal.AccessibleName)].CanShoot = true;
                            }

                            
                        }

                        if (controlInternal is PictureBox && controlInternal.Tag.ToString() == "bullet")
                        {
                            if (control.Bounds.IntersectsWith(controlInternal.Bounds))
                            {
                                Controls.Remove(control);
                                Controls.Remove(controlInternal);
                            }
                        }
                    }
                }

                label1.Text = $"Score: {score}";

                if (score > totalEnemies - 1)
                {
                    gameOver();
                    MessageBox.Show("You Win");
                }
            }
        }

        private void makeBulletInvader()
        {
            PictureBox bullet = new PictureBox()
            {
                Image = Properties.Resources.bullet,
                Size = new Size(5, 20),
                Tag = "new",
                Left = 351,
                Top = 20
            };

            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void makeBulletInvader(Control control)
        {
            PictureBox bullet = new PictureBox()
            {
                Image = Properties.Resources.bullet,
                Size = new Size(5, 20),
                Tag = "invaderBullet",
                Left = control.Left + control.Width / 2,
                Top = control.Top + 20,
                AccessibleName = control.AccessibleName
            };

            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void makeBullet()
        {
            PictureBox bullet = new PictureBox()
            {
                Image = Properties.Resources.bullet,
                Size = new Size(5, 20),
                Tag = "bullet",
                Left = player.Left + player.Width / 2,
                Top = player.Top - 20
            };

            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void makeShield()
        {
            var space = 0;
            for(int shields = 1; shields <= 4; shields++)
            {
                for (int shieldCollumn = 0; shieldCollumn < 10; shieldCollumn++)
                {
                    PictureBox bullet = new PictureBox()
                    {
                        Image = Properties.Resources.shield,
                        Size = new Size(6, 8),
                        Tag = "shield",
                        Left = (shields*45) + space + (shieldCollumn*6),
                        Top = 280
                    };

                    this.Controls.Add(bullet);
                    bullet.BringToFront();

                    for (int shieldLine = 1; shieldLine <= 4; shieldLine++)
                    {
                        PictureBox bullet1 = new PictureBox()
                        {
                            Image = Properties.Resources.shield,
                            Size = new Size(6, 8),
                            Tag = "shield",
                            Left = (shields * 45) + space + (shieldCollumn * 6),
                            Top = 280 - (shieldLine * 8)
                        };

                        this.Controls.Add(bullet1);
                        bullet1.BringToFront();
                    }
                }
                space += 70;
            }
        }


        private void gameOver()
        {
            timer1.Stop();
            label1.Text += " Game Over";
        }
    }
}
