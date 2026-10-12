namespace WinFormsApp5
{
    partial class CaneballHistoryForm
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
            historyListBox = new GreenBorderListBox();
            historyTextBox = new TextBox();
            closeButton = new RoundedButton();
            SuspendLayout();
            // 
            // historyListBox
            // 
            historyListBox.BackColor = Color.FromArgb(30, 30, 60);
            historyListBox.BorderStyle = BorderStyle.None;
            historyListBox.DrawMode = DrawMode.OwnerDrawFixed;
            historyListBox.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            historyListBox.ForeColor = Color.LimeGreen;
            historyListBox.FormattingEnabled = true;
            historyListBox.Location = new Point(18, 21);
            historyListBox.Margin = new Padding(4, 5, 4, 5);
            historyListBox.Name = "historyListBox";
            historyListBox.Size = new Size(300, 529);
            historyListBox.TabIndex = 0;
            historyListBox.SelectedIndexChanged += historyListBox_SelectedIndexChanged;
            // 
            // historyTextBox
            // 
            historyTextBox.BackColor = Color.FromArgb(30, 30, 60);
            historyTextBox.ForeColor = Color.LimeGreen;
            historyTextBox.Location = new Point(327, 21);
            historyTextBox.Margin = new Padding(4, 5, 4, 5);
            historyTextBox.Multiline = true;
            historyTextBox.Name = "historyTextBox";
            historyTextBox.ReadOnly = true;
            historyTextBox.ScrollBars = ScrollBars.Vertical;
            historyTextBox.Size = new Size(598, 528);
            historyTextBox.TabIndex = 1;
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.Gray;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.ForeColor = Color.White;
            closeButton.Location = new Point(456, 563);
            closeButton.Margin = new Padding(4, 5, 4, 5);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(120, 53);
            closeButton.TabIndex = 3;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = false;
            closeButton.Click += closeButton_Click;
            // 
            // CaneballHistoryForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            ClientSize = new Size(945, 637);
            Controls.Add(closeButton);
            Controls.Add(historyTextBox);
            Controls.Add(historyListBox);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CaneballHistoryForm";
            Text = "Caneball History";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private WinFormsApp5.GreenBorderListBox historyListBox;
        private System.Windows.Forms.TextBox historyTextBox;
        private WinFormsApp5.RoundedButton closeButton;
    }
}