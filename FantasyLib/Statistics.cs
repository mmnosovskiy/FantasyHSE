using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLib
{
    [Serializable]
    public class Statistics
    {
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int PenaltyMiss { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public int OwnGoal { get; set; }
        public int CleanSheet { get; set; }
        public int GoalsConc { get; set; }
        public int PenaltySave { get; set; }
    }
}
