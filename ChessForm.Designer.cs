namespace WinFormsApp5
{
    partial class ChessForm
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
            this.playersLabel = new System.Windows.Forms.Label();
            this.playersComboBox = new WinFormsApp5.GreenBorderComboBox();
            this.createButton = new WinFormsApp5.RoundedButton();
            this.viewHistoryButton = new WinFormsApp5.RoundedButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.ForeColor = System.Drawing.Color.LightGray;
            this.playersLabel.Location = new System.Drawing.Point(603, 127);
            this.playersLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(99, 23);
            this.playersLabel.TabIndex = 0;
            this.playersLabel.Text = "Number of Players";
            // 
            // playersComboBox
            // 
            this.playersComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.playersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.playersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playersComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playersComboBox.ForeColor = System.Drawing.Color.White;
            this.playersComboBox.FormattingEnabled = true;
            this.playersComboBox.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.playersComboBox.Location = new System.Drawing.Point(603, 180);
            this.playersComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.playersComboBox.Name = "playersComboBox";
            this.playersComboBox.Size = new System.Drawing.Size(298, 31);
            this.playersComboBox.TabIndex = 1;
            // 
            // createButton
            // 
            this.createButton.BackColor = System.Drawing.Color.LimeGreen;
            this.createButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createButton.ForeColor = System.Drawing.Color.White;
            this.createButton.Location = new System.Drawing.Point(603, 316);
            this.createButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(150, 39);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Create Schedule";
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // viewHistoryButton
            // 
            this.viewHistoryButton.BackColor = System.Drawing.Color.LimeGreen;
            this.viewHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewHistoryButton.ForeColor = System.Drawing.Color.White;
            this.viewHistoryButton.Location = new System.Drawing.Point(791, 316);
            this.viewHistoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewHistoryButton.Name = "viewHistoryButton";
            this.viewHistoryButton.Size = new System.Drawing.Size(150, 39);
            this.viewHistoryButton.TabIndex = 3;
            this.viewHistoryButton.Text = "View History";
            this.viewHistoryButton.UseVisualStyleBackColor = false;
            this.viewHistoryButton.Click += new System.EventHandler(this.viewHistoryButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.viewHistoryButton);
            this.panel1.Controls.Add(this.playersComboBox);
            this.panel1.Controls.Add(this.createButton);
            this.panel1.Controls.Add(this.playersLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1103, 579);
            this.panel1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)30))), ((int)(((byte)30))), ((int)(((byte)60))));
            this.ClientSize = new System.Drawing.Size(1103, 579);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Chess Scheduler";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label playersLabel;
        private WinFormsApp5.GreenBorderComboBox playersComboBox;
        private WinFormsApp5.RoundedButton createButton;
        private WinFormsApp5.RoundedButton viewHistoryButton;
        private System.Windows.Forms.Panel panel1;
    }
}