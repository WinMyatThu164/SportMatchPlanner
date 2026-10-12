namespace WinFormsApp5
{
    partial class ScoreEntryForm
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
            team1Label = new Label();
            team2Label = new Label();
            team1ScoreTextBox = new TextBox();
            team2ScoreTextBox = new TextBox();
            finalLabel = new Label();
            gameLabel = new Label();
            scoreLabel = new Label();
            noteLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // team1Label
            // 
            team1Label.AutoSize = true;
            team1Label.ForeColor = Color.LimeGreen;
            team1Label.Location = new Point(18, 35);
            team1Label.Margin = new Padding(4, 0, 4, 0);
            team1Label.Name = "team1Label";
            team1Label.Size = new Size(64, 23);
            team1Label.TabIndex = 0;
            team1Label.Text = "Team 1";
            // 
            // team2Label
            // 
            team2Label.AutoSize = true;
            team2Label.ForeColor = Color.LimeGreen;
            team2Label.Location = new Point(18, 88);
            team2Label.Margin = new Padding(4, 0, 4, 0);
            team2Label.Name = "team2Label";
            team2Label.Size = new Size(64, 23);
            team2Label.TabIndex = 1;
            team2Label.Text = "Team 6";
            // 
            // team1ScoreTextBox
            // 
            team1ScoreTextBox.Location = new Point(150, 35);
            team1ScoreTextBox.Margin = new Padding(4, 5, 4, 5);
            team1ScoreTextBox.Name = "team1ScoreTextBox";
            team1ScoreTextBox.Size = new Size(73, 30);
            team1ScoreTextBox.TabIndex = 2;
            // 
            // team2ScoreTextBox
            // 
            team2ScoreTextBox.Location = new Point(150, 88);
            team2ScoreTextBox.Margin = new Padding(4, 5, 4, 5);
            team2ScoreTextBox.Name = "team2ScoreTextBox";
            team2ScoreTextBox.Size = new Size(73, 30);
            team2ScoreTextBox.TabIndex = 3;
            // 
            // finalLabel
            // 
            finalLabel.AutoSize = true;
            finalLabel.ForeColor = Color.Gray;
            finalLabel.Location = new Point(240, 62);
            finalLabel.Margin = new Padding(4, 0, 4, 0);
            finalLabel.Name = "finalLabel";
            finalLabel.Size = new Size(45, 23);
            finalLabel.TabIndex = 4;
            finalLabel.Text = "Final";
            finalLabel.Visible = false;
            // 
            // gameLabel
            // 
            gameLabel.AutoSize = true;
            gameLabel.ForeColor = Color.LimeGreen;
            gameLabel.Location = new Point(18, 142);
            gameLabel.Margin = new Padding(4, 0, 4, 0);
            gameLabel.Name = "gameLabel";
            gameLabel.Size = new Size(55, 23);
            gameLabel.TabIndex = 5;
            gameLabel.Text = "Game";
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.ForeColor = Color.LimeGreen;
            scoreLabel.Location = new Point(105, 142);
            scoreLabel.Margin = new Padding(4, 0, 4, 0);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(52, 23);
            scoreLabel.TabIndex = 6;
            scoreLabel.Text = "Score";
            // 
            // noteLabel
            // 
            noteLabel.AutoSize = true;
            noteLabel.ForeColor = Color.LimeGreen;
            noteLabel.Location = new Point(195, 142);
            noteLabel.Margin = new Padding(4, 0, 4, 0);
            noteLabel.Name = "noteLabel";
            noteLabel.Size = new Size(48, 23);
            noteLabel.TabIndex = 7;
            noteLabel.Text = "Note";
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.LimeGreen;
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(22, 195);
            saveButton.Margin = new Padding(4, 5, 4, 5);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(90, 53);
            saveButton.TabIndex = 8;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.Gray;
            cancelButton.Location = new Point(135, 195);
            cancelButton.Margin = new Padding(4, 5, 4, 5);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(90, 53);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // ScoreEntryForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 245);
            ClientSize = new Size(300, 265);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(noteLabel);
            Controls.Add(scoreLabel);
            Controls.Add(gameLabel);
            Controls.Add(finalLabel);
            Controls.Add(team2ScoreTextBox);
            Controls.Add(team1ScoreTextBox);
            Controls.Add(team2Label);
            Controls.Add(team1Label);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ScoreEntryForm";
            Text = "Game 1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label team1Label;
        private System.Windows.Forms.Label team2Label;
        private System.Windows.Forms.TextBox team1ScoreTextBox;
        private System.Windows.Forms.TextBox team2ScoreTextBox;
        private System.Windows.Forms.Label finalLabel;
        private System.Windows.Forms.Label gameLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}