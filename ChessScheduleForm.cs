using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml; // For Excel export
using System.Data.SqlClient;
using System.Text;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ChessScheduleForm : Form
    {
        private Dictionary<string, int> playerPoints = new Dictionary<string, int>();
        private List<(string Player1, string Player2)> matches = new List<(string, string)>();
        private Dictionary<int, (double? Score1, double? Score2)> matchScores = new Dictionary<int, (double?, double?)>(); // Use double for Chess scores (0, 0.5, 1)
        private const string SaveFilePath = "chess_scores.txt"; // File to save manually entered scores
        private string connectionString;

        public ChessScheduleForm(string schedule = null, int numPlayers = 0, string connectionString = null)
        {
            InitializeComponent();
            this.connectionString = connectionString ?? "Server=(localdb)\\MSSQLLocalDB;Database=FootballSchedulerDB;Integrated Security=True;Connect Timeout=30;";
            InitializeDatabase(); // Ensure database and table exist for Chess
            DisplaySchedule(schedule ?? GenerateDefaultSchedule(numPlayers), numPlayers);
            LoadSavedScores(numPlayers); // Load any manually entered saved scores
            SaveScheduleToHistory(GenerateCurrentSchedule(), numPlayers); // Save the current schedule to history
            UpdateDisplay(); // Ensure display is updated after loading
        }

        public List<(string Player1, string Player2)> Matches => matches; // Expose matches for bulk entry

        public (double? Score1, double? Score2) GetMatchScores(int index)
        {
            return matchScores.ContainsKey(index) ? matchScores[index] : (null, null); // Returns only manually entered scores
        }

        public void UpdateMatchScore(int index, string player1, string player2, double score1, double score2)
        {
            if (player1 == player2) // Safety check for self-matches
            {
                MessageBox.Show("Players cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Only update with scores manually entered by the user (Chess scoring: 1 for win, 0 for loss, 0.5 for draw)
            matchScores[index] = (score1, score2);
            UpdatePoints(player1, player2, score1, score2);
            UpdateDisplay();
            DetermineAndShowWinner();
            SaveScores(); // Auto-save manually entered scores
            SaveScheduleToHistory(GenerateCurrentSchedule(), matches.Count); // Update history with new scores
            MessageBox.Show($"Scores for {player1} vs {player2} updated successfully (manually entered).", "Score Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisplaySchedule(string scheduleText, int numPlayers)
        {
            // Initialize player points and stats (no automatic scores)
            for (int i = 1; i <= numPlayers; i++)
            {
                string playerName = $"Player {i}";
                playerPoints[playerName] = 0;
            }

            // Parse round-robin schedule and store matches
            string[] lines = scheduleText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int matchIndex = 0;
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
                        matches.Add((players[0], players[1]));
                        // Parse scores if available
                        if (trimmedLine.Contains("Final"))
                        {
                            string[] scoreParts = trimmedLine.Split(new[] { " - " }, StringSplitOptions.None);
                            if (scoreParts.Length == 2)
                            {
                                string score1Str = scoreParts[0].Split(' ').Length > 1 ? scoreParts[0].Split(' ')[1] : "0";
                                string score2Str = scoreParts[1].Split(' ').Length > 1 ? scoreParts[1].Split(' ')[0] : "0";
                                if (double.TryParse(score1Str, out double score1) && double.TryParse(score2Str, out double score2))
                                {
                                    matchScores[matchIndex] = (score1, score2);
                                    UpdatePoints(players[0], players[1], score1, score2);
                                }
                            }
                        }
                        else
                        {
                            matchScores[matchIndex] = (null, null); // No scores yet
                        }
                        matchIndex++;
                    }
                }
            }

            // Log matches for debugging
            Console.WriteLine($"Loaded {matches.Count} matches: {string.Join(", ", matches.Select(m => $"{m.Player1} vs {m.Player2}"))}");
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Debug output to check row and column clicked
                Console.WriteLine($"Double-clicked cell at Row {e.RowIndex}, Column {e.ColumnIndex}");

                // Check if the click is in the schedule section (after standings and spacer)
                if (e.RowIndex >= 2 && e.RowIndex < 2 + matches.Count) // Skip "Standings" header, spacer, "Schedule" header
                {
                    int selectedIndex = (e.RowIndex - 2); // Adjust for standings, spacer, and label rows
                    Console.WriteLine($"Selected match index: {selectedIndex}");

                    if (selectedIndex >= 0 && selectedIndex < matches.Count)
                    {
                        var match = matches[selectedIndex];
                        if (match.Player1 == match.Player2)
                        {
                            MessageBox.Show("This is an invalid match (players cannot play against themselves). Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Open ScoreEntryForm for manual score entry (Chess scoring: 1-0, 0-1, 0.5-0.5)
                        using (ChessScoreEntryForm scoreForm = new ChessScoreEntryForm(match.Player1, match.Player2, this, selectedIndex, true)) // true for Chess scoring
                        {
                            scoreForm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid match selection. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please double-click a match in the Schedule section to enter scores individually, or use 'Enter All Scores' for bulk entry.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening score entry form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception details: {ex.StackTrace}");
            }
        }

        private void enterAllScoresButton_Click(object sender, EventArgs e)
        {
            using (ChessBulkScoreEntryForm bulkForm = new ChessBulkScoreEntryForm(this, matches.Count / 2 + 1, true)) // true for Chess scoring
            {
                bulkForm.ShowDialog();
            }
            UpdateDisplay(); // Refresh display after bulk scoring
            SaveScheduleToHistory(GenerateCurrentSchedule(), matches.Count); // Update history after bulk scoring
        }

        private void exportToExcelButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Show SaveFileDialog to let the user choose the Excel file location
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Save Chess Tournament Results",
                    FileName = "ChessTournamentResults.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Use EPPlus to create and save the Excel file
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context for EPPlus
                    using (var package = new ExcelPackage())
                    {
                        // Create a worksheet for standings
                        var worksheet = package.Workbook.Worksheets.Add("Standings");
                        worksheet.Cells[1, 1].Value = "Chess Tournament Standings";
                        worksheet.Cells[1, 1, 1, 6].Merge = true;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Add headers
                        worksheet.Cells[3, 1].Value = "Player";
                        worksheet.Cells[3, 2].Value = "Points";
                        worksheet.Cells[3, 3].Value = "W";
                        worksheet.Cells[3, 4].Value = "D";
                        worksheet.Cells[3, 5].Value = "L";
                        for (int col = 1; col <= 5; col++)
                        {
                            worksheet.Cells[3, col].Style.Font.Bold = true;
                            worksheet.Cells[3, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        // Add standings data
                        int row = 4;
                        Dictionary<string, (int Wins, int Losses, int Draws, int PointsFor, int PointsAgainst)> playerStats = new Dictionary<string, (int, int, int, int, int)>();
                        foreach (var player in playerPoints.Keys)
                        {
                            playerStats[player] = (0, 0, 0, 0, 0);
                        }

                        foreach (var match in matchScores)
                        {
                            int index = match.Key;
                            var scores = match.Value;
                            if (scores.Score1.HasValue && scores.Score2.HasValue)
                            {
                                string player1 = matches[index].Player1;
                                string player2 = matches[index].Player2;

                                if (player1 == player2) continue; // Skip self-matches

                                var stats1 = playerStats[player1];
                                var stats2 = playerStats[player2];

                                double score1 = scores.Score1.Value;
                                double score2 = scores.Score2.Value;

                                stats1.PointsFor += (int)score1;
                                stats1.PointsAgainst += (int)score2;
                                stats2.PointsFor += (int)score2;
                                stats2.PointsAgainst += (int)score1;

                                if (score1 == 1 && score2 == 0)
                                {
                                    stats1.Wins++;
                                    stats2.Losses++;
                                }
                                else if (score1 == 0 && score2 == 1)
                                {
                                    stats2.Wins++;
                                    stats1.Losses++;
                                }
                                else if (score1 == 0.5 && score2 == 0.5)
                                {
                                    stats1.Draws++;
                                    stats2.Draws++;
                                }

                                playerStats[player1] = stats1;
                                playerStats[player2] = stats2;
                            }
                        }

                        var sortedPlayers = new List<string>(playerStats.Keys);
                        sortedPlayers.Sort((a, b) =>
                        {
                            var statsA = playerStats[a];
                            var statsB = playerStats[b];
                            int pointsA = playerPoints.ContainsKey(a) ? playerPoints[a] : 0;
                            int pointsB = playerPoints.ContainsKey(b) ? playerPoints[b] : 0;

                            if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                            if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                            return (statsB.PointsFor - statsB.PointsAgainst).CompareTo(statsA.PointsFor - statsA.PointsAgainst); // Sort by point differential descending
                        });

                        foreach (var player in sortedPlayers)
                        {
                            var stats = playerStats[player];
                            int points = playerPoints[player];
                            worksheet.Cells[row, 1].Value = player;
                            worksheet.Cells[row, 2].Value = points;
                            worksheet.Cells[row, 3].Value = stats.Wins;
                            worksheet.Cells[row, 4].Value = stats.Draws;
                            worksheet.Cells[row, 5].Value = stats.Losses;
                            for (int col = 1; col <= 5; col++)
                            {
                                worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            row++;
                        }

                        // Auto-fit columns
                        for (int col = 1; col <= 5; col++)
                        {
                            worksheet.Column(col).AutoFit();
                        }

                        // Create a worksheet for match results
                        var matchesWorksheet = package.Workbook.Worksheets.Add("Matches");
                        matchesWorksheet.Cells[1, 1].Value = "Chess Tournament Matches";
                        matchesWorksheet.Cells[1, 1, 1, 2].Merge = true;
                        matchesWorksheet.Cells[1, 1].Style.Font.Bold = true;
                        matchesWorksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Add headers
                        matchesWorksheet.Cells[3, 1].Value = "Match";
                        matchesWorksheet.Cells[3, 2].Value = "Final";
                        for (int col = 1; col <= 2; col++)
                        {
                            matchesWorksheet.Cells[3, col].Style.Font.Bold = true;
                            matchesWorksheet.Cells[3, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        // Add match data
                        int matchRow = 4;
                        for (int i = 0; i < matches.Count; i++)
                        {
                            var match = matches[i];
                            var scores = matchScores[i];
                            string displayText = $"{match.Player1} vs {match.Player2}";
                            if (scores.Score1.HasValue && scores.Score2.HasValue)
                            {
                                displayText = $"{match.Player1} {scores.Score1} - {scores.Score2} {match.Player2} Final";
                            }
                            matchesWorksheet.Cells[matchRow, 1].Value = displayText;
                            matchesWorksheet.Cells[matchRow, 2].Value = scores.Score1.HasValue && scores.Score2.HasValue ? "Yes" : "No";
                            for (int col = 1; col <= 2; col++)
                            {
                                matchesWorksheet.Cells[matchRow, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            matchRow++;
                        }

                        // Auto-fit columns
                        for (int col = 1; col <= 2; col++)
                        {
                            matchesWorksheet.Column(col).AutoFit();
                        }

                        // Save the Excel file
                        FileInfo file = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(file);

                        MessageBox.Show($"Chess tournament results exported to {saveFileDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void viewHistoryButton_Click(object sender, EventArgs e)
        {
            using (ChessHistoryForm historyForm = new ChessHistoryForm(this, connectionString))
            {
                historyForm.ShowDialog();
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChessScheduleHistory')
                        BEGIN
                            CREATE TABLE ChessScheduleHistory (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Timestamp DATETIME NOT NULL,
                                NumPlayers INT NOT NULL,
                                Schedule NVARCHAR(MAX) NOT NULL,
                                Standings NVARCHAR(MAX) NOT NULL
                            )
                        END";
                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error initializing Chess database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Chess database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveScheduleToHistory(string schedule, int numPlayers)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string standings = GenerateStandingsText();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO ChessScheduleHistory (Timestamp, NumPlayers, Schedule, Standings) VALUES (@Timestamp, @NumPlayers, @Schedule, @Standings)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Timestamp", DateTime.Parse(timestamp));
                        command.Parameters.AddWithValue("@NumPlayers", numPlayers);
                        command.Parameters.AddWithValue("@Schedule", schedule);
                        command.Parameters.AddWithValue("@Standings", standings);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error saving Chess schedule history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Chess schedule history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateDefaultSchedule(int numPlayers)
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine($"Chess League Round Robin Schedule ({numPlayers} Players):\n");

            int totalMatches = (numPlayers * (numPlayers - 1)) / 2;
            int matchCount = 0;

            for (int i = 1; i <= numPlayers; i++)
            {
                for (int j = i + 1; j <= numPlayers; j++)
                {
                    if (matchCount >= totalMatches) break;
                    string player1 = $"Player {i}";
                    string player2 = $"Player {j}";
                    schedule.AppendLine($"{player1} vs {player2}");
                    schedule.AppendLine("    Edit    Score    Note");
                    matchCount++;
                }
            }

            if (numPlayers % 2 != 0)
            {
                schedule.AppendLine("\nNote: Players with a bye will rest in certain rounds (not shown as matches).");
            }

            return schedule.ToString();
        }

        private string GenerateCurrentSchedule()
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine($"Chess League Round Robin Schedule ({matches.Count} Matches):\n");

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var scores = matchScores[i];
                string displayText = $"{match.Player1} vs {match.Player2}";
                if (scores.Score1.HasValue && scores.Score2.HasValue)
                {
                    displayText = $"{match.Player1} {scores.Score1} - {scores.Score2} {match.Player2} Final";
                }
                schedule.AppendLine(displayText);
            }

            if (matches.Count % 2 != 0)
            {
                schedule.AppendLine("\nNote: Players with a bye will rest in certain rounds (not shown as matches).");
            }

            return schedule.ToString();
        }

        private string GenerateStandingsText()
        {
            StringBuilder standings = new StringBuilder();
            Dictionary<string, (int Wins, int Losses, int Draws, int PointsFor, int PointsAgainst)> playerStats = new Dictionary<string, (int, int, int, int, int)>();
            foreach (var player in playerPoints.Keys)
            {
                playerStats[player] = (0, 0, 0, 0, 0);
            }

            foreach (var match in matchScores)
            {
                int index = match.Key;
                var scores = match.Value;
                if (scores.Score1.HasValue && scores.Score2.HasValue)
                {
                    string player1 = matches[index].Player1;
                    string player2 = matches[index].Player2;

                    if (player1 == player2) continue; // Skip self-matches

                    var stats1 = playerStats[player1];
                    var stats2 = playerStats[player2];

                    double score1 = scores.Score1.Value;
                    double score2 = scores.Score2.Value;

                    stats1.PointsFor += (int)score1;
                    stats1.PointsAgainst += (int)score2;
                    stats2.PointsFor += (int)score2;
                    stats2.PointsAgainst += (int)score1;

                    if (score1 == 1 && score2 == 0)
                    {
                        stats1.Wins++;
                        stats2.Losses++;
                    }
                    else if (score1 == 0 && score2 == 1)
                    {
                        stats2.Wins++;
                        stats1.Losses++;
                    }
                    else if (score1 == 0.5 && score2 == 0.5)
                    {
                        stats1.Draws++;
                        stats2.Draws++;
                    }

                    playerStats[player1] = stats1;
                    playerStats[player2] = stats2;
                }
            }

            var sortedPlayers = new List<string>(playerStats.Keys);
            sortedPlayers.Sort((a, b) =>
            {
                var statsA = playerStats[a];
                var statsB = playerStats[b];
                int pointsA = playerPoints.ContainsKey(a) ? playerPoints[a] : 0;
                int pointsB = playerPoints.ContainsKey(b) ? playerPoints[b] : 0;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return (statsB.PointsFor - statsB.PointsAgainst).CompareTo(statsA.PointsFor - statsA.PointsAgainst); // Sort by point differential descending
            });

            standings.AppendLine("Player\tPoints\tW\tD\tL");
            foreach (var player in sortedPlayers)
            {
                var stats = playerStats[player];
                int points = playerPoints[player];
                standings.AppendLine($"{player}\t{points}\t{stats.Wins}\t{stats.Draws}\t{stats.Losses}");
            }

            return standings.ToString();
        }

        private void DetermineAndShowWinner()
        {
            bool allMatchesScored = true;
            foreach (var scores in matchScores.Values)
            {
                if (!scores.Score1.HasValue || !scores.Score2.HasValue) // Check for manually entered scores only
                {
                    allMatchesScored = false;
                    break;
                }
            }

            if (allMatchesScored)
            {
                string winner = "No winner (tie or error)";
                int maxPoints = 0;
                List<string> winners = new List<string>(); // To handle ties

                foreach (var player in playerPoints)
                {
                    if (player.Value > maxPoints)
                    {
                        maxPoints = player.Value;
                        winners.Clear();
                        winners.Add(player.Key);
                    }
                    else if (player.Value == maxPoints)
                    {
                        winners.Add(player.Key);
                    }
                }

                if (winners.Count == 1)
                {
                    winner = $"Final Winner: {winners[0]} with {maxPoints}.0 points";
                }
                else
                {
                    winner = $"Tie for First: {string.Join(", ", winners)} with {maxPoints}.0 points each";
                }
                winnerLabel.Text = winner;
            }
            else
            {
                winnerLabel.Text = "Score all matches to determine the final winner.";
            }
        }

        private void SaveScores()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SaveFilePath))
                {
                    // Save number of matches and players
                    writer.WriteLine(matches.Count); // Number of matches
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var match = matches[i];
                        if (match.Player1 == match.Player2) continue; // Skip self-matches
                        var scores = matchScores[i];
                        writer.WriteLine($"{match.Player1}|{match.Player2}|{scores.Score1}|{scores.Score2}");
                    }

                    // Save player points
                    foreach (var player in playerPoints)
                    {
                        writer.WriteLine($"{player.Key}|{player.Value}");
                    }

                    MessageBox.Show("Manually entered scores and points saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving manually entered scores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSavedScores(int numPlayers)
        {
            if (File.Exists(SaveFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(SaveFilePath))
                    {
                        // Read number of matches
                        int matchCount = int.Parse(reader.ReadLine());
                        if (matchCount != matches.Count)
                        {
                            MessageBox.Show("Saved data does not match the current schedule. Starting fresh.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Load manually entered match scores, skipping self-matches
                        for (int i = 0; i < matchCount; i++)
                        {
                            string line = reader.ReadLine();
                            string[] parts = line.Split('|');
                            if (parts.Length == 4 && double.TryParse(parts[2], out double score1) && double.TryParse(parts[3], out double score2))
                            {
                                if (parts[0] != parts[1]) // Skip self-matches
                                {
                                    matchScores[i] = (score1, score2);
                                    UpdatePoints(parts[0], parts[1], score1, score2);
                                }
                            }
                        }

                        // Load player points
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] parts = line.Split('|');
                            if (parts.Length == 2 && int.TryParse(parts[1], out int points))
                            {
                                if (parts[0].StartsWith("Player")) // Ensure only valid player names are loaded
                                {
                                    playerPoints[parts[0]] = points;
                                }
                            }
                        }

                        // Update display with loaded manually entered scores
                        UpdateDisplay();
                        DetermineAndShowWinner();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading manually entered scores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDisplay()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Add Standings section
            dataGridView.Columns.Add("Player", "Player");
            dataGridView.Columns.Add("Points", "Points");
            dataGridView.Columns.Add("W", "W");
            dataGridView.Columns.Add("D", "D");
            dataGridView.Columns.Add("L", "L");
            dataGridView.Columns["Player"].Width = 100;
            dataGridView.Columns["Points"].Width = 60;
            dataGridView.Columns["W"].Width = 40;
            dataGridView.Columns["D"].Width = 40;
            dataGridView.Columns["L"].Width = 40;

            // Calculate player stats based only on manually entered scores
            Dictionary<string, (int Wins, int Losses, int Draws, int PointsFor, int PointsAgainst)> playerStats = new Dictionary<string, (int, int, int, int, int)>();
            foreach (var player in playerPoints.Keys)
            {
                playerStats[player] = (0, 0, 0, 0, 0);
            }

            // Calculate stats from manually entered match scores
            foreach (var match in matchScores)
            {
                int index = match.Key;
                var scores = match.Value;
                if (scores.Score1.HasValue && scores.Score2.HasValue) // Only use user-entered scores
                {
                    string player1 = matches[index].Player1;
                    string player2 = matches[index].Player2;

                    if (player1 == player2) continue; // Skip self-matches

                    var stats1 = playerStats[player1];
                    var stats2 = playerStats[player2];

                    double score1 = scores.Score1.Value;
                    double score2 = scores.Score2.Value;

                    stats1.PointsFor += (int)score1;
                    stats1.PointsAgainst += (int)score2;
                    stats2.PointsFor += (int)score2;
                    stats2.PointsAgainst += (int)score1;

                    if (score1 == 1 && score2 == 0)
                    {
                        stats1.Wins++;
                        stats2.Losses++;
                    }
                    else if (score1 == 0 && score2 == 1)
                    {
                        stats2.Wins++;
                        stats1.Losses++;
                    }
                    else if (score1 == 0.5 && score2 == 0.5)
                    {
                        stats1.Draws++;
                        stats2.Draws++;
                    }

                    playerStats[player1] = stats1;
                    playerStats[player2] = stats2;
                }
            }

            // Sort players by points (descending), then wins, then point differential
            var sortedPlayers = new List<string>(playerStats.Keys);
            sortedPlayers.Sort((a, b) =>
            {
                var statsA = playerStats[a];
                var statsB = playerStats[b];
                int pointsA = playerPoints.ContainsKey(a) ? playerPoints[a] : 0;
                int pointsB = playerPoints.ContainsKey(b) ? playerPoints[b] : 0;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return (statsB.PointsFor - statsB.PointsAgainst).CompareTo(statsA.PointsFor - statsA.PointsAgainst); // Sort by point differential descending
            });

            // Add Standings header
            dataGridView.Rows.Add("Standings");
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Blue; // Blue header
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
            dataGridView.Rows[dataGridView.Rows.Count - 1].ReadOnly = true;

            // Add standings rows
            foreach (var player in sortedPlayers)
            {
                var stats = playerStats[player];
                int points = playerPoints[player];
                dataGridView.Rows.Add(player, points, stats.Wins, stats.Draws, stats.Losses);
                dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray; // Gray rows
                dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.LimeGreen; // Green text
            }

            // Add Schedule section
            dataGridView.Rows.Add(""); // Spacer
            dataGridView.Rows.Add("Schedule");
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray; // Gray header
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
            dataGridView.Rows[dataGridView.Rows.Count - 1].ReadOnly = true;

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.Player1 == match.Player2) continue; // Skip self-matches

                var scores = matchScores[i];
                string displayText = $"{match.Player1} vs {match.Player2}";
                if (scores.Score1.HasValue && scores.Score2.HasValue) // Only display manually entered scores
                {
                    displayText = $"{match.Player1} {scores.Score1} - {scores.Score2} {match.Player2} Final";
                }
                dataGridView.Rows.Add(displayText);
                dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.LimeGreen; // Green text
            }

            // Format DataGridView
            dataGridView.BackgroundColor = Color.FromArgb(240, 240, 245); // Light blue background
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue; // Dark blue headers
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.EnableHeadersVisualStyles = false;
        }

        private void UpdatePoints(string player1, string player2, double score1, double score2)
        {
            if (player1 == player2) // Safety check for self-matches
            {
                MessageBox.Show("Players cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update points based on Chess scoring: 1 for win, 0.5 for draw, 0 for loss
            if (score1 == 1 && score2 == 0)
            {
                playerPoints[player1] += 1; // Win
            }
            else if (score1 == 0 && score2 == 1)
            {
                playerPoints[player2] += 1; // Win for player2
            }
            else if (score1 == 0.5 && score2 == 0.5)
            {
                playerPoints[player1] += 1; // Draw
                playerPoints[player2] += 1; // Draw
            }
        }
    }
}