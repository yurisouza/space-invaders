using Newtonsoft.Json;
using SpaceInvaders.YuriSouza.Entities;
using SpaceInvaders.YuriSouza.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.YuriSouza.Repository
{
    public class RepositoryMemory : IRepository
    {
        public List<Gamer> Gamers { get; set; }

        public RepositoryMemory()
        {
            Gamers = new List<Gamer>();
        }

        public Gamer Get()
        {
            return Gamers.LastOrDefault();
        }

        public void Insert(Gamer gamer)
        {
            Gamers.Add(gamer.Clone());
        }

        
    }
}
