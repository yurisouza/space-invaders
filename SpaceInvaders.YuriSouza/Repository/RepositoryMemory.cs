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
        private Dictionary<int, Gamer> _gamers;
        private List<int> _scores;

        public RepositoryMemory()
        {
            _scores = new List<int>();
            _gamers = new Dictionary<int, Gamer>();
        }

        public Gamer Get(int position)
        {
            for (int i = 1; i <= position; i++)
            {
                Gamer gamer;
                _gamers.TryGetValue(position - i, out gamer);

                if (gamer != null)
                    return gamer;
            }

            return null;
        }

        public void Insert(Gamer gamer)
        {
            var gamersTemp = new Dictionary<int, Gamer>();

            gamersTemp.Add(0, gamer.Clone());

            for (int i = 0; i < 4; i++)
            {
                Gamer gamerTemp;
                _gamers.TryGetValue(i, out gamerTemp);

                if (gamerTemp == null)
                    break;
                else
                    gamersTemp.Add(i+1, gamerTemp);
            }

            _gamers = gamersTemp;
        }

        public void Insert(int score)
        {
            _scores.Add(score);
        }

        public List<int> Get()
        {
            return _scores.OrderByDescending(i => i).Take(10).ToList();
        }
    }
}
