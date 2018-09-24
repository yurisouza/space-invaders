using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class Shield : Element
    {
        public Shield(ElementControl controle)
        {
            _controle = controle;
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
