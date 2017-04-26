using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLib
{
    [Serializable]
    public class Goalkeeper : Player
    {
        public int Score
        {
            get
            {
                return 3 * Stat.Assists - 2 * Stat.PenaltyMiss - Stat.YellowCard - 3 * Stat.RedCard - 2 * Stat.OwnGoal + 6 * Stat.CleanSheet + 4 * Stat.PenaltySave + 10 * Stat.Goals - Stat.GoalsConc; 
            }
        }
        public override string ToString()
        {
            return string.Format("{0} - вратарь - {1} очков", Surname, Score);
        }
    }
}
