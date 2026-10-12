using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using WinFormsApp5;

namespace WinFormsApp5
{
    public partial class CaneballHomeForm : Form
    {
        private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Mgkaung\\C#\\project\\new\\WinFormsApp5\\WinFormsApp5\\FootballScheduleHistory.mdf;Integrated Security=True";

        public CaneballHomeForm()
        {
            InitializeComponent();
        }

        private void createScheduleButton_Click(object sender, EventArgs e)
        {
            int numberOfTeams;
            if (!int.TryParse(teamsComboBox.SelectedItem?.ToString(), out numberOfTeams) || numberOfTeams < 2 || numberOfTeams % 2 != 0)
            {
                MessageBox.Show("Please select a valid even number of teams (minimum 2, typically 6 for Caneball).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string schedule = GenerateCaneballSchedule(numberOfTeams, "Round Robin");
            CaneballScheduleForm caneballForm = new CaneballScheduleForm(schedule, numberOfTeams, ConnectionString);
            caneballForm.ShowDialog(); // Or use Show() if you want it non-modal
        }

        private string GenerateCaneballSchedule(int numTeams, string scheduleType)
        {
            StringBuilder schedule = new StringBuilder();
            schedule.AppendLine($"Caneball {scheduleType} Schedule ({numTeams} Teams):\n");

            int totalMatches = (numTeams * (numTeams - 1)) / 2;
            int matchCount = 0;

            for (int i = 1; i <= numTeams; i++)
            {
                for (int j = i + 1; j <= numTeams; j++)
                {
                    if (matchCount >= totalMatches) break;
                    string team1 = $"Team {i}";
                    string team2 = $"Team {j}";
                    schedule.AppendLine($"{team1} vs {team2}");
                    schedule.AppendLine("    Edit    Score    Note");
                    matchCount++;
                }
            }

            if (numTeams % 2 != 0)
            {
                schedule.AppendLine("\nNote: Teams with a bye will rest in certain rounds (not shown as matches).");
            }

            return schedule.ToString();
        }

        private void viewHistoryButton_Click(object sender, EventArgs e)
        {
            bool hasHistory = CheckHistoryExists();
            if (hasHistory)
            {
                using (CaneballHistoryForm historyForm = new CaneballHistoryForm(this, ConnectionString))
                {
                    historyForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No history available for Caneball schedules. Create and save a schedule to view history.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckHistoryExists()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM CaneballScheduleHistory";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error checking Caneball history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking Caneball history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void teamsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}