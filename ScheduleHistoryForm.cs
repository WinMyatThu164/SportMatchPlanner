using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ScheduleHistoryForm : Form
    {
        private string connectionString;
        private Form parentForm; // Reference to the parent form (Form1 or ScheduleForm)

        public ScheduleHistoryForm(Form parent, string connectionString)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.connectionString = connectionString;
            LoadHistory();
        }

        private void LoadHistory()
        {
            historyListBox.Items.Clear();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Id, Timestamp, NumTeams FROM ScheduleHistory ORDER BY Timestamp DESC";
                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                DateTime timestamp = reader.GetDateTime(1);
                                int numTeams = reader.GetInt32(2);
                                historyListBox.Items.Add($"ID: {id} - {timestamp} ({numTeams} teams)");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historyListBox.SelectedIndex >= 0)
            {
                string selectedItem = historyListBox.SelectedItem.ToString();
                int id = int.Parse(selectedItem.Split('-')[0].Replace("ID: ", "").Trim());

                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = "SELECT Schedule, NumTeams FROM ScheduleHistory WHERE Id = @Id";
                        using (var command = new SqlCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string schedule = reader.GetString(0);
                                    int numTeams = reader.GetInt32(1);
                                    historyTextBox.Text = $"Schedule:\n{schedule}\n\nStandings:\n{GenerateStandingsFromSchedule(schedule)}";
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading schedule details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                historyTextBox.Text = string.Empty;
            }
        }

        private string GenerateStandingsFromSchedule(string schedule)
        {
            // Parse schedule to generate standings
            Dictionary<string, (int Wins, int Losses, int Ties, int PointsFor, int PointsAgainst)> teamStats = new Dictionary<string, (int, int, int, int, int)>();
            Dictionary<string, int> teamPoints = new Dictionary<string, int>();
            string[] lines = schedule.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inSchedule = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("Football (Soccer) League Round Robin Schedule"))
                {
                    inSchedule = true;
                    continue;
                }
                else if (trimmedLine.StartsWith("Note:"))
                {
                    inSchedule = false;
                    continue;
                }
                else if (trimmedLine.Contains("Edit") || trimmedLine.Contains("Score") || trimmedLine.Contains("Note"))
                {
                    continue; // Skip labels
                }

                if (inSchedule && !string.IsNullOrEmpty(trimmedLine) && trimmedLine.Contains("vs"))
                {
                    string[] teams = trimmedLine.Split(new[] { " vs " }, StringSplitOptions.None);
                    if (teams.Length == 2 && teams[0].StartsWith("Team") && teams[1].StartsWith("Team") && teams[0] != teams[1])
                    {
                        if (!teamStats.ContainsKey(teams[0])) teamStats[teams[0]] = (0, 0, 0, 0, 0);
                        if (!teamStats.ContainsKey(teams[1])) teamStats[teams[1]] = (0, 0, 0, 0, 0);
                        if (!teamPoints.ContainsKey(teams[0])) teamPoints[teams[0]] = 0;
                        if (!teamPoints.ContainsKey(teams[1])) teamPoints[teams[1]] = 0;

                        if (trimmedLine.Contains("Final"))
                        {
                            string[] scoreParts = trimmedLine.Split(new[] { " - " }, StringSplitOptions.None);
                            if (scoreParts.Length == 2)
                            {
                                string score1Str = scoreParts[0].Split(' ').Length > 1 ? scoreParts[0].Split(' ')[1] : "0";
                                string score2Str = scoreParts[1].Split(' ').Length > 1 ? scoreParts[1].Split(' ')[0] : "0";
                                if (int.TryParse(score1Str, out int score1) && int.TryParse(score2Str, out int score2))
                                {
                                    var stats1 = teamStats[teams[0]];
                                    var stats2 = teamStats[teams[1]];
                                    stats1.PointsFor += score1;
                                    stats1.PointsAgainst += score2;
                                    stats2.PointsFor += score2;
                                    stats2.PointsAgainst += score1;

                                    if (score1 > score2)
                                    {
                                        stats1.Wins++;
                                        stats2.Losses++;
                                        teamPoints[teams[0]] += 3; // Win
                                    }
                                    else if (score1 < score2)
                                    {
                                        stats2.Wins++;
                                        stats1.Losses++;
                                        teamPoints[teams[1]] += 3; // Win for team2
                                    }
                                    else
                                    {
                                        stats1.Ties++;
                                        stats2.Ties++;
                                        teamPoints[teams[0]] += 1; // Draw
                                        teamPoints[teams[1]] += 1; // Draw
                                    }

                                    teamStats[teams[0]] = stats1;
                                    teamStats[teams[1]] = stats2;
                                }
                            }
                        }
                    }
                }
            }

            StringBuilder standings = new StringBuilder();
            var sortedTeams = new List<string>(teamStats.Keys);
            sortedTeams.Sort((a, b) =>
            {
                int pointsA = teamPoints.ContainsKey(a) ? teamPoints[a] : 0;
                int pointsB = teamPoints.ContainsKey(b) ? teamPoints[b] : 0;
                var statsA = teamStats[a];
                var statsB = teamStats[b];
                int pdA = statsA.PointsFor - statsA.PointsAgainst;
                int pdB = statsB.PointsFor - statsB.PointsAgainst;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return pdB.CompareTo(pdA); // Sort by point differential descending
            });

            standings.AppendLine("Team\tW\tL\tT\tPCT\tPF\tPA\tPD");
            foreach (var team in sortedTeams)
            {
                var stats = teamStats[team];
                double pct = stats.Wins + stats.Ties > 0 ? (double)(stats.Wins + stats.Ties * 0.5) / (stats.Wins + stats.Losses + stats.Ties) : 0;
                int pd = stats.PointsFor - stats.PointsAgainst;
                int points = teamPoints[team];
                standings.AppendLine($"{team}\t{stats.Wins}\t{stats.Losses}\t{stats.Ties}\t{pct:F3}\t{stats.PointsFor}\t{stats.PointsAgainst}\t{pd}");
            }

            return standings.ToString();
        }

        

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}