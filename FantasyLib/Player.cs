using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLib
{
    [Serializable]
    public abstract class Player
    {
        public string Surname { get; set; }
        public double Price { get; set; }
        public Statistics Stat { get; set; }
    }
}
