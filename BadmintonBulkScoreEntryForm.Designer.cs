namespace WinFormsApp5
{
    partial class BadmintonBulkScoreEntryForm
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
            bulkScoreGrid = new DataGridView();
            saveAllButton = new RoundedButton();
            cancelButton = new RoundedButton();
            ((System.ComponentModel.ISupportInitialize)bulkScoreGrid).BeginInit();
            SuspendLayout();
            // 
            // bulkScoreGrid
            // 
            bulkScoreGrid.AllowUserToAddRows = false;
            bulkScoreGrid.AllowUserToDeleteRows = false;
            bulkScoreGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            bulkScoreGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            bulkScoreGrid.BackgroundColor = Color.FromArgb(240, 240, 245);
            bulkScoreGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            bulkScoreGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            bulkScoreGrid.Location = new Point(18, 21);
            bulkScoreGrid.Margin = new Padding(4, 5, 4, 5);
            bulkScoreGrid.Name = "bulkScoreGrid";
            bulkScoreGrid.RowHeadersVisible = false;
            bulkScoreGrid.RowHeadersWidth = 51;
            bulkScoreGrid.Size = new Size(708, 531);
            bulkScoreGrid.TabIndex = 0;
            // 
            // saveAllButton
            // 
            saveAllButton.BackColor = Color.LimeGreen;
            saveAllButton.FlatStyle = FlatStyle.Flat;
            saveAllButton.ForeColor = Color.White;
            saveAllButton.Location = new Point(468, 563);
            saveAllButton.Margin = new Padding(4, 5, 4, 5);
            saveAllButton.Name = "saveAllButton";
            saveAllButton.Size = new Size(120, 53);
            saveAllButton.TabIndex = 1;
            saveAllButton.Text = "Save All";
            saveAllButton.UseVisualStyleBackColor = false;
            saveAllButton.Click += saveAllButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.Gray;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(597, 563);
            cancelButton.Margin = new Padding(4, 5, 4, 5);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(120, 53);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = false;
            cancelButton.Click += cancelButton_Click;
            // 
            // BadmintonBulkScoreEntryForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 60);
            ClientSize = new Size(744, 637);
            Controls.Add(cancelButton);
            Controls.Add(saveAllButton);
            Controls.Add(bulkScoreGrid);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BadmintonBulkScoreEntryForm";
            Text = "Enter Badminton Scores (Bulk)";
            ((System.ComponentModel.ISupportInitialize)bulkScoreGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView bulkScoreGrid;
        private WinFormsApp5.RoundedButton saveAllButton;
        private WinFormsApp5.RoundedButton cancelButton;
    }
}