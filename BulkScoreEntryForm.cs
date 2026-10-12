using System;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class BulkScoreEntryForm : Form
    {
        private ScheduleForm parentForm;
        private int numTeams;

        public BulkScoreEntryForm(ScheduleForm parent, int numTeams)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.numTeams = numTeams;
            LoadMatches();
        }

        private void LoadMatches()
        {
            bulkScoreGrid.Rows.Clear();
            for (int i = 0; i < parentForm.Matches.Count; i++)
            {
                var match = parentForm.Matches[i];
                var scores = parentForm.GetMatchScores(i);
                string matchText = $"{match.Team1} vs {match.Team2}";
                bulkScoreGrid.Rows.Add(matchText, scores.Score1?.ToString() ?? "", scores.Score2?.ToString() ?? "", scores.Score1.HasValue && scores.Score2.HasValue);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
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
                        if (team1Score < 0 || team2Score < 0)
                        {
                            MessageBox.Show($"Scores cannot be negative for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        parentForm.UpdateMatchScore(i, teams[0], teams[1], team1Score, team2Score);
                        bulkScoreGrid.Rows[i].Cells[3].Value = true; // Mark as final
                    }
                    else
                    {
                        MessageBox.Show($"Please enter valid scores (numbers only) for match at row {i + 1}: {matchText}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("All scores saved successfully (manually entered).", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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