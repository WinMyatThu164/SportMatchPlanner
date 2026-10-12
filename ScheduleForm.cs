using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Text;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ScheduleForm : Form
    {
        private Dictionary<string, int> teamPoints = new Dictionary<string, int>();
        private List<(string Team1, string Team2)> matches = new List<(string, string)>();
        private Dictionary<int, (int? Score1, int? Score2)> matchScores = new Dictionary<int, (int?, int?)>(); // Store scores manually entered by user
        private const string SaveFilePath = "football_scores.txt"; 
        private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Mgkaung\\C#\\project\\new\\WinFormsApp5\\WinFormsApp5\\FootballScheduleHistory.mdf;Integrated Security=True"; // Define ConnectionString here

        public ScheduleForm(string schedule = null, int numTeams = 0)
        {
            InitializeComponent();
            InitializeDatabase(); // Ensure database and table exist
            DisplaySchedule(schedule ?? GenerateDefaultSchedule(numTeams), numTeams);
            LoadSavedScores(numTeams); // Load any manually entered saved scores
            SaveScheduleToHistory(GenerateCurrentSchedule(), numTeams); // Save the current schedule to history
            UpdateDisplay(); // Ensure display is updated after loading
        }

        public List<(string Team1, string Team2)> Matches => matches; 

        public (int? Score1, int? Score2) GetMatchScores(int index)
        {
            return matchScores.ContainsKey(index) ? matchScores[index] : (null, null); // Returns only manually entered scores
        }

        public void UpdateMatchScore(int index, string team1, string team2, int score1, int score2)
        {
            if (team1 == team2) // Safety check for self-matches
            {
                MessageBox.Show("Teams cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Only update with scores manually entered by the user
            matchScores[index] = (score1, score2);
            UpdatePoints(team1, team2, score1, score2);
            UpdateDisplay();
            DetermineAndShowWinner();
            SaveScores(); // Auto-save manually entered scores
            SaveScheduleToHistory(GenerateCurrentSchedule(), matches.Count); // Update history with new scores
        }

        private void DisplaySchedule(string scheduleText, int numTeams)
        {
            // Initialize team points and stats (no automatic scores)
            for (int i = 1; i <= numTeams; i++)
            {
                string teamName = $"Team {i}";
                teamPoints[teamName] = 0;
            }

            // Parse round-robin schedule and store matches
            string[] lines = scheduleText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int matchIndex = 0;
            bool inSchedule = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("Soccer League Round Robin Schedule") || trimmedLine.StartsWith("Football (Soccer) League Round Robin Schedule"))
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
                        matches.Add((teams[0], teams[1]));
                        // Parse scores if available
                        if (trimmedLine.Contains("Final"))
                        {
                            string[] scoreParts = trimmedLine.Split(new[] { " - " }, StringSplitOptions.None);
                            if (scoreParts.Length == 2)
                            {
                                string score1Str = scoreParts[0].Split(' ').Length > 1 ? scoreParts[0].Split(' ')[1] : "0";
                                string score2Str = scoreParts[1].Split(' ').Length > 1 ? scoreParts[1].Split(' ')[0] : "0";
                                if (int.TryParse(score1Str, out int score1) && int.TryParse(score2Str, out int score2))
                                {
                                    matchScores[matchIndex] = (score1, score2);
                                    UpdatePoints(teams[0], teams[1], score1, score2);
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
            Console.WriteLine($"Loaded {matches.Count} matches: {string.Join(", ", matches.Select(m => $"{m.Team1} vs {m.Team2}"))}");
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Debug output to check row and column clicked
                Console.WriteLine($"Double-clicked cell at Row {e.RowIndex}, Column {e.ColumnIndex}");

                // Check if the click is in the schedule section (after standings and spacer)
                if (e.RowIndex >= 2 && e.RowIndex < 2 + matches.Count * 2) // Skip "Standings" header, spacer, "Schedule" header, and account for "Edit," "Score," "Note" rows
                {
                    int selectedIndex = (e.RowIndex - 2) / 2; // Adjust for standings, spacer, and label rows
                    Console.WriteLine($"Selected match index: {selectedIndex}");

                    if (selectedIndex >= 0 && selectedIndex < matches.Count)
                    {
                        var match = matches[selectedIndex];
                        if (match.Team1 == match.Team2)
                        {
                            MessageBox.Show("This is an invalid match (teams cannot play against themselves). Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Open ScoreEntryForm for manual score entry (individual match)
                        using (ScoreEntryForm scoreForm = new ScoreEntryForm(match.Team1, match.Team2, this, selectedIndex))
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

        private void bulkScoreButton_Click(object sender, EventArgs e)
        {
            using (BulkScoreEntryForm bulkForm = new BulkScoreEntryForm(this, matches.Count / 2 + 1))
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
                    Title = "Save Football Tournament Results",
                    FileName = "FootballTournamentResults.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Use EPPlus to create and save the Excel file
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context for EPPlus
                    using (var package = new ExcelPackage())
                    {
                        // Create a worksheet for standings
                        var worksheet = package.Workbook.Worksheets.Add("Standings");
                        worksheet.Cells[1, 1].Value = "Football Tournament Standings";
                        worksheet.Cells[1, 1, 1, 8].Merge = true;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Add headers
                        worksheet.Cells[3, 1].Value = "Team";
                        worksheet.Cells[3, 2].Value = "Wins";
                        worksheet.Cells[3, 3].Value = "Losses";
                        worksheet.Cells[3, 4].Value = "Ties";
                        worksheet.Cells[3, 5].Value = "Points";
                        worksheet.Cells[3, 6].Value = "Win Percentage";
                        worksheet.Cells[3, 7].Value = "Points For";
                        worksheet.Cells[3, 8].Value = "Points Against";
                        worksheet.Cells[3, 9].Value = "Point Differential";
                        for (int col = 1; col <= 9; col++)
                        {
                            worksheet.Cells[3, col].Style.Font.Bold = true;
                            worksheet.Cells[3, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        // Add standings data
                        int row = 4;
                        Dictionary<string, (int Wins, int Losses, int Ties, int PointsFor, int PointsAgainst)> teamStats = new Dictionary<string, (int, int, int, int, int)>();
                        foreach (var team in teamPoints.Keys)
                        {
                            teamStats[team] = (0, 0, 0, 0, 0);
                        }

                        foreach (var match in matchScores)
                        {
                            int index = match.Key;
                            var scores = match.Value;
                            if (scores.Score1.HasValue && scores.Score2.HasValue)
                            {
                                string team1 = matches[index].Team1;
                                string team2 = matches[index].Team2;

                                if (team1 == team2) continue; // Skip self-matches

                                var stats1 = teamStats[team1];
                                var stats2 = teamStats[team2];

                                int score1 = scores.Score1.Value;
                                int score2 = scores.Score2.Value;

                                stats1.PointsFor += score1;
                                stats1.PointsAgainst += score2;
                                stats2.PointsFor += score2;
                                stats2.PointsAgainst += score1;

                                if (score1 > score2)
                                {
                                    stats1.Wins++;
                                    stats2.Losses++;
                                }
                                else if (score1 < score2)
                                {
                                    stats2.Wins++;
                                    stats1.Losses++;
                                }
                                else
                                {
                                    stats1.Ties++;
                                    stats2.Ties++;
                                }

                                teamStats[team1] = stats1;
                                teamStats[team2] = stats2;
                            }
                        }

                        var sortedTeams = new List<string>(teamStats.Keys);
                        sortedTeams.Sort((a, b) =>
                        {
                            var statsA = teamStats[a];
                            var statsB = teamStats[b];
                            int pointsA = teamPoints.ContainsKey(a) ? teamPoints[a] : 0;
                            int pointsB = teamPoints.ContainsKey(b) ? teamPoints[b] : 0;
                            int pdA = statsA.PointsFor - statsA.PointsAgainst;
                            int pdB = statsB.PointsFor - statsB.PointsAgainst;

                            if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                            if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                            return pdB.CompareTo(pdA); // Sort by point differential descending
                        });

                        foreach (var team in sortedTeams)
                        {
                            var stats = teamStats[team];
                            int points = teamPoints[team];
                            double pct = stats.Wins + stats.Ties > 0 ? (double)(stats.Wins + stats.Ties * 0.5) / (stats.Wins + stats.Losses + stats.Ties) : 0;
                            int pd = stats.PointsFor - stats.PointsAgainst;
                            worksheet.Cells[row, 1].Value = team;
                            worksheet.Cells[row, 2].Value = stats.Wins;
                            worksheet.Cells[row, 3].Value = stats.Losses;
                            worksheet.Cells[row, 4].Value = stats.Ties;
                            worksheet.Cells[row, 5].Value = points;
                            worksheet.Cells[row, 6].Value = pct.ToString("F3");
                            worksheet.Cells[row, 7].Value = stats.PointsFor;
                            worksheet.Cells[row, 8].Value = stats.PointsAgainst;
                            worksheet.Cells[row, 9].Value = pd;
                            for (int col = 1; col <= 9; col++)
                            {
                                worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            row++;
                        }

                        // Auto-fit columns
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();
                        worksheet.Column(4).AutoFit();
                        worksheet.Column(5).AutoFit();
                        worksheet.Column(6).AutoFit();
                        worksheet.Column(7).AutoFit();
                        worksheet.Column(8).AutoFit();
                        worksheet.Column(9).AutoFit();

                        // Create a worksheet for match results
                        var matchesWorksheet = package.Workbook.Worksheets.Add("Matches");
                        matchesWorksheet.Cells[1, 1].Value = "Football Tournament Matches";
                        matchesWorksheet.Cells[1, 1, 1, 4].Merge = true;
                        matchesWorksheet.Cells[1, 1].Style.Font.Bold = true;
                        matchesWorksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Add headers
                        matchesWorksheet.Cells[3, 1].Value = "Match";
                        matchesWorksheet.Cells[3, 2].Value = "Team 1 Score";
                        matchesWorksheet.Cells[3, 3].Value = "Team 2 Score";
                        matchesWorksheet.Cells[3, 4].Value = "Final";
                        for (int col = 1; col <= 4; col++)
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
                            matchesWorksheet.Cells[matchRow, 1].Value = $"{match.Team1} vs {match.Team2}";
                            // Convert nullable int to string, default to empty string if null
                            matchesWorksheet.Cells[matchRow, 2].Value = scores.Score1?.ToString() ?? "";
                            matchesWorksheet.Cells[matchRow, 3].Value = scores.Score2?.ToString() ?? "";
                            matchesWorksheet.Cells[matchRow, 4].Value = scores.Score1.HasValue && scores.Score2.HasValue ? "Yes" : "No";
                            for (int col = 1; col <= 4; col++)
                            {
                                matchesWorksheet.Cells[matchRow, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            }
                            matchRow++;
                        }

                        // Auto-fit columns
                        matchesWorksheet.Column(1).AutoFit();
                        matchesWorksheet.Column(2).AutoFit();
                        matchesWorksheet.Column(3).AutoFit();
                        matchesWorksheet.Column(4).AutoFit();

                        // Save the Excel file
                        FileInfo file = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(file);

                        MessageBox.Show($"Football tournament results exported to {saveFileDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadScheduleFromHistory(string schedule, int numTeams)
        {
            DisplaySchedule(schedule, numTeams);
            // Parse and load any scores and points from the schedule text
            teamPoints.Clear();
            matchScores.Clear();
            for (int i = 1; i <= numTeams; i++)
            {
                string teamName = $"Team {i}";
                teamPoints[teamName] = 0;
            }
            string[] lines = schedule.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int matchIndex = 0;
            bool inSchedule = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("Soccer League Round Robin Schedule") || trimmedLine.StartsWith("Football (Soccer) League Round Robin Schedule"))
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
                        matches.Add((teams[0], teams[1]));
                        // Parse scores if available
                        if (trimmedLine.Contains("Final"))
                        {
                            string[] scoreParts = trimmedLine.Split(new[] { " - " }, StringSplitOptions.None);
                            if (scoreParts.Length == 2)
                            {
                                string score1Str = scoreParts[0].Split(' ').Length > 1 ? scoreParts[0].Split(' ')[1] : "0";
                                string score2Str = scoreParts[1].Split(' ').Length > 1 ? scoreParts[1].Split(' ')[0] : "0";
                                if (int.TryParse(score1Str, out int score1) && int.TryParse(score2Str, out int score2))
                                {
                                    matchScores[matchIndex] = (score1, score2);
                                    UpdatePoints(teams[0], teams[1], score1, score2);
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
        }

        private string GenerateDefaultSchedule(int numTeams)
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine("Football (Soccer) League Round Robin Schedule (" + ((numTeams * (numTeams - 1)) / 2) + " Matches):\n");

            int totalMatches = 0;
            for (int i = 1; i <= numTeams; i++)
            {
                for (int j = i + 1; j <= numTeams; j++)
                {
                    if (totalMatches >= (numTeams * (numTeams - 1)) / 2) break;
                    string team1 = $"Team {i}";
                    string team2 = $"Team {j}";
                    schedule.AppendLine($"{team1} vs {team2}");
                    schedule.AppendLine("    Edit    Score    Note");
                    totalMatches++;
                }
            }

            if (numTeams % 2 != 0)
            {
                schedule.AppendLine("\nNote: Teams with a bye will rest in certain rounds (not shown as matches).");
            }

            return schedule.ToString();
        }

        private void SaveScheduleToHistory(string schedule, int numTeams)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string standings = GenerateStandingsText();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO ScheduleHistory (Timestamp, NumTeams, Schedule, Standings) VALUES (@Timestamp, @NumTeams, @Schedule, @Standings)";
                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Timestamp", DateTime.Parse(timestamp));
                        command.Parameters.AddWithValue("@NumTeams", numTeams);
                        command.Parameters.AddWithValue("@Schedule", schedule);
                        command.Parameters.AddWithValue("@Standings", standings);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving schedule history to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateCurrentSchedule()
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine("Football (Soccer) League Round Robin Schedule (" + matches.Count + " Matches):\n");

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var scores = matchScores[i];
                string displayText = $"{match.Team1} vs {match.Team2}";
                if (scores.Score1.HasValue && scores.Score2.HasValue)
                {
                    displayText = $"{match.Team1} {scores.Score1} - {scores.Score2} {match.Team2} Final";
                }
                schedule.AppendLine(displayText);
                schedule.AppendLine("    Edit    Score    Note");
            }

            if (matches.Count % 2 != 0)
            {
                schedule.AppendLine("\nNote: Teams with a bye will rest in certain rounds (not shown as matches).");
            }

            return schedule.ToString();
        }

        private string GenerateStandingsText()
        {
            StringBuilder standings = new StringBuilder();
            Dictionary<string, (int Wins, int Losses, int Ties, int PointsFor, int PointsAgainst)> teamStats = new Dictionary<string, (int, int, int, int, int)>();
            foreach (var team in teamPoints.Keys)
            {
                teamStats[team] = (0, 0, 0, 0, 0);
            }

            foreach (var match in matchScores)
            {
                int index = match.Key;
                var scores = match.Value;
                if (scores.Score1.HasValue && scores.Score2.HasValue)
                {
                    string team1 = matches[index].Team1;
                    string team2 = matches[index].Team2;

                    if (team1 == team2) continue; // Skip self-matches

                    var stats1 = teamStats[team1];
                    var stats2 = teamStats[team2];

                    int score1 = scores.Score1.Value;
                    int score2 = scores.Score2.Value;

                    stats1.PointsFor += score1;
                    stats1.PointsAgainst += score2;
                    stats2.PointsFor += score2;
                    stats2.PointsAgainst += score1;

                    if (score1 > score2)
                    {
                        stats1.Wins++;
                        stats2.Losses++;
                    }
                    else if (score1 < score2)
                    {
                        stats2.Wins++;
                        stats1.Losses++;
                    }
                    else
                    {
                        stats1.Ties++;
                        stats2.Ties++;
                    }

                    teamStats[team1] = stats1;
                    teamStats[team2] = stats2;
                }
            }

            var sortedTeams = new List<string>(teamStats.Keys);
            sortedTeams.Sort((a, b) =>
            {
                var statsA = teamStats[a];
                var statsB = teamStats[b];
                int pointsA = teamPoints.ContainsKey(a) ? teamPoints[a] : 0;
                int pointsB = teamPoints.ContainsKey(b) ? teamPoints[b] : 0;
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

                foreach (var team in teamPoints)
                {
                    if (team.Value > maxPoints)
                    {
                        maxPoints = team.Value;
                        winners.Clear();
                        winners.Add(team.Key);
                    }
                    else if (team.Value == maxPoints)
                    {
                        winners.Add(team.Key);
                    }
                }

                if (winners.Count == 1)
                {
                    winner = $"Final Winner: {winners[0]} with {maxPoints} points";
                }
                else
                {
                    winner = $"Tie for First: {string.Join(", ", winners)} with {maxPoints} points each";
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
                    // Save number of teams and matches
                    writer.WriteLine(matches.Count); // Number of matches
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var match = matches[i];
                        if (match.Team1 == match.Team2) continue; // Skip self-matches
                        var scores = matchScores[i];
                        writer.WriteLine($"{match.Team1}|{match.Team2}|{scores.Score1}|{scores.Score2}");
                    }

                    // Save team points
                    foreach (var team in teamPoints)
                    {
                        writer.WriteLine($"{team.Key}|{team.Value}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving manually entered scores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSavedScores(int numTeams)
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
                            if (parts.Length == 4 && int.TryParse(parts[2], out int score1) && int.TryParse(parts[3], out int score2))
                            {
                                if (parts[0] != parts[1]) // Skip self-matches
                                {
                                    matchScores[i] = (score1, score2);
                                    UpdatePoints(parts[0], parts[1], score1, score2);
                                }
                            }
                        }

                        // Load team points
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] parts = line.Split('|');
                            if (parts.Length == 2 && int.TryParse(parts[1], out int points))
                            {
                                if (parts[0].StartsWith("Team")) // Ensure only valid team names are loaded
                                {
                                    teamPoints[parts[0]] = points;
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
            dataGridView.Columns.Add("Team", "Team");
            dataGridView.Columns.Add("W", "W");
            dataGridView.Columns.Add("L", "L");
            dataGridView.Columns.Add("T", "T");
            dataGridView.Columns.Add("PCT", "PCT");
            dataGridView.Columns.Add("PF", "PF");
            dataGridView.Columns.Add("PA", "PA");
            dataGridView.Columns.Add("PD", "PD");

            // Calculate team stats based only on manually entered scores
            Dictionary<string, (int Wins, int Losses, int Ties, int PointsFor, int PointsAgainst)> teamStats = new Dictionary<string, (int, int, int, int, int)>();
            foreach (var team in teamPoints.Keys)
            {
                teamStats[team] = (0, 0, 0, 0, 0);
            }

            // Calculate stats from manually entered match scores
            foreach (var match in matchScores)
            {
                int index = match.Key;
                var scores = match.Value;
                if (scores.Score1.HasValue && scores.Score2.HasValue) // Only use user-entered scores
                {
                    string team1 = matches[index].Team1;
                    string team2 = matches[index].Team2;

                    if (team1 == team2) continue; // Skip self-matches

                    var stats1 = teamStats[team1];
                    var stats2 = teamStats[team2];

                    int score1 = scores.Score1.Value;
                    int score2 = scores.Score2.Value;

                    stats1.PointsFor += score1;
                    stats1.PointsAgainst += score2;
                    stats2.PointsFor += score2;
                    stats2.PointsAgainst += score1;

                    if (score1 > score2)
                    {
                        stats1.Wins++;
                        stats2.Losses++;
                    }
                    else if (score1 < score2)
                    {
                        stats2.Wins++;
                        stats1.Losses++;
                    }
                    else
                    {
                        stats1.Ties++;
                        stats2.Ties++;
                    }

                    teamStats[team1] = stats1;
                    teamStats[team2] = stats2;
                }
            }

            // Sort teams by points (descending), then wins, then point differential
            var sortedTeams = new List<string>(teamStats.Keys);
            sortedTeams.Sort((a, b) =>
            {
                var statsA = teamStats[a];
                var statsB = teamStats[b];
                int pointsA = teamPoints.ContainsKey(a) ? teamPoints[a] : 0;
                int pointsB = teamPoints.ContainsKey(b) ? teamPoints[b] : 0;
                int pdA = statsA.PointsFor - statsA.PointsAgainst;
                int pdB = statsB.PointsFor - statsB.PointsAgainst;

                if (pointsA != pointsB) return pointsB.CompareTo(pointsA); // Sort by points descending
                if (statsA.Wins != statsB.Wins) return statsB.Wins.CompareTo(statsA.Wins); // Sort by wins descending
                return pdB.CompareTo(pdA); // Sort by point differential descending
            });

            // Add standings rows
            dataGridView.Rows.Add("Standings"); // Header
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView.Rows[dataGridView.Rows.Count - 1].ReadOnly = true;

            foreach (var team in sortedTeams)
            {
                var stats = teamStats[team];
                double pct = stats.Wins + stats.Ties > 0 ? (double)(stats.Wins + stats.Ties * 0.5) / (stats.Wins + stats.Losses + stats.Ties) : 0;
                int pd = stats.PointsFor - stats.PointsAgainst;
                dataGridView.Rows.Add(team, stats.Wins, stats.Losses, stats.Ties, pct.ToString("F3"), stats.PointsFor, stats.PointsAgainst, pd);
                dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.LimeGreen;
            }

            // Add Schedule section
            dataGridView.Rows.Add(""); // Spacer
            dataGridView.Rows.Add("Schedule"); // Header
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView.Rows[dataGridView.Rows.Count - 1].ReadOnly = true;

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.Team1 == match.Team2) continue; // Skip self-matches

                var scores = matchScores[i];
                string displayText = $"{match.Team1} vs {match.Team2}";
                if (scores.Score1.HasValue && scores.Score2.HasValue) // Only display manually entered scores
                {
                    displayText = $"{match.Team1} {scores.Score1} - {scores.Score2} {match.Team2} Final";
                }
                dataGridView.Rows.Add(displayText);
                dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.LimeGreen;

            }

            // Format columns
            dataGridView.Columns["Team"].Width = 100;
            dataGridView.Columns["W"].Width = 50;
            dataGridView.Columns["L"].Width = 50;
            dataGridView.Columns["T"].Width = 50;
            dataGridView.Columns["PCT"].Width = 60;
            dataGridView.Columns["PF"].Width = 50;
            dataGridView.Columns["PA"].Width = 50;
            dataGridView.Columns["PD"].Width = 50;
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ScheduleHistory')
                    BEGIN
                        CREATE TABLE ScheduleHistory (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Timestamp DATETIME NOT NULL,
                            NumTeams INT NOT NULL,
                            Schedule NVARCHAR(MAX) NOT NULL,
                            Standings NVARCHAR(MAX) NOT NULL
                        )
                    END";
                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void UpdatePoints(string team1, string team2, int score1, int score2)
        {
            if (team1 == team2) // Safety check for self-matches
            {
                MessageBox.Show("Teams cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update points based on football scoring: 3 for win, 1 for draw, 0 for loss
            if (score1 > score2)
            {
                teamPoints[team1] += 3; // Win
            }
            else if (score1 < score2)
            {
                teamPoints[team2] += 3; // Win for team2
            }
            else
            {
                teamPoints[team1] += 1; // Draw
                teamPoints[team2] += 1; // Draw
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}