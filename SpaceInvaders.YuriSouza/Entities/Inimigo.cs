using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Entities
{
    public class Inimigo
    {
        public int Id { get; set; }
        public bool CanShoot { get; set; }
        public bool IsLive { get; set; }

        public Inimigo(int id)
        {
            Id = id;
            CanShoot = true;
            IsLive = true;
        }
    }
}
