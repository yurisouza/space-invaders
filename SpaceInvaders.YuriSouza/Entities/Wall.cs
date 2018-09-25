using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    [Serializable]
    public class Wall : Element
    {
        public int Id { get; set; }
        public IList<Shield> Shields { get; set; }

        public Wall(int id, IList<Shield> shields)
        {
            Shields = shields;
        }

        public Shield Shield(string id)
        {
            return Shields.FirstOrDefault(s => s.Id == Convert.ToInt32(id));
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
