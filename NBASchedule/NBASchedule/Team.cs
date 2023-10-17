using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBASchedule
{
    public class Team
    {
        public string Name { get; set; }
        public string Division { get; set; }    
        public string Conference { get; set; }

        public int gamesAgainstConf { get; set; }
        public int gamesAgainstDiv { get; set; }
        public int gamesAgainstOtherConf { get; set; }

        public int maxB2B { get; set; }

        public Team(string name, string div, string conf)
        {
            Name = name; 
            Division = div;
            Conference = conf;
            gamesAgainstConf = 8;
            gamesAgainstDiv = 3;
            gamesAgainstOtherConf = 6;
            maxB2B = 3;
        }

    }
}
