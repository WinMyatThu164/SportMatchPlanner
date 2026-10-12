using System;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class CaneballScoreEntryForm : Form
    {
        private string team1;
        private string team2;
        private CaneballScheduleForm parentForm;
        private int matchIndex;

        public CaneballScoreEntryForm(string team1, string team2, CaneballScheduleForm parent, int matchIndex)
        {
            InitializeComponent();
            this.team1 = team1;
            this.team2 = team2;
            this.parentForm = parent;
            this.matchIndex = matchIndex;
            matchLabel.Text = $"{team1} vs {team2}";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(team1ScoreTextBox.Text, out int score1) && int.TryParse(team2ScoreTextBox.Text, out int score2))
            {
                // Caneball scoring: assume 0-10 scale for performance points or 1/0 for wins/losses
                parentForm.UpdateMatchScore(matchIndex, team1, team2, score1, score2);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric scores (e.g., 0-10 for performance or 1/0 for wins/losses).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}