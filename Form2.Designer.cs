namespace WinFormsApp5
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            sportLabel = new Label();
            sportComboBox = new GreenBorderComboBox();
            teamsLabel = new Label();
            teamsComboBox = new GreenBorderComboBox();
            scheduleLabel = new Label();
            scheduleComboBox = new GreenBorderComboBox();
            createButton = new RoundedButton();
            panel2 = new Panel();
            viewHistoryButton = new RoundedButton();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // sportLabel
            // 
            sportLabel.AutoSize = true;
            sportLabel.ForeColor = Color.LightGray;
            sportLabel.Location = new Point(603, 74);
            sportLabel.Margin = new Padding(4, 0, 4, 0);
            sportLabel.Name = "sportLabel";
            sportLabel.Size = new Size(51, 23);
            sportLabel.TabIndex = 0;
            sportLabel.Text = "Sport";
            // 
            // sportComboBox
            // 
            sportComboBox.BackColor = Color.FromArgb(30, 30, 60);
            sportComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            sportComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sportComboBox.FlatStyle = FlatStyle.Flat;
            sportComboBox.ForeColor = Color.White;
            sportComboBox.FormattingEnabled = true;
            sportComboBox.Items.AddRange(new object[] { "Soccer" });
            sportComboBox.Location = new Point(603, 127);
            sportComboBox.Margin = new Padding(4, 5, 4, 5);
            sportComboBox.Name = "sportComboBox";
            sportComboBox.Size = new Size(298, 31);
            sportComboBox.TabIndex = 1;
            sportComboBox.SelectedIndexChanged += sportComboBox_SelectedIndexChanged;
            // 
            // teamsLabel
            // 
            teamsLabel.AutoSize = true;
            teamsLabel.ForeColor = Color.LightGray;
            teamsLabel.Location = new Point(603, 198);
            teamsLabel.Margin = new Padding(4, 0, 4, 0);
            teamsLabel.Name = "teamsLabel";
            teamsLabel.Size = new Size(145, 23);
            teamsLabel.TabIndex = 2;
            teamsLabel.Text = "Number of Teams";
            // 
            // teamsComboBox
            // 
            teamsComboBox.BackColor = Color.FromArgb(30, 30, 60);
            teamsComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            teamsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            teamsComboBox.FlatStyle = FlatStyle.Flat;
            teamsComboBox.ForeColor = Color.White;
            teamsComboBox.FormattingEnabled = true;
            teamsComboBox.Items.AddRange(new object[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" });
            teamsComboBox.Location = new Point(603, 251);
            teamsComboBox.Margin = new Padding(4, 5, 4, 5);
            teamsComboBox.Name = "teamsComboBox";
            teamsComboBox.Size = new Size(298, 31);
            teamsComboBox.TabIndex = 3;
            // 
            // scheduleLabel
            // 
            scheduleLabel.AutoSize = true;
            scheduleLabel.ForeColor = Color.LightGray;
            scheduleLabel.Location = new Point(603, 322);
            scheduleLabel.Margin = new Padding(4, 0, 4, 0);
            scheduleLabel.Name = "scheduleLabel";
            scheduleLabel.Size = new Size(139, 23);
            scheduleLabel.TabIndex = 4;
            scheduleLabel.Text = "Type of Schedule";
            // 
            // scheduleComboBox
            // 
            scheduleComboBox.BackColor = Color.FromArgb(30, 30, 60);
            scheduleComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            scheduleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            scheduleComboBox.FlatStyle = FlatStyle.Flat;
            scheduleComboBox.ForeColor = Color.White;
            scheduleComboBox.FormattingEnabled = true;
            scheduleComboBox.Items.AddRange(new object[] { "League Round Robin" });
            scheduleComboBox.Location = new Point(603, 375);
            scheduleComboBox.Margin = new Padding(4, 5, 4, 5);
            scheduleComboBox.Name = "scheduleComboBox";
            scheduleComboBox.Size = new Size(298, 31);
            scheduleComboBox.TabIndex = 5;
            // 
            // createButton
            // 
            createButton.BackColor = Color.White;
            createButton.FlatStyle = FlatStyle.Flat;
            createButton.ForeColor = Color.LimeGreen;
            createButton.Location = new Point(603, 511);
            createButton.Margin = new Padding(4, 5, 4, 5);
            createButton.Name = "createButton";
            createButton.Size = new Size(150, 39);
            createButton.TabIndex = 6;
            createButton.Text = "Create Schedule";
            createButton.UseVisualStyleBackColor = false;
            createButton.Click += createButton_Click;
            // 
            // panel2
            // 
            panel2.BackgroundImageLayout = ImageLayout.Center;
            panel2.Controls.Add(viewHistoryButton);
            panel2.Controls.Add(sportComboBox);
            panel2.Controls.Add(createButton);
            panel2.Controls.Add(sportLabel);
            panel2.Controls.Add(scheduleComboBox);
            panel2.Controls.Add(teamsLabel);
            panel2.Controls.Add(scheduleLabel);
            panel2.Controls.Add(teamsComboBox);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1103, 579);
            panel2.TabIndex = 8;
            // 
            // viewHistoryButton
            // 
            viewHistoryButton.BackColor = Color.LimeGreen;
            viewHistoryButton.FlatStyle = FlatStyle.Flat;
            viewHistoryButton.ForeColor = Color.White;
            viewHistoryButton.Location = new Point(791, 508);
            viewHistoryButton.Margin = new Padding(4, 5, 4, 5);
            viewHistoryButton.Name = "viewHistoryButton";
            viewHistoryButton.Size = new Size(150, 42);
            viewHistoryButton.TabIndex = 9;
            viewHistoryButton.Text = "View History";
            viewHistoryButton.UseVisualStyleBackColor = false;
            viewHistoryButton.Click += viewHistoryButton_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1103, 579);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "Form2";
            Text = "Sports Scheduler";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label sportLabel;
        private WinFormsApp5.GreenBorderComboBox sportComboBox;
        private System.Windows.Forms.Label teamsLabel;
        private WinFormsApp5.GreenBorderComboBox teamsComboBox;
        private System.Windows.Forms.Label scheduleLabel;
        private WinFormsApp5.GreenBorderComboBox scheduleComboBox;
        private WinFormsApp5.RoundedButton createButton;
        private System.Windows.Forms.Panel panel2;
        private WinFormsApp5.RoundedButton viewHistoryButton;
    }
}