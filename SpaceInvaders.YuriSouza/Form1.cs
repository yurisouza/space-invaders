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
        private int invaderSpeed = 5, score = 0, totalEnemies = 15, playerSpeed = 7, invaderToMove = 15;
        private int[] invadersLive;

        public Form1()
        {

            invadersLive = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                if (control is PictureBox && control.Tag == "invader" && canShoot)
                {
                    canShoot = false;
                    makeBulletInvader(control);
                }
            }

            foreach (Control control in Controls)
            {

                //movimento tiro da nave
                if (control is PictureBox && control.Tag == "bullet")
                {
                    control.Top -= 20;

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
                    }

                    control.Top += 4;

                    if (control.Top > 380)
                    {
                        Controls.Remove(control);
                        canShoot = true;
                    }
                }

                //NAVE ATIRANDO
                foreach (Control controlInternal in Controls)
                {
                    if (control is PictureBox && control.Tag == "invader")
                    {
                        if (controlInternal is PictureBox && controlInternal.Tag == "bullet")
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
                Top = control.Top + 20
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


        private void gameOver()
        {
            timer1.Stop();
            label1.Text += " Game Over";
            MessageBox.Show("You LOSER");
        }
    }
}
