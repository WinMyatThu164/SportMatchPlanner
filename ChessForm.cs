using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class ChessForm : Form
    {
        private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Mgkaung\\C#\\project\\new\\WinFormsApp5\\WinFormsApp5\\FootballScheduleHistory.mdf;Integrated Security=True";

        public ChessForm()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            int numberOfPlayers;
            if (!int.TryParse(playersComboBox.SelectedItem?.ToString(), out numberOfPlayers) || numberOfPlayers < 2)
            {
                MessageBox.Show("Please select a valid number of players (minimum 2).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string schedule = GenerateChessSchedule(numberOfPlayers, "League Round Robin");
            ChessScheduleForm chessForm = new ChessScheduleForm(schedule, numberOfPlayers, ConnectionString);
            chessForm.ShowDialog(); // Or use Show() if you want it non-modal
        }

        private string GenerateChessSchedule(int numPlayers, string scheduleType)
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine($"Chess {scheduleType} Schedule ({numPlayers} Players):\n");

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

        private void viewHistoryButton_Click(object sender, EventArgs e)
        {
            bool hasHistory = CheckHistoryExists();
            if (hasHistory)
            {
                using (ChessHistoryForm historyForm = new ChessHistoryForm(this, ConnectionString))
                {
                    historyForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No history available for Chess schedules. Create and save a schedule to view history.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckHistoryExists()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM ChessScheduleHistory";
                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking Chess history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}