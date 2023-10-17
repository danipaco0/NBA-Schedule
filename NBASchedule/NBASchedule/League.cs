using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBASchedule
{
    public class League
    {
        string[] names = { "Boston Celtics", "Brooklyn Nets","Chicago Bulls", "Milwaukee Bucks", "Miami Heat", "Atlanta Hawks",
                           "Denver Nuggets", "Utah Jazz", "Los Angeles Lakers", "Golden State Warriors",  "San Antonio Spurs", "Memphis Grizzlies"};
        string[] div = { "Atlantic", "Central", "South-East", "North-West", "Pacific", "South-West" };
        string[] conf = { "East", "West" };
        List<Team> teams = new List<Team>();
        


        public League()
        {
            for (int i = 0; i < names.Count(); i++)
            {
                Team team = new Team(names[i], div[i / (names.Count()/6)], conf[i / (names.Count()/2)]);
                teams.Add(team);
            }
        }

        public List<Team> getTeams()
        {
            return teams;
        }
    }
}
