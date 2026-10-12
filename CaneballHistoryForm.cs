using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class CaneballHistoryForm : Form
    {
        private string connectionString;
        private Form parentForm; // Reference to the parent form

        public CaneballHistoryForm(Form parent, string connectionString)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.connectionString = connectionString ?? "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Mgkaung\\C#\\project\\new\\WinFormsApp5\\WinFormsApp5\\FootballScheduleHistory.mdf;Integrated Security=True";
            LoadHistory();
        }

        private void LoadHistory()
        {
            historyListBox.Items.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Id, Timestamp, NumTeams FROM CaneballScheduleHistory ORDER BY Timestamp DESC";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
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
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error loading Caneball history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Caneball history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = "SELECT Schedule, NumTeams FROM CaneballScheduleHistory WHERE Id = @Id";
                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string schedule = reader.GetString(0);
                                    int numTeams = reader.GetInt32(1);
                                    historyTextBox.Text = $"Schedule:\n{schedule}\n\nStandings:\n{GenerateStandingsFromSchedule(schedule)}";
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"SQL Error loading Caneball schedule details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Caneball schedule details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                historyTextBox.Text = string.Empty;
            }
        }

        private string GenerateStandingsFromSchedule(string schedule)
        {
            // Parse schedule to generate standings for Caneball
            Dictionary<string, (int Wins, int Losses, int Draws, int PointsFor, int PointsAgainst)> teamStats = new Dictionary<string, (int, int, int, int, int)>();
            Dictionary<string, int> teamPerformance = new Dictionary<string, int>();
            string[] lines = schedule.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inSchedule = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("Caneball Round Robin Schedule"))
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
                        if (!teamPerformance.ContainsKey(teams[0])) teamPerformance[teams[0]] = 0;
                        if (!teamPerformance.ContainsKey(teams[1])) teamPerformance[teams[1]] = 0;

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
                                        teamPerformance[teams[0]] += 2; // Win
                                    }
                                    else if (score1 < score2)
                                    {
                                        stats2.Wins++;
                                        stats1.Losses++;
                                        teamPerformance[teams[1]] += 2; // Win
                                    }
                                    else
                                    {
                                        stats1.Draws++;
                                        stats2.Draws++;
                                        teamPerformance[teams[0]] += 1; // Draw
                                        teamPerformance[teams[1]] += 1; // Draw
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
                int pointsA = teamPerformance.ContainsKey(a) ? teamPerformance[a] : 0;
                int pointsB = teamPerformance.ContainsKey(b) ? teamPerformance[b] : 0;
                var statsA = teamStats[a];
                var statsB = teamStats[b];
                int pdA = statsA.PointsFor - statsA.PointsAgainst;
                int pdB = statsB.PointsFor - statsB.PointsAgainst;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return pdB.CompareTo(pdA); // Sort by point differential descending
            });

            standings.AppendLine("Team\tPoints\tW\tD\tL");
            foreach (var team in sortedTeams)
            {
                var stats = teamStats[team];
                int points = teamPerformance[team];
                standings.AppendLine($"{team}\t{points}\t{stats.Wins}\t{stats.Draws}\t{stats.Losses}");
            }

            return standings.ToString();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}