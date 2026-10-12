namespace WinFormsApp5
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private bool sidebarVisible = true; // Track sidebar state

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            sidebarPanel = new Panel();
            button2 = new Button();
            button1 = new Button();
            btnHome = new Button();
            btnMatches = new Button();
            btnTeams = new Button();
            panel2 = new Panel();
            btnToggleSidebar = new Button();
            label1 = new Label();
            sidebarPanel.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.DarkSlateGray;
            sidebarPanel.Controls.Add(button2);
            sidebarPanel.Controls.Add(button1);
            sidebarPanel.Controls.Add(btnHome);
            sidebarPanel.Controls.Add(btnMatches);
            sidebarPanel.Controls.Add(btnTeams);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(200, 733);
            sidebarPanel.TabIndex = 0;
            sidebarPanel.Paint += sidebarPanel_Paint;
            // 
            // button2
            // 
            button2.ForeColor = Color.White;
            button2.Location = new Point(10, 213);
            button2.Name = "button2";
            button2.Size = new Size(180, 40);
            button2.TabIndex = 5;
            button2.Text = "🏸 Badminton";
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.ForeColor = Color.White;
            button1.Location = new Point(10, 165);
            button1.Name = "button1";
            button1.Size = new Size(180, 40);
            button1.TabIndex = 4;
            button1.Text = "🏐 CaneBall";
            button1.Click += button1_Click;
            // 
            // btnHome
            // 
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(10, 14);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(180, 40);
            btnHome.TabIndex = 1;
            btnHome.Text = "🏠 Home";
            btnHome.Click += BtnHome_Click;
            // 
            // btnMatches
            // 
            btnMatches.ForeColor = Color.White;
            btnMatches.Location = new Point(10, 64);
            btnMatches.Name = "btnMatches";
            btnMatches.Size = new Size(180, 40);
            btnMatches.TabIndex = 2;
            btnMatches.Text = "⚽ Football";
            btnMatches.Click += BtnMatches_Click;
            // 
            // btnTeams
            // 
            btnTeams.ForeColor = Color.White;
            btnTeams.Location = new Point(10, 114);
            btnTeams.Name = "btnTeams";
            btnTeams.Size = new Size(180, 40);
            btnTeams.TabIndex = 3;
            btnTeams.Text = "🏆 Chess";
            btnTeams.Click += BtnTeams_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 192, 192);
            panel2.Controls.Add(btnToggleSidebar);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(200, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1169, 72);
            panel2.TabIndex = 0;
            panel2.Paint += panel2_Paint;
            // 
            // btnToggleSidebar
            // 
            btnToggleSidebar.BackColor = Color.Green;
            btnToggleSidebar.ForeColor = Color.Black;
            btnToggleSidebar.Location = new Point(18, 12);
            btnToggleSidebar.Name = "btnToggleSidebar";
            btnToggleSidebar.Size = new Size(50, 40);
            btnToggleSidebar.TabIndex = 0;
            btnToggleSidebar.Text = "☰";
            btnToggleSidebar.UseVisualStyleBackColor = false;
            btnToggleSidebar.Click += BtnToggleSidebar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Script MT Bold", 72F, FontStyle.Bold | FontStyle.Strikeout, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Red;
            label1.Image = (Image)resources.GetObject("label1.Image");
            label1.Location = new Point(585, 446);
            label1.Name = "label1";
            label1.Size = new Size(1139, 144);
            label1.TabIndex = 2;
            label1.Text = "Sports Match Planner";
            label1.Click += label1_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1369, 733);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(sidebarPanel);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            IsMdiContainer = true;
            Name = "Form1";
            Text = "Sports Match Planner";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            sidebarPanel.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel sidebarPanel;
        private Button btnHome;
        private Button btnMatches;
        private Button btnTeams;
        private Panel panel2;
        private Button btnToggleSidebar;
        private Button button2;
        private Button button1;
        private Label label1;
    }
}
