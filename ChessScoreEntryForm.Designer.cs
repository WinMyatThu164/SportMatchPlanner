namespace WinFormsApp5
{
    partial class ChessScoreEntryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.player1ScoreTextBox = new System.Windows.Forms.TextBox();
            this.player2ScoreTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.saveButton = new WinFormsApp5.RoundedButton();
            this.cancelButton = new WinFormsApp5.RoundedButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Match";
            // 
            // player1ScoreTextBox
            // 
            this.player1ScoreTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.player1ScoreTextBox.ForeColor = System.Drawing.Color.White;
            this.player1ScoreTextBox.Location = new System.Drawing.Point(15, 50);
            this.player1ScoreTextBox.Name = "player1ScoreTextBox";
            this.player1ScoreTextBox.Size = new System.Drawing.Size(100, 20);
            this.player1ScoreTextBox.TabIndex = 1;
            // 
            // player2ScoreTextBox
            // 
            this.player2ScoreTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.player2ScoreTextBox.ForeColor = System.Drawing.Color.White;
            this.player2ScoreTextBox.Location = new System.Drawing.Point(15, 90);
            this.player2ScoreTextBox.Name = "player2ScoreTextBox";
            this.player2ScoreTextBox.Size = new System.Drawing.Size(100, 20);
            this.player2ScoreTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Player 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Player 2";
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.LimeGreen;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(15, 130);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 30);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.LimeGreen;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(101, 130);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ScoreEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.ClientSize = new System.Drawing.Size(193, 172);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.player2ScoreTextBox);
            this.Controls.Add(this.player1ScoreTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScoreEntryForm";
            this.Text = "Enter Chess Scores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox player1ScoreTextBox;
        private System.Windows.Forms.TextBox player2ScoreTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private WinFormsApp5.RoundedButton saveButton;
        private WinFormsApp5.RoundedButton cancelButton;
    }
}