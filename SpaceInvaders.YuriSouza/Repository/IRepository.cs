using SpaceInvaders.YuriSouza.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Repository
{
    public interface IRepository
    {
        void Insert(Gamer game);
        Gamer Get();
    }
}
