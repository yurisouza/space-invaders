using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class Wall : Element
    {
        public IList<Shield> Shields { get; set; }

        public Wall(IList<Shield> shields)
        {
            Shields = shields;
        }

        public override void MoveToLeft()
        {
            throw new NotImplementedException();
        }

        public override void MoveToRight()
        {
            throw new NotImplementedException();
        }
    }
}
