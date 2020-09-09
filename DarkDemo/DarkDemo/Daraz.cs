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

namespace DarkDemo
{
    public partial class Daraz : Form
    {
        public string DARAZ_PATH = "";
        string button_selected = "";
        DataSet data = new DataSet();
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
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\com.google.android.gms.signin.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Search Details")
                {
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\search_history_storage.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Trader Accounts")
                {
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                    conn.Open();
                    query = "Select ACCOUNT_ID,DATA From account";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Conversations")
                {
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                    conn.Open();
                    query = "Select Account_Id From Account";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    getConverstationData(dt);
                    //dgv_category.DataSource = dt;
                }
                if (button_selected == "Daraz Orders")
                {
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\message_accs_db";
                    conn.Open();
                    query = "Select message,create_time From Message";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Device and App Info")
                {
                    dgv_category.DataSource = null;
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\ACCS_SDK.xml");
                    DataTable dataTable = data.Tables[0];
                    for(int i=0;i<data.Tables.Count;i++)
                        dataTable.Merge(data.Tables[i]);
                    
                    data.ReadXml(DARAZ_PATH + "\\shared_prefs\\whitelabel_prefs.xml");
                    for (int i = 0; i < data.Tables.Count; i++)
                        dataTable.Merge(data.Tables[i]);

                    dgv_category.DataSource = dataTable;
                    data = dataTable.DataSet;
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
            if (lblName.Text == "Conversations")
            {
                lblList.Visible = true;
                cmbBuyerList.Visible = true;
                dgv_category.Location = new Point(dgv_category.Location.X, 38);
                dgv_category.Height = 607;
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            try
            {
                if (button_selected == "User Login and Location")
                {
                    LoginBtnSummanry();
                }
                if (button_selected == "Search Details")
                {
                    SearchBtnSummary();
                }
                if (button_selected == "Trader Accounts")
                {
                    TraderAccountBtnSummary();
                }
                if (button_selected == "Daraz Orders")
                {   
                    OrderBtnSummary();
                }
                if (button_selected == "Device and App Info")
                {
                    DeviceAppBtnSummary();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void LoginBtnSummanry()
        {
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Artifacts";
            dgv_category.Columns[1].Name = "Information";
            for (int j = 0; j < data.Tables[0].Rows.Count; j++)
            {
                string[] values = data.Tables[0].Rows[j].ItemArray[1].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']'}).Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    string[] row_text = values[i].Split(':');
                    if(row_text.Length > 2)
                        dgv_category.Rows.Add(row_text[0], row_text[1] + row_text[2]);
                    else if(row_text.Length > 1)
                        dgv_category.Rows.Add(row_text[0], row_text[1]);
                }
            }
        } //end of function

        void TraderAccountBtnSummary()
        {
            string id;
            string[] details = new string[3];
            int count;
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 4;
            dgv_category.Columns[0].Name = "Account_Id";
            dgv_category.Columns[1].Name = "Name";
            dgv_category.Columns[2].Name = "URL";
            dgv_category.Columns[3].Name = "Active Time";
            for (int j = 0; j < data.Tables[0].Rows.Count; j++)
            {
                count = 0;
                id = data.Tables[0].Rows[j].ItemArray[0].ToString();
                string[] values = data.Tables[0].Rows[j].ItemArray[1].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    if (values.Length > 3)
                    {
                        if (i != 0)
                        {
                            string[] row_text = values[i].Split(':');
                            if (row_text.Length > 2)
                                details[count] = row_text[1] + row_text[2];
                            else if (row_text.Length > 1)
                                details[count] = row_text[1];
                            count++;
                        }
                    }
                    else if(values.Length == 3)
                    {
                        string[] row_text = values[i].Split(':');
                        if (row_text.Length > 2)
                            details[count] = row_text[1] + row_text[2];
                        else if (row_text.Length > 1)
                            details[count] = row_text[1];
                        count++;
                    }
                }
                dgv_category.Rows.Add(id, details[1], details[2], details[0]);
            }
        } //end of function

        void SearchBtnSummary()
        {
            string id;
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Search History key";
            dgv_category.Columns[1].Name = "Last_search_history";
            id = data.Tables[0].Rows[0].ItemArray[1].ToString();
            string[] values = data.Tables[0].Rows[1].ItemArray[1].ToString().Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                string txt = values[i].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).ToString();
                if (txt.Contains("searchKey"))
                {
                    dgv_category.Rows.Add(id, txt);
                    id = "";
                }
            }
        } //end of function

        void OrderBtnSummary()
        {
            string id = "";
            string code = "";
            string order_place = "";
            string order_complete = ""; 
            string order_create = "";
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 5;
            dgv_category.Columns[0].Name = "Buyers User Id";
            dgv_category.Columns[1].Name = "Area Code";
            dgv_category.Columns[2].Name = "Orders Placed";
            dgv_category.Columns[3].Name = "Order Completion time by Server";
            dgv_category.Columns[4].Name = "Order Creation Time";
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < data.Tables[0].Rows[i].ItemArray.Length; j++)
                {
                    string[] values = data.Tables[0].Rows[i].ItemArray[j].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                    if(data.Tables[0].Rows[i].ItemArray[j].ToString().Contains("areaCode"))
                    {
                        for(int k=0;k<values.Length;k++)
                        {
                            string[] row_text = values[k].Split(':');
                            if (row_text[0].Contains("sellerId"))
                                id = row_text[1];
                            if (row_text[0].Contains("areaCode"))
                                code = row_text[1];
                        }
                    }
                    else if (data.Tables[0].Rows[i].ItemArray[j].ToString().Contains("body"))
                    {
                        for (int k = 0; k < values.Length; k++)
                        {
                            string[] row_text = values[k].Split(':');
                            if (row_text.Length > 1)
                            {
                                if (row_text[1].Contains("body"))
                                {
                                    order_place = row_text[2];
                                }
                                if (row_text[0].Contains("title"))
                                    order_complete = row_text[1];
                            }
                        }
                    }
                    else
                    {
                        if (values.Length == 1)
                            order_create = values[0];
                    }
                }
                if (order_place != "")
                {
                    dgv_category.Rows.Add(id, code, order_place, order_complete, order_create);
                    order_place = "";
                }
            }
        } //end of function

        void DeviceAppBtnSummary()
        {
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Artifacts";
            dgv_category.Columns[1].Name = "Information";
            for (int j = 0; j < data.Tables[0].Rows.Count; j++)
            {
               if(data.Tables[0].Rows[j].ItemArray[1].ToString().Contains("resourceLocal"))
               {
                    string[] values = data.Tables[0].Rows[j].ItemArray[1].ToString().Split(',');
                    for (int i=0;i<values.Length;i++)
                    {
                        if(values[i].Contains("resourceLocal"))
                        {
                            string[] row_text = values[i].Split(':');
                            if (row_text.Length > 2)
                                dgv_category.Rows.Add(row_text[0], row_text[1] + row_text[2]);
                            else if (row_text.Length > 1)
                                dgv_category.Rows.Add(row_text[0], row_text[1]);
                        }
                    }
               }
               else
               {
                    if(data.Tables[0].Rows[j].ItemArray[0].ToString().Contains("appkey") || data.Tables[0].Rows[j].ItemArray[0].ToString().Contains("appVersionName") || data.Tables[0].Rows[j].ItemArray[0].ToString().Contains("SYSTEM_INFO"))
                        dgv_category.Rows.Add(data.Tables[0].Rows[j].ItemArray[0].ToString(), data.Tables[0].Rows[j].ItemArray[1].ToString());
               }
            }
        } //end of function

        void getConverstationData(DataTable conversation_list)
        {
            string curr_id; 
            string[] name;
            for (int i = 0; i < conversation_list.Rows.Count; i++)
            {
                curr_id = conversation_list.Rows[i].ItemArray[0].ToString();
                name = getBuyerDetail(curr_id).Split(',');
                cmbBuyerList.Items.Add(name[0].Trim('\"'));
            }
        } //end of function

        string getBuyerDetail(string id)
        {
            string buyer_detail = "";
            try
            {
                string query;
                DataTable buyerdata = new DataTable();
                SQLiteConnection conn = new SQLiteConnection();
                conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                conn.Open();
                query = "Select Data From Account Where Account_id =" + "\"" + id + "\"";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(buyerdata);
                buyer_detail = getBuyerName(buyerdata.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return buyer_detail;
        } //end of function

        string getBuyerName(string data)
        {
            string name = "";
            string[] details = new string[3];
            int count = 0;
            string[] values = data.Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                if (values.Length > 3)
                {
                    if (i != 0)
                    {
                        string[] row_text = values[i].Split(':');
                        if (row_text.Length > 2)
                            details[count] = row_text[1] + row_text[2];
                        else if (row_text.Length > 1)
                            details[count] = row_text[1];
                        count++;
                    }
                }
                else if (values.Length <= 3)
                {
                    string[] row_text = values[i].Split(':');
                    if (row_text.Length > 2)
                        details[count] = row_text[1] + row_text[2];
                    else if (row_text.Length > 1)
                        details[count] = row_text[1];
                    count++;
                }
            }
            name = details[1] + "," + details[2];
            return name;
        } //end of function

        private void cmbBuyerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string query, sender_detail, sender_id; 
                string name = cmbBuyerList.SelectedItem.ToString();
                DataTable conv_detail = new DataTable();
                conv_detail.Clear();
                SQLiteConnection conn = new SQLiteConnection();
                conn.ConnectionString = "Data Source =" + DARAZ_PATH + "\\databases\\RippleDB_1_600017532652_pk";
                conn.Open();

                //get Buyer Id
                query = "Select Account_id From Account Where Data like " + "\"%" + name.Trim('\"') + "%\"";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);
                sender_id = conv_detail.Rows[0].ItemArray[0].ToString();
                conv_detail.Clear();

                //get Conversation
                query = "Select Summary,Create_Time From Message Where Sender_ID =" + "\"" + sender_id + "\"";
                cmd = new SQLiteCommand(query, conn);
                da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);

                //get buyer details
                sender_detail = getBuyerDetail(sender_id);
                string[] row_text = sender_detail.Split(',');

                //Adding data to GridView
                dgv_category.Rows.Clear();
                dgv_category.Refresh();
                dgv_category.ColumnCount = 4;
                dgv_category.Columns[0].Name = "Buyer Name";
                dgv_category.Columns[1].Name = "URL";
                dgv_category.Columns[2].Name = "Text";
                dgv_category.Columns[3].Name = "Created Time";
                for (int i = 0; i < conv_detail.Rows.Count; i++)
                {
                    dgv_category.Rows.Add(row_text[0], row_text[1], conv_detail.Rows[i].ItemArray[1].ToString(),
                                          UnixTimeStampToDateTime(Convert.ToDouble(conv_detail.Rows[i].ItemArray[2])).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } //end of function

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
