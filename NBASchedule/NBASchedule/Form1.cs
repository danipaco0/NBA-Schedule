using System.Linq;
using System;
using System.Collections.Generic;

namespace NBASchedule
{
    public partial class Form1 : Form
    {
        public enum Status{ WORKING, NO_SOLUTION};
        string status = Status.NO_SOLUTION.ToString();
        const int nbrDays = 42;
        List<Team> teams = new List<Team>();
        IDictionary<Team, List<Team>> opponents = new Dictionary<Team, List<Team>>();
        IDictionary<int, List<Match>> schedule = new Dictionary<int, List<Match>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void ScheduleCreation()
        {
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams.Count; j++)
                {
                    if(teams[i].Name != teams[j].Name)
                    {
                        for (int k = 1; k <= nbrDays; k++)
                        {
                            if (schedule[k].Count < 4)
                            {
                                CheckConstraints(k, teams[i], teams[j]);
                            }
                        }
                    }
                }
            }
        }

        //Mélanger liste des équipes
        private void RndTeams()
        {
            Random rand = new Random();
            teams = teams.OrderBy(s => rand.Next()).ToList();
        }

        private void CreateOppDict()
        {
            foreach(Team team in teams)
            {
                List<Team> temp = new List<Team>();
                opponents.Add(team, temp);
            }
        }

        private void CreateSchedDict()
        {
            for(int i = 1; i <= nbrDays ; i++)
            {
                List<Match> m = new List<Match>();
                schedule.Add(i, m);
            }
        }

        //Vérification de toutes les contraintes et actions
        private void CheckConstraints(int day, Team a, Team b)
        {
            if (!SameDay(day, a, b) && !BackToBack(day, a, b))
            {
                if(!BackToBackBreak(day, a, b) && !NoBackToBack(day, a, b))
                {
                    if (SameDivision(a, b))
                    {
                        a.gamesAgainstDiv--;
                        b.gamesAgainstDiv--;
                        MatchCreator(day, a, b);
                    }
                    else if (SameConference(a, b))
                    {
                        a.gamesAgainstConf--;
                        b.gamesAgainstConf--;
                        MatchCreator(day, a, b);
                    }
                    else if (CanPlayOtherConference(a, b))
                    {
                        a.gamesAgainstOtherConf--;
                        b.gamesAgainstOtherConf--;
                        MatchCreator(day, a, b);
                    }
                }
            }
        }

        //Création d'un match et ajout dans la liste
        private void MatchCreator(int day, Team a, Team b)
        {
            opponents[a].Add(b);
            opponents[b].Add(a);
            Match match = new Match(a, b);
            schedule[day].Add(match);
        }

        //Vérification si l'équipe ne joue pas le même jour
        private bool SameDay(int day, Team a, Team b)
        {
            if (schedule[day].Any(item => (item.homeTeam == a || item.homeTeam == b) || (item.awayTeam == a || item.awayTeam == b)))
                return true;
            return false;
        }

        //Vérification si une des équipe a joué les 2 derniers jours d'affilés
        private bool BackToBack(int day, Team a, Team b)
        {
            if(day > 2)
            {
                if(SameDay(day-1, a, b) && SameDay(day-2, a, b))
                    return true;
            }
            return false;
        }

        //Vérification si pause de 2 jours après back to back
        private bool BackToBackBreak(int day, Team a, Team b)
        {
            if(day > 3)
            {
                if (BackToBack(day - 1, a, b))
                    return true;
            }
            return false;
        }

        //Pas de back to back les deux premiers jours
        private bool NoBackToBack(int day, Team a, Team b)
        {
            if (day == 2 && SameDay(day-1, a, b))
                return true;
            return false;
        }

        //Vérification si même division
        private bool SameDivision(Team a, Team b)
        {
            if ((a.Division == b.Division) && (a.gamesAgainstDiv > 0))
                return true;
            return false;
        }

        //Vérification si même conférence mais pas même division
        private bool SameConference(Team a, Team b)
        {
            if((a.Division != b.Division)&&(a.Conference == b.Conference))
            {
                if(a.gamesAgainstConf > 0 && b.gamesAgainstConf > 0)
                {
                    if (opponents[a].Where(s => s.Name.Equals(b.Name)).Count() < 2)
                        return true;
                }
            }
            return false;
        }

        //Vérification si match possible contre équipe hors-conférence
        private bool CanPlayOtherConference(Team a, Team b)
        {
            if (!opponents[a].Contains(b))
                return true;
            return false;
        }


        private static string returnDay(int k)
        {
            switch (k)
            {
                case 1:
                case 8:
                case 15:
                case 22:
                case 29:
                case 36:
                case 43:
                    return "Monday";
                case 2:
                case 9:
                case 16:
                case 23:
                case 30:
                case 37:
                case 44:
                    return "Tuesday";
                case 3:
                case 10:
                case 17:
                case 24:
                case 31:
                case 38:
                case 45:
                    return "Wednesday";
                case 4:
                case 11:
                case 18:
                case 25:
                case 32:
                case 39:
                case 46:
                    return "Thursday";
                case 5:
                case 12:
                case 19:
                case 26:
                case 33:
                case 40:
                case 47:
                    return "Friday";
                case 6:
                case 13:
                case 20:
                case 27:
                case 34:
                case 41:
                case 48:
                    return "Saturday";
                case 7:
                case 14:
                case 21:
                case 28:
                case 35:
                case 42:
                case 49:
                    return "Sunday";
                default:
                    return "null";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int nbrOfTries = 20;
            label1.Visible = true;
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            do
            {
                League league = new League();
                teams.Clear();
                opponents.Clear();
                schedule.Clear();
                teams = league.getTeams();
                RndTeams();
                CreateOppDict();
                CreateSchedDict();
                ScheduleCreation();
                if (CheckStatus())
                    break;
                nbrOfTries--;
            } while (nbrOfTries > 0);
            if (status == Status.WORKING.ToString())
            {
                comboBox1.Visible = true;
                label1.Text = "Status = " + status + "\r\n";
                label1.ForeColor = System.Drawing.Color.Green;
                comboBox1.Items.Clear();
                for (int i = 1; i <= nbrDays / 7; i++)
                    comboBox1.Items.Add("Week " + i);
                dataGridView1.RowCount = 8;
                comboBox1.SelectedIndex = 0;
                ShowInConsole(1, 8);
            }
            else
            {
                label1.Text = "Status = " + status + "\r\n";
                label1.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void ShowInConsole(int min, int max)
        {
            for(int i = min; i < max; i++)
            {
                int index = 0;
                foreach (Match m in schedule[i])
                {
                    dataGridView1.Rows[index].Cells[returnDay(i)].Value = m.homeTeam.Name;
                    dataGridView1.Rows[index+1].Cells[returnDay(i)].Value = m.awayTeam.Name;
                    index += 2;
                }
            }
        }

        private bool CheckStatus()
        {
            int index = 0;
            foreach(Team t in teams)
            {
                if (t.gamesAgainstConf == 0 && t.gamesAgainstDiv == 0 && t.gamesAgainstOtherConf == 0)
                    index++;
            }
            if (index == 12)
            {
                status = Status.WORKING.ToString();
                return true;
            }
            return false;   
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = 8;
            ShowInConsole(7*comboBox1.SelectedIndex + 1, 8 + comboBox1.SelectedIndex * 7);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.RowIndex%2 == 0)
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            else
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;  
        }
    }
}