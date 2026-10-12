using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ChessHistoryForm : Form
    {
        private string connectionString;
        private Form parentForm; // Reference to the parent form (Form1 or ChessScheduleForm)

        public ChessHistoryForm(Form parent, string connectionString)
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
                    string selectQuery = "SELECT Id, Timestamp, NumPlayers FROM ChessScheduleHistory ORDER BY Timestamp DESC";
                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                DateTime timestamp = reader.GetDateTime(1);
                                int numPlayers = reader.GetInt32(2);
                                historyListBox.Items.Add($"ID: {id} - {timestamp} ({numPlayers} players)");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Chess history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string selectQuery = "SELECT Schedule, NumPlayers FROM ChessScheduleHistory WHERE Id = @Id";
                        using (var command = new SqlCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string schedule = reader.GetString(0);
                                    int numPlayers = reader.GetInt32(1);
                                    historyTextBox.Text = $"Schedule:\n{schedule}\n\nStandings:\n{GenerateStandingsFromSchedule(schedule)}";
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Chess schedule details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                historyTextBox.Text = string.Empty;
            }
        }

        private string GenerateStandingsFromSchedule(string schedule)
        {
            // Parse schedule to generate standings for Chess
            Dictionary<string, (int Wins, int Losses, int Draws, int PointsFor, int PointsAgainst)> playerStats = new Dictionary<string, (int, int, int, int, int)>();
            Dictionary<string, int> playerPoints = new Dictionary<string, int>();
            string[] lines = schedule.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inSchedule = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("Chess League Round Robin Schedule"))
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
                    string[] players = trimmedLine.Split(new[] { " vs " }, StringSplitOptions.None);
                    if (players.Length == 2 && players[0].StartsWith("Player") && players[1].StartsWith("Player") && players[0] != players[1])
                    {
                        if (!playerStats.ContainsKey(players[0])) playerStats[players[0]] = (0, 0, 0, 0, 0);
                        if (!playerStats.ContainsKey(players[1])) playerStats[players[1]] = (0, 0, 0, 0, 0);
                        if (!playerPoints.ContainsKey(players[0])) playerPoints[players[0]] = 0;
                        if (!playerPoints.ContainsKey(players[1])) playerPoints[players[1]] = 0;

                        if (trimmedLine.Contains("Final"))
                        {
                            string[] scoreParts = trimmedLine.Split(new[] { " - " }, StringSplitOptions.None);
                            if (scoreParts.Length == 2)
                            {
                                string score1Str = scoreParts[0].Split(' ').Length > 1 ? scoreParts[0].Split(' ')[1] : "0";
                                string score2Str = scoreParts[1].Split(' ').Length > 1 ? scoreParts[1].Split(' ')[0] : "0";
                                if (double.TryParse(score1Str, out double score1) && double.TryParse(score2Str, out double score2))
                                {
                                    var stats1 = playerStats[players[0]];
                                    var stats2 = playerStats[players[1]];
                                    stats1.PointsFor += (int)score1;
                                    stats1.PointsAgainst += (int)score2;
                                    stats2.PointsFor += (int)score2;
                                    stats2.PointsAgainst += (int)score1;

                                    if (score1 == 1 && score2 == 0)
                                    {
                                        stats1.Wins++;
                                        stats2.Losses++;
                                        playerPoints[players[0]] += 1; // Win
                                    }
                                    else if (score1 == 0 && score2 == 1)
                                    {
                                        stats2.Wins++;
                                        stats1.Losses++;
                                        playerPoints[players[1]] += 1; // Win for player2
                                    }
                                    else if (score1 == 0.5 && score2 == 0.5)
                                    {
                                        stats1.Draws++;
                                        stats2.Draws++;
                                        playerPoints[players[0]] += 1; // Draw
                                        playerPoints[players[1]] += 1; // Draw
                                    }

                                    playerStats[players[0]] = stats1;
                                    playerStats[players[1]] = stats2;
                                }
                            }
                        }
                    }
                }
            }

            StringBuilder standings = new StringBuilder();
            var sortedPlayers = new List<string>(playerStats.Keys);
            sortedPlayers.Sort((a, b) =>
            {
                int pointsA = playerPoints.ContainsKey(a) ? playerPoints[a] : 0;
                int pointsB = playerPoints.ContainsKey(b) ? playerPoints[b] : 0;
                var statsA = playerStats[a];
                var statsB = playerStats[b];
                int pdA = statsA.PointsFor - statsA.PointsAgainst;
                int pdB = statsB.PointsFor - statsB.PointsAgainst;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return pdB.CompareTo(pdA); // Sort by point differential descending
            });

            standings.AppendLine("Player\tW\tL\tD\tPoints\tPCT\tPF\tPA");
            foreach (var player in sortedPlayers)
            {
                var stats = playerStats[player];
                int points = playerPoints[player];
                double pct = stats.Wins + stats.Draws * 0.5 > 0 ? (double)(stats.Wins + stats.Draws * 0.5) / (stats.Wins + stats.Losses + stats.Draws) : 0;
                standings.AppendLine($"{player}\t{stats.Wins}\t{stats.Losses}\t{stats.Draws}\t{points}\t{pct:F3}\t{stats.PointsFor}\t{stats.PointsAgainst}");
            }

            return standings.ToString();
        }

        

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}