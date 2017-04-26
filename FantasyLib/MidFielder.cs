using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLib
{
    public class MidFielder : Player
    {
        public int Score
        {
            get
            {
                return  3 * Stat.Assists - 2 * Stat.PenaltyMiss - Stat.YellowCard - 3 * Stat.RedCard - 2 * Stat.OwnGoal + Stat.CleanSheet + 5 * Stat.Goals;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} - полузащитник - {1} очков", Surname, Score);
        }
    }
}
