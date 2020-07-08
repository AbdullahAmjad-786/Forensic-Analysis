using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SQLite;
using System.IO;
using System.Xml;

namespace DarkDemo
{
    public partial class Daraz : Form
    {
        public string DARAZ_PATH = "";
        string button_selected = "";
        public Daraz()
        {
            InitializeComponent();
        }

        public Daraz(string name)
        {
            InitializeComponent();
            char[] delims = new[] { '\r', '\n' };
            string text = File.ReadAllText("Configurations.txt");
            string[] values = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            string[] path = values[1].Split('=');
            DARAZ_PATH = path[1];
            button_selected = name;
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Daraz_Menu menu = new Daraz_Menu();
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
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\com.google.android.gms.signin.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Search Details")
                {
                    lblName.Text = button_selected;
                    DataSet data = new DataSet();
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\search_history_storage.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Trader Accounts")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                    conn.Open();
                    query = "Select ACCOUNT_ID,DATA From account";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Conversations")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                    conn.Open();
                    query = "Select Summary,body From Message";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Daraz Orders")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\message_accs_db";
                    conn.Open();
                    query = "Select message,create_time From Message";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Device and App Info")
                {
                    lblName.Text = button_selected;
                    DataSet data = new DataSet();
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\ACCS_SDK.xml");
                    DataTable dataTable = data.Tables[0];
                    for(int i=0;i<data.Tables.Count;i++)
                        dataTable.Merge(data.Tables[i]);
                    
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\whitelabel_prefs.xml");
                    for (int i = 0; i < data.Tables.Count; i++)
                        dataTable.Merge(data.Tables[i]);

                    dgv_category.DataSource = dataTable;                
                }

                //dgv_category.Visible = true;
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
            for (int i = 0; i < 100; i++)
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
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            lblFetch.Visible = false;
            dgv_category.Visible = true;
        }
    }
}
