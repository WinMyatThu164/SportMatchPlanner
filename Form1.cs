using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        Form2 form2;
        ChessForm cf;
        CaneballHomeForm caneball;
        BadmintonHomeForm baminton;



        public Form1()
        {
            InitializeComponent();
            IsMdiContainer = true; // Set this form as an MDI container



        }
        private void BtnHome_Click(object sender, EventArgs e)
        {
            if (form2 != null)
            {
                form2.Close();
                this.Show();

                label1.Visible = true;


            }
            if (cf != null)
            {
                cf.Close();
                this.Show();

                label1.Visible = true;
            }
            if (caneball != null)
            {
                caneball.Close();
                this.Show();

                label1.Visible = true;
            }
            if (baminton != null)
            {
                baminton.Close();
                this.Show();

                label1.Visible = true;
            }


        }

        private void BtnMatches_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            form2.FormClosed += form2_FormClosed;

            form2.MdiParent = this;
            form2.Dock = DockStyle.Fill;
            form2.Show();
            label1.Visible = false;
        }
        private void form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            form2 = null;
        }
        private void BtnTeams_Click(object sender, EventArgs e)
        {
            cf = new ChessForm();
            cf.FormClosed += cf_FormClosed;

            cf.MdiParent = this;
            cf.Dock = DockStyle.Fill;
            cf.Show();
            label1.Visible = false;
        }
        private void cf_FormClosed(object sender, FormClosedEventArgs e)
        {
            form2 = null;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Settings Clicked!");
        }
        private void BtnToggleSidebar_Click(object sender, EventArgs e)
        {
            if (sidebarVisible)
            {
                sidebarPanel.Width = 70; // Collapse sidebar
                btnToggleSidebar.Text = "➡"; // Change button icon
                btnToggleSidebar.Location = new System.Drawing.Point(5, 10); // Move button position
                foreach (Control control in sidebarPanel.Controls)
                {
                    if (control != btnToggleSidebar)
                        control.Visible = false; // Hide other buttons
                }
            }
            else
            {
                sidebarPanel.Width = 200; // Expand sidebar
                btnToggleSidebar.Text = "☰"; // Restore button icon
                btnToggleSidebar.Location = new System.Drawing.Point(18, 12); // Reset button position
                foreach (Control control in sidebarPanel.Controls)
                {
                    if (control != btnToggleSidebar)
                        control.Visible = true; // Show buttons
                }
            }

            sidebarVisible = !sidebarVisible; // Toggle state
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Football_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            //form2.FormBorderStyle = FormBorderStyle.None;
            //form2.MdiParent = this;

            form2.Show();
        }

        private void sidebarPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            caneball = new CaneballHomeForm();
            caneball.FormClosed += caneball_FormClosed;

            caneball.MdiParent = this;
            caneball.Dock = DockStyle.Fill;
            caneball.Show();
            label1.Visible = false;
        }
        private void caneball_FormClosed(object sender, FormClosedEventArgs e)
        {
            caneball = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baminton = new BadmintonHomeForm();
            baminton.FormClosed += baminton_FormClosed;

            baminton.MdiParent = this;
            baminton.Dock = DockStyle.Fill;
            baminton.Show();
            label1.Visible = false;
        }
        private void baminton_FormClosed(object sender, FormClosedEventArgs e)
        {
            baminton = null;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }

}
