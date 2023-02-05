using KalinWinApp.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.View.Order
{
    public partial class PrintPreview : Form
    {
        string customer;
        string date;
        string invoiceId;
        string total;
        string address;

        public PrintPreview(string customer, string date, string invoiceId,string total,string address)
        {
            InitializeComponent();
            this.customer = customer;
            this.date = date;
            this.invoiceId = invoiceId;
            this.total = total;
            this.address = address;
        }
        private void PrintPreview_Load(object sender, EventArgs e)
        {
            SelectAll selectAll = new SelectAll();
            dataGridView1.DataSource = selectAll.dataTable("select isnull(tempOrder.price*tempOrder.quantity,0) as سەرجەم, isnull(tempOrder.quantity,0) as دانە,isnull(tempOrder.price ,0 )  as نرخ , products.name as بابەت   from printSetting left join tempOrder on tempOrder.productId = printSetting.productsId inner join products on printSetting.productsId = products.id order by printSetting.place desc", null);
            txtTotal.Text = total;
            txtDate.Text = date;
            txtCustomer.Text = customer;
            txtAddress.Text = address;
            lblInvoice.Text = invoiceId;
        }

        #region print
        Bitmap bmp;
        private void btnPrint_Click(object sender, EventArgs e)
        {

            btnPrint.Hide();
            dataGridView1.SelectedCells[0].Selected = false;
            lblInvoice.Focus();
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            doc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            doc.Print();
            btnPrint.Show();

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            TableLayoutPanel grd = tableLayoutPanel1;
            Bitmap bmp = new Bitmap(grd.Width, grd.Height, grd.CreateGraphics());
            grd.DrawToBitmap(bmp, new Rectangle(0, 0, grd.Width, 1120));
            RectangleF bounds = e.PageSettings.PrintableArea;
            float factor = ((float)bmp.Height / (float)bmp.Width);
            e.Graphics.DrawImage(bmp, bounds.Left, bounds.Top, bounds.Width, factor * bounds.Width);
        }


        #endregion

    
    }
}
