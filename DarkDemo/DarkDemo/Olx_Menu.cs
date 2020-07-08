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
    public partial class Olx_Menu : Form
    {
        public Olx_Menu()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            this.Hide();
            menu.Show();
        }

        private void btnOlxAttributes_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("Olx Attributes");
            this.Hide();
            olx.Show();
        }

        private void btnPostAds_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("Posted Ad's");
            this.Hide();
            olx.Show();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("Conversations");
            this.Hide();
            olx.Show();
        }

        private void btnBuyerinfo_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("Buyer's info");
            this.Hide();
            olx.Show();
        }

        private void btnSecurity_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("App Security");
            this.Hide();
            olx.Show();
        }

        private void btnAdsDetail_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("Ad's Details");
            this.Hide();
            olx.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Olx olx = new Olx("User Login and Location");
            this.Hide();
            olx.Show();
        }
    }
}