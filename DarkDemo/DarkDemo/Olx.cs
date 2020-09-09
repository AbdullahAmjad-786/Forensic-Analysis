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
        DataSet data = new DataSet();
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
                    btnSummary.Visible = true;
                    data.ReadXml(OLX_PATH + "\\shared_prefs\\panamera_preferences.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Posted Ad's")
                {
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select * From Ad";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Conversations")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select contactJid From Conversation";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    getConverstationData(dt);
                }
                if (button_selected == "Ad's Details")
                {
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    data.ReadXml(OLX_PATH + "\\shared_prefs\\Drafts.xml");
                    dgv_category.DataSource = data.Tables[0];
                }
                if (button_selected == "Buyer's info")
                {
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                    conn.Open();
                    query = "Select value,phonenumber From Profile";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
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
            if(lblName.Text == "Conversations")
            { 
                lblList.Visible = true;
                cmbBuyerList.Visible = true;
                dgv_category.Location = new Point(dgv_category.Location.X, 38);
                dgv_category.Height = 607;
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (button_selected == "User Login and Location")
            {
                LoginBtnSummanry();   
            }
            if (button_selected == "Posted Ad's")
            {
                PostedAdsBtnSummary();
            }
            if (button_selected == "Ad's Details")
            {
                DetailAdsBtnSummary();
            }
            if (button_selected == "Buyer's info")
            {
                BuyerInfoBtnSummary();
            }
        }

        void LoginBtnSummanry()
        {
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Artifacts";
            dgv_category.Columns[1].Name = "Information";
            for (int d = 0; d < data.Tables.Count; d++)
            {
                for (int j = 0; j < data.Tables[d].Rows.Count; j++)
                {
                    if (data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("userName") ||
                        data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("login_method") ||
                        data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("loginUserName") ||
                        data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("userIdLogged") ||
                        data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("GCM_APP_VERSION"))
                    {
                        dgv_category.Rows.Add(data.Tables[d].Rows[j].ItemArray[0].ToString(), data.Tables[d].Rows[j].ItemArray[1].ToString());
                    }
                    else if (data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("lastCurrentLocation") ||
                            data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("postingLocation") ||
                            data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("lastSearchLocation") ||
                            data.Tables[d].Rows[j].ItemArray[0].ToString().Contains("loggedUser"))
                    {
                        string[] values = data.Tables[d].Rows[j].ItemArray[1].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                        string txt = "";
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Contains("name") || values[i].Contains("latitude") || values[i].Contains("longitude"))
                            {
                                txt += values[i] + "\n ";
                            }
                            else if(values[i].Contains("contacts"))
                            {
                                string[] row_text = values[i].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                                dgv_category.Rows.Add(row_text[0], row_text[1]+ row_text[2]);
                            }
                        }
                        if(txt!="")
                            dgv_category.Rows.Add(data.Tables[d].Rows[j].ItemArray[0].ToString(), txt);
                    }
                }
            }
        } //end of function

        void PostedAdsBtnSummary()
        {
            string id = "", mileage = "", price = "", desc = "";
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 4;
            dgv_category.Columns[0].Name = "Ad Id";
            dgv_category.Columns[1].Name = "Mileage";
            dgv_category.Columns[2].Name = "Price";
            dgv_category.Columns[3].Name = "Description";
            for(int i=0; i<data.Tables[0].Rows.Count; i++)
            {
                string[] values = data.Tables[0].Rows[i].ItemArray[1].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                for(int j=0;j<values.Length;j++)
                {
                    if(values[j].Contains("sellerId"))
                    {
                        string[] row_text = values[j].Split(':');
                        id = row_text[1];
                    }
                    else if(values[j].Contains("mileage"))
                    {
                        string[] row_text = values[j].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                        if(values[j+1].Contains("categoryId"))
                        {
                            mileage = row_text[2];
                        }
                        else
                        {
                            mileage = (row_text[2] + values[j + 1]).Trim(new Char[] { '{', '/', '\'', '}', '[', ']' });
                        }
                    }
                    else if (values[j].Contains("rawPrice"))
                    {
                        string[] row_text = values[j].Split(':');
                        price = "Rs " + row_text[1];
                    }
                    else if (values[j].Contains("title"))
                    {
                        string[] row_text = values[j].Split(':');
                        desc = row_text[1];
                    }
                }
                dgv_category.Rows.Add(id, mileage, price, desc);
            }

        } //end of function

        void DetailAdsBtnSummary()
        {
            string id = "", value = "";
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Artifacts";
            dgv_category.Columns[1].Name = "Information";
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                string[] values = data.Tables[0].Rows[i].ItemArray[1].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    if (values[j].Contains("attribute_id"))
                    {
                        string[] row_text = values[j].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                        if (row_text.Length > 2)
                            id = row_text[2];
                        else if (row_text.Length > 1)
                            id = row_text[1];
                    }
                    else if(values[j].Contains("attribute_value"))
                    {
                        string[] row_text = values[j].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                        if (row_text.Length > 2)
                        {
                           if (id.Contains("user_location"))
                               value = row_text[1] + " : " + row_text[2];
                           else
                                value = row_text[2];
                        }
                        else if (row_text.Length > 1)
                            value = row_text[1];
                    }

                    if(id != "" && value != "")
                    {
                        dgv_category.Rows.Add(id, value);
                        id = "";
                        value = "";
                    }
                }
            }
        } //end of function

        void BuyerInfoBtnSummary()
        {
            string id = "", name = "", number = "";
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 3;
            dgv_category.Columns[0].Name = "Buyer's Id";
            dgv_category.Columns[1].Name = "Name";
            dgv_category.Columns[2].Name = "Caller No's";
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < data.Tables[0].Rows[i].ItemArray.Length; j++)
                {
                    string[] values = data.Tables[0].Rows[i].ItemArray[j].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                    if (j == 1)
                        number = data.Tables[0].Rows[i].ItemArray[j].ToString();
                    else
                    {
                        for(int k=0;k<values.Length;k++)
                        {
                            string[] row_text = values[k].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                            if (row_text[0] == "\"id\"")
                            {
                                id = row_text[1];
                            }
                            else if(row_text[0] == "\"name\"")
                            {
                                name = row_text[1];
                            }
                        }
                    }
                }
                dgv_category.Rows.Add(id,name,number);
            }
        } //end of function

        void getConverstationData(DataTable conversation_list)
        {
            string[] curr_id;
            for(int i=0;i<conversation_list.Rows.Count;i++)
            {
                curr_id = conversation_list.Rows[i].ItemArray[0].ToString().Split('@');
                cmbBuyerList.Items.Add(curr_id[0]);
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
                conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                conn.Open();
                query = "Select value,phonenumber From Profile Where id =" + "\"" + id + "\"";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(buyerdata);
                buyer_detail = getBuyerName(buyerdata.Rows[0].ItemArray[0].ToString());
                buyer_detail = buyer_detail + "," + buyerdata.Rows[0].ItemArray[1].ToString();
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
            string[] values = data.Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
            for (int k = 0; k < values.Length; k++)
            {
                string[] row_text = values[k].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                if (row_text[0] == "\"name\"")
                {
                    name = row_text[1];
                }
            }
            return name;
        } //end of function

        private void cmbBuyerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conv_id = "";
            try
            {
                string query,sender_detail;
                DataTable conv_detail = new DataTable();
                conv_detail.Clear();
                SQLiteConnection conn = new SQLiteConnection();
                conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\chat_new";
                conn.Open();
                
                //get Conversation Id
                query = "Select uuid From Conversation Where contactJid like " + "\"" + cmbBuyerList.SelectedItem.ToString() + "%\"";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);
                conv_id = conv_detail.Rows[0].ItemArray[0].ToString();
                
                //get Conversation
                query = "Select body,timeSent From Message Where conversationUuid =" + "\"" + conv_id + "\"";
                cmd = new SQLiteCommand(query, conn);
                da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);

                //get buyer details
                sender_detail = getBuyerDetail(cmbBuyerList.SelectedItem.ToString());
                string[] row_text = sender_detail.Split(',');
                
                //Adding data to GridView
                dgv_category.Rows.Clear();
                dgv_category.Refresh();
                dgv_category.ColumnCount = 4;
                dgv_category.Columns[0].Name = "Buyer Name";
                dgv_category.Columns[1].Name = "Phone Number";
                dgv_category.Columns[2].Name = "Text";
                dgv_category.Columns[3].Name = "Created Time";
                for (int i=1;i<conv_detail.Rows.Count;i++)
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
