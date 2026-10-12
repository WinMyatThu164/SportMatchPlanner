using System;
using System.Reflection.Emit;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ChessScoreEntryForm : Form
    {
        private string player1;
        private string player2;
        private ChessScheduleForm parentForm;
        private int matchIndex;
        private bool isChess; // Flag for Chess scoring

        public ChessScoreEntryForm(string player1, string player2, ChessScheduleForm parent, int matchIndex, bool isChess = true)
        {
            InitializeComponent();
            this.player1 = player1;
            this.player2 = player2;
            this.parentForm = parent;
            this.matchIndex = matchIndex;
            this.isChess = isChess;
            label1.Text = $"{player1} vs {player2}";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (double.TryParse(player1ScoreTextBox.Text, out double score1) && double.TryParse(player2ScoreTextBox.Text, out double score2))
            {
                if (isChess && (score1 != 1 && score1 != 0 && score1 != 0.5) || (score2 != 1 && score2 != 0 && score2 != 0.5))
                {
                    MessageBox.Show("For Chess, scores must be 1 (win), 0 (loss), or 0.5 (draw) for each player.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (score1 + score2 != 1 && isChess) // Ensure Chess scores sum to 1 (e.g., 1-0, 0-1, 0.5-0.5)
                {
                    MessageBox.Show("For Chess, the total scores for a match must sum to 1 (e.g., 1-0, 0-1, or 0.5-0.5).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                parentForm.UpdateMatchScore(matchIndex, player1, player2, score1, score2);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric scores.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}