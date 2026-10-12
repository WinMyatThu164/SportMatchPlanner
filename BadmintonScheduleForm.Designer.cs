namespace WinFormsApp5
{
    partial class BadmintonScheduleForm
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
            dataGridView = new DataGridView();
            winnerLabel = new Label();
            cancelButton = new RoundedButton();
            enterAllScoresButton = new RoundedButton();
            exportToExcelButton = new RoundedButton();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.BackgroundColor = Color.FromArgb(240, 240, 245);
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(18, 21);
            dataGridView.Margin = new Padding(4, 5, 4, 5);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.Size = new Size(900, 708);
            dataGridView.TabIndex = 0;
            dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;
            // 
            // winnerLabel
            // 
            winnerLabel.AutoSize = true;
            winnerLabel.ForeColor = Color.LimeGreen;
            winnerLabel.Location = new Point(18, 740);
            winnerLabel.Margin = new Padding(4, 0, 4, 0);
            winnerLabel.Name = "winnerLabel";
            winnerLabel.Size = new Size(0, 23);
            winnerLabel.TabIndex = 2;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.Gray;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(468, 740);
            cancelButton.Margin = new Padding(4, 5, 4, 5);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(150, 53);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = false;
            cancelButton.Click += cancelButton_Click;
            // 
            // enterAllScoresButton
            // 
            enterAllScoresButton.BackColor = Color.LimeGreen;
            enterAllScoresButton.FlatStyle = FlatStyle.Flat;
            enterAllScoresButton.ForeColor = Color.White;
            enterAllScoresButton.Location = new Point(627, 740);
            enterAllScoresButton.Margin = new Padding(4, 5, 4, 5);
            enterAllScoresButton.Name = "enterAllScoresButton";
            enterAllScoresButton.Size = new Size(150, 53);
            enterAllScoresButton.TabIndex = 4;
            enterAllScoresButton.Text = "Enter All Scores";
            enterAllScoresButton.UseVisualStyleBackColor = false;
            enterAllScoresButton.Click += enterAllScoresButton_Click;
            // 
            // exportToExcelButton
            // 
            exportToExcelButton.BackColor = Color.LimeGreen;
            exportToExcelButton.FlatStyle = FlatStyle.Flat;
            exportToExcelButton.ForeColor = Color.White;
            exportToExcelButton.Location = new Point(786, 740);
            exportToExcelButton.Margin = new Padding(4, 5, 4, 5);
            exportToExcelButton.Name = "exportToExcelButton";
            exportToExcelButton.Size = new Size(150, 53);
            exportToExcelButton.TabIndex = 5;
            exportToExcelButton.Text = "Export to Excel";
            exportToExcelButton.UseVisualStyleBackColor = false;
            exportToExcelButton.Click += exportToExcelButton_Click;
            // 
            // BadmintonScheduleForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            ClientSize = new Size(954, 796);
            Controls.Add(exportToExcelButton);
            Controls.Add(enterAllScoresButton);
            Controls.Add(cancelButton);
            Controls.Add(winnerLabel);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "BadmintonScheduleForm";
            Text = "Badminton Tournament";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label winnerLabel;
        private WinFormsApp5.RoundedButton cancelButton;
        private WinFormsApp5.RoundedButton enterAllScoresButton;
        private WinFormsApp5.RoundedButton exportToExcelButton;
    }
}