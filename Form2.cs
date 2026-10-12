using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class Form2 : Form
    {
        private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Mgkaung\\C#\\project\\new\\WinFormsApp5\\WinFormsApp5\\FootballScheduleHistory.mdf;Integrated Security=True"; // Define ConnectionString here

        public Form2()
        {
            InitializeComponent();
        }

        private void sportComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Add logic here if needed for sport selection changes
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            string selectedSport = sportComboBox.SelectedItem?.ToString() ?? "Soccer"; // Default to "Soccer" if not selected
            if (string.IsNullOrEmpty(selectedSport) || selectedSport == "Select your sport...")
            {
                MessageBox.Show("Please select a sport.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int numberOfTeams;
            if (!int.TryParse(teamsComboBox.SelectedItem?.ToString(), out numberOfTeams) || numberOfTeams < 2)
            {
                MessageBox.Show("Please select a valid number of teams (minimum 2).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedSchedule = scheduleComboBox.SelectedItem?.ToString() ?? "League Round Robin";
            if (string.IsNullOrEmpty(selectedSchedule) || selectedSchedule == "Select type of schedule...")
            {
                MessageBox.Show("Please select a type of schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedSport == "Soccer")
            {
                string schedule = GenerateFootballSchedule(numberOfTeams, selectedSchedule);
                ScheduleForm scheduleForm = new ScheduleForm(schedule, numberOfTeams);
                scheduleForm.ShowDialog(); // Or use Show() if you want it non-modal, depending on MDI setup
            }
            else if (selectedSport == "Chess")
            {
                string schedule = GenerateChessSchedule(numberOfTeams, selectedSchedule);
                ChessScheduleForm chessForm = new ChessScheduleForm(schedule, numberOfTeams);
                chessForm.MdiParent = this; // If using MDI, ensure Form1 is set as MDI container
                chessForm.Show();
            }
        }

        private string GenerateFootballSchedule(int numTeams, string scheduleType)
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine($"Football (Soccer) {scheduleType} Schedule (" + ((numTeams * (numTeams - 1)) / 2) + " Matches):\n");

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
            // Check if there’s any football (Soccer) history in the database
            bool hasHistory = CheckHistoryExists();
            if (hasHistory)
            {
                using (ScheduleHistoryForm historyForm = new ScheduleHistoryForm(this, ConnectionString))
                {
                    historyForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No history available for Soccer schedules. Create and save a schedule to view history.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckHistoryExists()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM ScheduleHistory WHERE Schedule LIKE '%Football (Soccer)%'";
                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}