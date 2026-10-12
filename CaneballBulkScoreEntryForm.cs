using System;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class CaneballBulkScoreEntryForm : Form
    {
        private CaneballScheduleForm parentForm;
        private int numTeams;

        public CaneballBulkScoreEntryForm(CaneballScheduleForm parent, int numTeams)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.numTeams = numTeams;
            LoadMatches();
        }

        private void LoadMatches()
        {
            // Clear existing rows and columns
            bulkScoreGrid.Rows.Clear();
            bulkScoreGrid.Columns.Clear();

            // Add columns matching the design
            bulkScoreGrid.Columns.Add("Match", "Match");
            bulkScoreGrid.Columns.Add("Team1Score", "Team 1");
            bulkScoreGrid.Columns.Add("Team2Score", "Team 2");
            DataGridViewCheckBoxColumn finalColumn = new DataGridViewCheckBoxColumn();
            finalColumn.Name = "Final";
            finalColumn.HeaderText = "Final";
            finalColumn.Width = 50;
            bulkScoreGrid.Columns.Add(finalColumn);

            bulkScoreGrid.Columns["Match"].Width = 200;
            bulkScoreGrid.Columns["Team1Score"].Width = 80;
            bulkScoreGrid.Columns["Team2Score"].Width = 80;

            // Set DataGridView styling
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
                string matchText = $"{match.Team1} vs {match.Team2}";
                bool isFinal = scores.Score1.HasValue && scores.Score2.HasValue;
                bulkScoreGrid.Rows.Add(matchText, scores.Score1?.ToString() ?? "", scores.Score2?.ToString() ?? "", isFinal);
            }

            // Make score columns editable
            bulkScoreGrid.Columns["Team1Score"].ReadOnly = false;
            bulkScoreGrid.Columns["Team2Score"].ReadOnly = false;
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
                    string[] teams = matchText.Split(new[] { " vs " }, StringSplitOptions.None);
                    if (teams.Length != 2 || teams[0] == teams[1])
                    {
                        MessageBox.Show($"Invalid match at row {i + 1}: {matchText}. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string team1ScoreText = bulkScoreGrid.Rows[i].Cells[1].Value?.ToString() ?? "";
                    string team2ScoreText = bulkScoreGrid.Rows[i].Cells[2].Value?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(team1ScoreText) || string.IsNullOrWhiteSpace(team2ScoreText))
                    {
                        MessageBox.Show($"Please enter scores for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (int.TryParse(team1ScoreText, out int team1Score) && int.TryParse(team2ScoreText, out int team2Score))
                    {
                        // Caneball scoring: assume 0-10 scale for performance points or 1/0 for wins/losses
                        bool isFinal = (bool)bulkScoreGrid.Rows[i].Cells[3].Value;
                        if (isFinal)
                        {
                            parentForm.UpdateMatchScore(i, teams[0], teams[1], team1Score, team2Score);
                            bulkScoreGrid.Rows[i].Cells[3].Value = true; // Ensure checkbox remains checked
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Please enter valid numeric scores (e.g., 0-10 for performance or 1/0 for wins/losses) for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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