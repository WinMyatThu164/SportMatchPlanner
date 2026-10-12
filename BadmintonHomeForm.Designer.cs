namespace WinFormsApp5
{
    partial class BadmintonHomeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            teamsLabel = new Label();
            teamsComboBox = new ComboBox();
            createScheduleButton = new RoundedButton();
            viewHistoryButton = new RoundedButton();
            SuspendLayout();
            // 
            // teamsLabel
            // 
            teamsLabel.AutoSize = true;
            teamsLabel.ForeColor = Color.White;
            teamsLabel.Location = new Point(710, 138);
            teamsLabel.Margin = new Padding(4, 0, 4, 0);
            teamsLabel.Name = "teamsLabel";
            teamsLabel.Size = new Size(149, 23);
            teamsLabel.TabIndex = 0;
            teamsLabel.Text = "Number of Teams:";
            // 
            // teamsComboBox
            // 
            teamsComboBox.BackColor = Color.FromArgb(30, 30, 60);
            teamsComboBox.ForeColor = Color.LimeGreen;
            teamsComboBox.FormattingEnabled = true;
            teamsComboBox.Items.AddRange(new object[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
            teamsComboBox.Location = new Point(710, 212);
            teamsComboBox.Margin = new Padding(4, 5, 4, 5);
            teamsComboBox.Name = "teamsComboBox";
            teamsComboBox.Size = new Size(304, 31);
            teamsComboBox.TabIndex = 1;
            teamsComboBox.SelectedIndexChanged += teamsComboBox_SelectedIndexChanged;
            // 
            // createScheduleButton
            // 
            createScheduleButton.BackColor = Color.LimeGreen;
            createScheduleButton.FlatStyle = FlatStyle.Flat;
            createScheduleButton.ForeColor = Color.White;
            createScheduleButton.Location = new Point(663, 355);
            createScheduleButton.Margin = new Padding(4, 5, 4, 5);
            createScheduleButton.Name = "createScheduleButton";
            createScheduleButton.Size = new Size(180, 42);
            createScheduleButton.TabIndex = 2;
            createScheduleButton.Text = "Create Schedule";
            createScheduleButton.UseVisualStyleBackColor = false;
            createScheduleButton.Click += createScheduleButton_Click;
            // 
            // viewHistoryButton
            // 
            viewHistoryButton.BackColor = Color.LimeGreen;
            viewHistoryButton.FlatStyle = FlatStyle.Flat;
            viewHistoryButton.ForeColor = Color.White;
            viewHistoryButton.Location = new Point(896, 355);
            viewHistoryButton.Margin = new Padding(4, 5, 4, 5);
            viewHistoryButton.Name = "viewHistoryButton";
            viewHistoryButton.Size = new Size(180, 42);
            viewHistoryButton.TabIndex = 3;
            viewHistoryButton.Text = "View History";
            viewHistoryButton.UseVisualStyleBackColor = false;
            viewHistoryButton.Click += viewHistoryButton_Click;
            // 
            // BadmintonHomeForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            ClientSize = new Size(1282, 554);
            Controls.Add(viewHistoryButton);
            Controls.Add(createScheduleButton);
            Controls.Add(teamsComboBox);
            Controls.Add(teamsLabel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BadmintonHomeForm";
            Text = "Badminton Scheduler";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label teamsLabel;
        private System.Windows.Forms.ComboBox teamsComboBox;
        private WinFormsApp5.RoundedButton createScheduleButton;
        private WinFormsApp5.RoundedButton viewHistoryButton;
    }
}