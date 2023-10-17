using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBASchedule
{
    public class Match
    {
        public Team homeTeam { get; set; }
        public Team awayTeam { get; set; }
        public Match(Team h, Team a)
        {
            homeTeam = h;
            awayTeam = a;
        }
    }
}
