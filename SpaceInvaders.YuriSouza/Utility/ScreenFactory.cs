using SpaceInvaders.YuriSouza.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza.Utility
{
    public class ScreenFactory
    {
        public static Enemy NewEnemy(string name, int left, int top)
        {
            var inimigoScreen = new PictureBox()
            {
                Image = Properties.Resources.Space_invader,
                Size = new Size(Variables.EnemyWidth, Variables.EnemyHeight),
                Tag = Variables.EnemyName,
                Left = left,
                Top = top,
                SizeMode = PictureBoxSizeMode.StretchImage,
                AccessibleName = name
            };

            inimigoScreen.BringToFront();

            return new Enemy(new ElementControl(new ControlImplementation(inimigoScreen)));
        }

        public static Shield NewShield(int left, int top)
        {
            var shield = new PictureBox()
            {
                Image = Properties.Resources.shield,
                Size = new Size(Variables.ShieldWidth, Variables.ShieldHeight),
                Tag = Variables.ShieldName,
                Left = left,
                Top = top
            };

            shield.BringToFront();

            return new Shield(new ElementControl(new ControlImplementation(shield)));
        }

        public static Wall NewWall(int id, int left, int top)
        {
            var shields = new List<Shield>();

            for (int collumn = 1; collumn <= Variables.TotalCollumnInWall; collumn++)
            {
                for (int line = 1; line <= Variables.TotalLinesInWall; line++)
                {
                    var shield = NewShield(left + (collumn * Variables.ShieldWidth), top - (line * Variables.ShieldHeight));
                    shields.Add(shield);
                }
            }

            return new Wall(id, shields);
        }

        public static Control NewShoot(string name, ElementControl elementControl)
        {
            var shoot = new PictureBox()
            {
                Image = Properties.Resources.bullet,
                Size = new Size(Variables.ShootWidth, Variables.ShootHeight),
                Tag = name,
                Left = elementControl.ElementScreen().Left + elementControl.ElementScreen().Width / 2,
                Top = elementControl.ElementScreen().Top + 20
            };
            
            shoot.BringToFront();
            return shoot;
        }

        public static T CloneElment<T>(Element element)
        {
            return (T) element.Clone();
        }
    }
}
