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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Daraz_Menu daraz = new Daraz_Menu();
            this.Hide();
            daraz.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Olx_Menu olx = new Olx_Menu();
            this.Hide();
            olx.Show();
        }
    }
}
