using System;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ScoreEntryForm : Form
    {
        private string team1;
        private string team2;
        private ScheduleForm parentForm;
        private int matchIndex;

        public ScoreEntryForm(string team1, string team2, ScheduleForm parent, int index)
        {
            InitializeComponent();
            this.team1 = team1;
            this.team2 = team2;
            this.parentForm = parent;
            this.matchIndex = index;

            // Set form title and labels
            this.Text = $"Game {index + 1}";
            team1Label.Text = team1;
            team2Label.Text = team2;

            // Add a label to remind users to enter scores manually
            Label manualEntryLabel = new Label
            {
                Text = "Please enter scores manually for each team.",
                ForeColor = Color.Gray,
                Location = new Point(12, 100),
                AutoSize = true
            };
            this.Controls.Add(manualEntryLabel);

            // Load existing manually entered scores if any
            var existingScores = parentForm.GetMatchScores(index);
            if (existingScores.Score1.HasValue)
            {
                team1ScoreTextBox.Text = existingScores.Score1.Value.ToString();
                CheckIfFinal(); // Check if scores are final
            }
            if (existingScores.Score2.HasValue)
            {
                team2ScoreTextBox.Text = existingScores.Score2.Value.ToString();
                CheckIfFinal(); // Check if scores are final
            }

            // Ensure teams are different
            if (team1 == team2)
            {
                MessageBox.Show("Teams cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (team1 == team2)
            {
                MessageBox.Show("Teams cannot play against themselves. Please regenerate the schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(team1ScoreTextBox.Text) || string.IsNullOrWhiteSpace(team2ScoreTextBox.Text))
            {
                MessageBox.Show("Please enter scores for both teams manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(team1ScoreTextBox.Text, out int team1Score) && int.TryParse(team2ScoreTextBox.Text, out int team2Score))
            {
                if (team1Score < 0 || team2Score < 0)
                {
                    MessageBox.Show("Scores cannot be negative.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Only save manually entered scores by the user
                parentForm.UpdateMatchScore(matchIndex, team1, team2, team1Score, team2Score);
                finalLabel.Visible = true; // Show "Final" label after saving
                team1ScoreTextBox.ReadOnly = true; // Make text boxes read-only after saving
                team2ScoreTextBox.ReadOnly = true;
                MessageBox.Show($"Manually entered scores for {team1} vs {team2} saved as final: {team1Score} - {team2Score}", "Score Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Ensure the form closes after saving
            }
            else
            {
                MessageBox.Show("Please enter valid scores (numbers only) manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the form when Cancel is clicked
        }

        private void CheckIfFinal()
        {
            var existingScores = parentForm.GetMatchScores(matchIndex);
            if (existingScores.Score1.HasValue && existingScores.Score2.HasValue)
            {
                finalLabel.Visible = true; // Show "Final" if scores exist
                team1ScoreTextBox.ReadOnly = true; // Make text boxes read-only if scores are final
                team2ScoreTextBox.ReadOnly = true;
            }
            else
            {
                finalLabel.Visible = false; // Hide "Final" if no scores or incomplete
                team1ScoreTextBox.ReadOnly = false; // Ensure text boxes are editable if no final scores
                team2ScoreTextBox.ReadOnly = false;
            }
        }
    }
}