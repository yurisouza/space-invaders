using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class Shield : Element
    {
        public int Id { get; set; }
        public int WallId { get; set; }
        public bool IsLive { get; set; }

        public Shield(ElementControl controle)
        {
            _controle = controle;
            IsLive = true;
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
