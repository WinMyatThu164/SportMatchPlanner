namespace WinFormsApp5
{
    partial class BadmintonScoreEntryForm
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
            this.matchLabel = new System.Windows.Forms.Label();
            this.team1ScoreTextBox = new System.Windows.Forms.TextBox();
            this.team2ScoreTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new WinFormsApp5.RoundedButton();
            this.cancelButton = new WinFormsApp5.RoundedButton();
            this.SuspendLayout();
            // 
            // matchLabel
            // 
            this.matchLabel.AutoSize = true;
            this.matchLabel.ForeColor = System.Drawing.Color.LimeGreen;
            this.matchLabel.Location = new System.Drawing.Point(12, 20);
            this.matchLabel.Name = "matchLabel";
            this.matchLabel.Size = new System.Drawing.Size(0, 13);
            this.matchLabel.TabIndex = 0;
            // 
            // team1ScoreTextBox
            // 
            this.team1ScoreTextBox.Location = new System.Drawing.Point(15, 50);
            this.team1ScoreTextBox.Name = "team1ScoreTextBox";
            this.team1ScoreTextBox.Size = new System.Drawing.Size(100, 20);
            this.team1ScoreTextBox.TabIndex = 1;
            this.team1ScoreTextBox.ForeColor = System.Drawing.Color.LimeGreen;
            this.team1ScoreTextBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 60);
            // 
            // team2ScoreTextBox
            // 
            this.team2ScoreTextBox.Location = new System.Drawing.Point(125, 50);
            this.team2ScoreTextBox.Name = "team2ScoreTextBox";
            this.team2ScoreTextBox.Size = new System.Drawing.Size(100, 20);
            this.team2ScoreTextBox.TabIndex = 2;
            this.team2ScoreTextBox.ForeColor = System.Drawing.Color.LimeGreen;
            this.team2ScoreTextBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 60);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.LimeGreen;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(15, 80);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 30);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Gray;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(105, 80);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // BadmintonScoreEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.ClientSize = new System.Drawing.Size(240, 120);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.team2ScoreTextBox);
            this.Controls.Add(this.team1ScoreTextBox);
            this.Controls.Add(this.matchLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BadmintonScoreEntryForm";
            this.Text = "Enter Badminton Scores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label matchLabel;
        private System.Windows.Forms.TextBox team1ScoreTextBox;
        private System.Windows.Forms.TextBox team2ScoreTextBox;
        private WinFormsApp5.RoundedButton saveButton;
        private WinFormsApp5.RoundedButton cancelButton;
    }
}