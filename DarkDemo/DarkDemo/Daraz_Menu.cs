using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarkDemo
{
    public partial class Daraz_Menu : Form
    {
        public Daraz_Menu()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            this.Hide();
            menu.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("User Login and Location");
            this.Hide();
            daraz.Show();
        }

        private void btnTrader_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("Trader Accounts");
            this.Hide();
            daraz.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("Search Details");
            this.Hide();
            daraz.Show();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("Conversations");
            this.Hide();
            daraz.Show();
        }

        private void btnDarazOrder_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("Daraz Orders");
            this.Hide();
            daraz.Show();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            Daraz daraz = new Daraz("Device and App Info");
            this.Hide();
            daraz.Show();
        }
    }
}
