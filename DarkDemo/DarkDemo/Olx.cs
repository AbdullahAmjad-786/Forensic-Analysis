using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace DarkDemo
{
    public partial class Olx : Form
    {
        public string OLX_PATH = "";
        string button_selected = "";
        public Olx()
        {
            InitializeComponent();
        }

        public Olx(string name)
        {
            InitializeComponent();
            char[] delims = new[] { '\r', '\n' };
            string text = File.ReadAllText("Configurations.txt");
            string[] values = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            string[] path = values[0].Split('=');
            OLX_PATH = path[1];
            button_selected = name;
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Olx_Menu menu = new Olx_Menu();
            this.Hide();
            menu.Show();
        }

        void StartProgressBar()
        {
            if (dgv_category.Visible)
                dgv_category.Visible = false;

            lblFetch.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Update();
            backgroundWorker1.RunWorkerAsync();
        }

        void LoadData()
        {
            string query = "";
            DataTable dt = new DataTable();
            StartProgressBar();
            //lblinfo.Visible = true;
            SQLiteConnection conn = new SQLiteConnection();
            try
            {
                if (button_selected == "User Login and Location")
                {
                    lblName.Text = button_selected;
                    DataSet data = new DataSet();
                    data.ReadXml(OLX_PATH + "\\shared_prefs\\panamera_preferences.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Posted Ad's")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select * From Ad";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Conversations")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select body,extras From Message";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Ad's Details")
                {
                    lblName.Text = button_selected;
                    DataSet data = new DataSet();
                    data.ReadXml(OLX_PATH + "\\shared_prefs\\Drafts.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Buyer's info")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select value,phonenumber From Profile";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Olx Attributes")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\delorean_base.db";
                    conn.Open();
                    query = "Select * From attributes_values";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "App Security")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select id,subtitle From SystemMessageMetadata";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }

                //dgv_category.Visible = true;
                //dgv_category.DataSource = dt;
                dgv_category.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv_category.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i=0;i<100;i++)
            {
                if (backgroundWorker1.CancellationPending)
                    e.Cancel = true;
                else
                {
                    Thread.Sleep(30);
                    backgroundWorker1.ReportProgress(i);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ProgressBar.Visible = false;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            lblFetch.Visible = false;
            dgv_category.Visible = true;
        }
    }
}
