using KalinWinApp.Database;
using KalinWinApp.View;
using KalinWinApp.View.Customer;
using KalinWinApp.View.products;
using KalinWinApp.View.wasta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

  


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ProductsView pv = new ProductsView();
            pv.ShowDialog();
      


        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintSetting ps =  new PrintSetting();
            ps.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WastaView wv = new WastaView();
            wv.ShowDialog();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            CustomerView cv = new CustomerView();
            cv.ShowDialog();
        }

        private void btnOrder_Click_1(object sender, EventArgs e)
        {
            OrderView ov = new OrderView();
            ov.ShowDialog();
        }
    }
}
