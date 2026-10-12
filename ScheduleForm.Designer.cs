namespace WinFormsApp5
{
    partial class ScheduleForm
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
            bulkScoreButton = new RoundedButton();
            exportToExcelButton = new RoundedButton();
            close = new RoundedButton();
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
            dataGridView.CellContentClick += dataGridView_CellContentClick;
            dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;
            // 
            // winnerLabel
            // 
            winnerLabel.AutoSize = true;
            winnerLabel.ForeColor = Color.LightGray;
            winnerLabel.Location = new Point(18, 740);
            winnerLabel.Margin = new Padding(4, 0, 4, 0);
            winnerLabel.Name = "winnerLabel";
            winnerLabel.Size = new Size(0, 23);
            winnerLabel.TabIndex = 2;
            // 
            // bulkScoreButton
            // 
            bulkScoreButton.BackColor = Color.LimeGreen;
            bulkScoreButton.FlatStyle = FlatStyle.Flat;
            bulkScoreButton.ForeColor = Color.White;
            bulkScoreButton.Location = new Point(596, 739);
            bulkScoreButton.Margin = new Padding(4, 5, 4, 5);
            bulkScoreButton.Name = "bulkScoreButton";
            bulkScoreButton.Size = new Size(150, 42);
            bulkScoreButton.TabIndex = 3;
            bulkScoreButton.Text = "Enter All Scores";
            bulkScoreButton.UseVisualStyleBackColor = false;
            bulkScoreButton.Click += bulkScoreButton_Click;
            // 
            // exportToExcelButton
            // 
            exportToExcelButton.BackColor = Color.LimeGreen;
            exportToExcelButton.FlatStyle = FlatStyle.Flat;
            exportToExcelButton.ForeColor = Color.White;
            exportToExcelButton.Location = new Point(756, 739);
            exportToExcelButton.Margin = new Padding(4, 5, 4, 5);
            exportToExcelButton.Name = "exportToExcelButton";
            exportToExcelButton.Size = new Size(150, 43);
            exportToExcelButton.TabIndex = 4;
            exportToExcelButton.Text = "Export to Excel";
            exportToExcelButton.UseVisualStyleBackColor = false;
            exportToExcelButton.Click += exportToExcelButton_Click;
            // 
            // close
            // 
            close.BackColor = Color.LimeGreen;
            close.FlatStyle = FlatStyle.Flat;
            close.ForeColor = Color.White;
            close.Location = new Point(485, 739);
            close.Margin = new Padding(4, 5, 4, 5);
            close.Name = "close";
            close.Size = new Size(98, 43);
            close.TabIndex = 5;
            close.Text = "Close";
            close.UseVisualStyleBackColor = false;
            close.Click += close_Click;
            // 
            // ScheduleForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            ClientSize = new Size(936, 796);
            Controls.Add(close);
            Controls.Add(exportToExcelButton);
            Controls.Add(bulkScoreButton);
            Controls.Add(winnerLabel);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "ScheduleForm";
            Text = "Football League";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label winnerLabel;
        private WinFormsApp5.RoundedButton bulkScoreButton;
        private WinFormsApp5.RoundedButton exportToExcelButton;
        private RoundedButton close;
    }
}