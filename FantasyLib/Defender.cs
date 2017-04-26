﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLib
{
    public class Defender : Player
    {
        public int Score
        {
            get
            {
                return 3 * Stat.Assists - 2 * Stat.PenaltyMiss - Stat.YellowCard - 3 * Stat.RedCard - 2 * Stat.OwnGoal + 5 * Stat.CleanSheet + 6 * Stat.Goals - Stat.GoalsConc;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} - защитник - {1} очков", Surname, Score);
        }
    }
}
