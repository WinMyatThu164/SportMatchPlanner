using System;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ChessBulkScoreEntryForm : Form
    {
        private ChessScheduleForm parentForm;
        private int numPlayers;
        private bool isChess;

        public ChessBulkScoreEntryForm(ChessScheduleForm parent, int numPlayers, bool isChess = true)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.numPlayers = numPlayers;
            this.isChess = isChess;
            LoadMatches();
        }

        private void LoadMatches()
        {
            // Clear existing rows and columns
            bulkScoreGrid.Rows.Clear();
            bulkScoreGrid.Columns.Clear();

            // Add columns matching the screenshot design
            bulkScoreGrid.Columns.Add("Match", "Match");
            bulkScoreGrid.Columns.Add("Player1", "Player 1");
            bulkScoreGrid.Columns.Add("Player2", "Player 2");
            DataGridViewCheckBoxColumn finalColumn = new DataGridViewCheckBoxColumn();
            finalColumn.Name = "Final";
            finalColumn.HeaderText = "Final";
            finalColumn.Width = 50;
            bulkScoreGrid.Columns.Add(finalColumn);

            bulkScoreGrid.Columns["Match"].Width = 200;
            bulkScoreGrid.Columns["Player1"].Width = 80;
            bulkScoreGrid.Columns["Player2"].Width = 80;

            // Set DataGridView styling to match the screenshot
            bulkScoreGrid.BackgroundColor = System.Drawing.Color.FromArgb(240, 240, 245); // Light blue background
            bulkScoreGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkBlue; // Dark blue headers
            bulkScoreGrid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White; // White text for headers
            bulkScoreGrid.EnableHeadersVisualStyles = false;
            bulkScoreGrid.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen; // Green text for rows
            bulkScoreGrid.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 245); // Light blue for cells

            // Add rows with match data
            for (int i = 0; i < parentForm.Matches.Count; i++)
            {
                var match = parentForm.Matches[i];
                var scores = parentForm.GetMatchScores(i);
                string matchText = $"{match.Player1} vs {match.Player2}";
                bool isFinal = scores.Score1.HasValue && scores.Score2.HasValue;
                bulkScoreGrid.Rows.Add(matchText, scores.Score1?.ToString() ?? "", scores.Score2?.ToString() ?? "", isFinal);
            }

            // Make score columns editable
            bulkScoreGrid.Columns["Player1"].ReadOnly = false;
            bulkScoreGrid.Columns["Player2"].ReadOnly = false;
            bulkScoreGrid.Columns["Match"].ReadOnly = true;
            bulkScoreGrid.Columns["Final"].ReadOnly = false; // Allow users to toggle the checkbox

            // Allow editing on enter
            bulkScoreGrid.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void saveAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < bulkScoreGrid.Rows.Count; i++)
                {
                    string matchText = bulkScoreGrid.Rows[i].Cells[0].Value.ToString();
                    string[] players = matchText.Split(new[] { " vs " }, StringSplitOptions.None);
                    if (players.Length != 2 || players[0] == players[1])
                    {
                        MessageBox.Show($"Invalid match at row {i + 1}: {matchText}. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string player1ScoreText = bulkScoreGrid.Rows[i].Cells[1].Value?.ToString() ?? "";
                    string player2ScoreText = bulkScoreGrid.Rows[i].Cells[2].Value?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(player1ScoreText) || string.IsNullOrWhiteSpace(player2ScoreText))
                    {
                        MessageBox.Show($"Please enter scores for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (double.TryParse(player1ScoreText, out double player1Score) && double.TryParse(player2ScoreText, out double player2Score))
                    {
                        if (isChess && (player1Score != 1 && player1Score != 0 && player1Score != 0.5) || (player2Score != 1 && player2Score != 0 && player2Score != 0.5))
                        {
                            MessageBox.Show($"For Chess, scores must be 1 (win), 0 (loss), or 0.5 (draw) for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (player1Score + player2Score != 1 && isChess) // Ensure Chess scores sum to 1 (e.g., 1-0, 0-1, 0.5-0.5)
                        {
                            MessageBox.Show($"For Chess, the total scores for match at row {i + 1} must sum to 1 (e.g., 1-0, 0-1, or 0.5-0.5): {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Only update scores in ChessScheduleForm if the "Final" checkbox is checked
                        bool isFinal = (bool)bulkScoreGrid.Rows[i].Cells[3].Value;
                        if (isFinal)
                        {
                            parentForm.UpdateMatchScore(i, players[0], players[1], player1Score, player2Score);
                            bulkScoreGrid.Rows[i].Cells[3].Value = true; // Ensure checkbox remains checked
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Please enter valid scores (0, 0.5, or 1) for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("All final scores saved successfully (manually entered).", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving scores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}