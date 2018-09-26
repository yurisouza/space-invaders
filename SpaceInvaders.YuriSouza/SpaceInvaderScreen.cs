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
        private Gamer gamer;

        public SpaceInvaderScreen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listScores.Hide();
            timer1.Stop();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                NewGamer_Click();

            if (gamer != null)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
                    gamer.EventHandler(EventActionEnum.DOWN, sender, e);
                if (e.KeyCode == Keys.S)
                    gamer.EventHandler(EventActionEnum.SCORE, sender, e);
                else if (e.KeyCode == Keys.Space)
                    gamer.EventHandler(EventActionEnum.SPACE, sender, e);
                else
                    gamer.EventHandler(EventActionEnum.RESTART, sender, e);
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (gamer != null)
                gamer.EventHandler(EventActionEnum.UP, sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gamer.GamerManager();
            gamer.UpdateScore(scoreScreen);
        }

        private void NewGamer_Click()
        {
            timer1.Stop();
            gamer = new Gamer(Controls, player, timer1, listScores);
            gamer.LoadGamer();
            timer1.Start();
        }
    }
}
