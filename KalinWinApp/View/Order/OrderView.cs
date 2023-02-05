using KalinWinApp.Database;
using KalinWinApp.Helper;
using KalinWinApp.View.Customer;
using KalinWinApp.View.Order;
using KalinWinApp.View.products;
using KalinWinApp.View.wasta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.View
{
    public partial class OrderView : Form
    {
        #region exit minimize
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion


        #region initialize
        DataTable dataTable =  new DataTable();
        string selectedId = null;
        Excute ex = new Excute();
        string invoiceId;

        public static string ProductId;
        public static string ProductName;
        public static string ProductPrice;

        #endregion

        #region load
        public OrderView()
        {
            InitializeComponent();
        }

        private void OrderView_Load(object sender, EventArgs e)
        {
            getInvoiceId();
            getTemp();
        }
        #endregion

        #region helper methods

        void upperReset()
        {
            selectedId = null;
            txtName.Text= ProductName = null;
            txtPrice.Value  = 0;
            ProductPrice = null;
            ProductId= null;

        }
        void getTemp()
        {
            SelectAll selectAll = new SelectAll();
            dataTable  = selectAll.dataTable("select * from vw_tempOrder", null); ;
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns[0].Visible= false;
            dataGridView1.Columns[1].Visible= false;
            dataGridView1.Columns[2].Visible= false;

            #region get total
                decimal Total = dataTable.AsEnumerable().Sum(
                row => decimal.Parse(row["سەرجەم"].ToString()));
                txtTotal.Value = Total;
            #endregion

            #region get final
            txtFinal.Value = txtTotal.Value - txtDiscount.Value; 
            txtRemaining.Value = txtFinal.Value - txtPaid.Value;
            #endregion
        }

        void getInvoiceId()
        { 
            SelectOne selectOne= new SelectOne();
            
            invoiceId = selectOne.Select("select isnull(max(id),0)+1 from invoices", null);
            lblLastInvoiceId.Text = invoiceId +" "+ ":ژمارەی پسوڵە";
        
        }
        #endregion

        #region CRUD
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] { ProductId, ProductName }, "هیچ کاڵایەکت هەڵنەبژاردوە، تکایە سەرەتا کاڵایەک هەڵبژێرە بۆ فرۆشتن");
                #region search if Product exsist in tempOrder then increament it if the has same price
                SelectOne select = new SelectOne();
                var isProductExist = select.Select("select productId from tempOrder where productId = @id", new string[,] { { "id", ProductId } });
                if (!string.IsNullOrEmpty(isProductExist))
                {
                    throw new Exception("ئەم کاڵایە پێشتر داخڵکراوە");
                } 
                #endregion
                #region insert to tempOrder
                ex.Command("insert into tempOrder values( (select isnull(max(id),0)+1 from tempOrder),@invoiceId,@id, @price, @quantity )",
                          new string[,] {
                    { "@invoiceId",invoiceId},
                    { "@id",ProductId},
                    { "@price",txtPrice.Value.ToString()},
                    { "@quantity",txtQuantity.Value.ToString()}
                          }); 
                #endregion
                upperReset();
                getTemp();
            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandlerMessage.SingleCheckIfNullOrEmpty(selectedId,"هیچ کاڵایەکت هەڵنەبژاردوە بۆ سڕینەوە");
                ex.Command("delete from tempOrder where id = @id", new string[,] { { "id",selectedId} });
                getTemp();
                upperReset();
            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                #region ExHandler

                ExHandlerMessage.SingleCheckIfNullOrEmpty(selectedId,"هیچ کاڵایەکت هەڵنەبژاردوە بۆ دەستکاریکردن");
                if (txtQuantity.Value <= 0)
                {
                    throw new Exception("تکایە دانەی کاڵا دیاری بکە");
                }
                if (txtPrice.Value <= 0)
                {
                    throw new Exception("تکایە نرخی کاڵا دیاریبکە");
                }
                #endregion
                #region update
                ex.Command("update tempOrder set price=@price, quantity=@quantity where id=@id",
                        new string[,]
                        {
                    { "price",txtPrice.Value.ToString()},
                    { "quantity",txtQuantity.Value.ToString()},
                    { "id",selectedId},
                        });
                #endregion
                getTemp();
                upperReset();
            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        } 
        #endregion

        #region Choose
        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                PrintSetting printSetting = new PrintSetting("fromOrder");
                printSetting.ShowDialog();
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] { ProductName, ProductId }, "هیچ کاڵایەک هەڵنەبژێردرا");
                txtName.Text = ProductName;
                txtPrice.Value = decimal.Parse(ProductPrice.Trim());
                txtQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region wasta
        public static string wastaId = null;
        public static string wastaName = null;
        private void btnWasta_Click(object sender, EventArgs e)
        {
            try
            {
                WastaView wv = new WastaView("fromOrder");
                wv.ShowDialog();
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] { wastaId, wastaName }, "هیچ وەستایەک هەڵنەبژێردرا");
                lblwasta.Text = wastaName;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        void wastaReset()
        {
            wastaId = null;
            wastaName = null;
            lblwasta.Text = string.Empty;
            txtGift.Value = 0;

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWasta.Checked)
            {
                btnWasta.Enabled = true;
                txtGift.Enabled = true;
                lblwasta.Enabled = true;
            }
            else
            {
                btnWasta.Enabled  = false;
                txtGift.Enabled = false;
                wastaReset();
            }
        }
        #endregion

        #region custommer
        public static string customerId = null;
        public static string customerName = null;
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerView cv = new CustomerView("fromOrder");
                cv.ShowDialog();
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] { customerId, customerName }, "هیچ کڕیارێکت هەڵنەبژێردرا");
                lblCustomer.Text = customerName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
            {
                btnCustomer.Enabled = true;
                lblCustomer.Enabled = true;
                txtForAddress.Enabled = true;
            }
            else
            {
                btnCustomer.Enabled = false;
                lblCustomer.Enabled = false;
                txtForAddress.Enabled = false;
                txtForAddress.Text = string.Empty; 
                customerId = null;
                customerName = null;
            }
        }

        #endregion

        #region calculations
        private void txtDiscount_ValueChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Value > txtTotal.Value)
            {
                MessageBox.Show("نابێ داشکاندن زیاتر بێ لە کۆی گشتی");
                txtDiscount.Value = 0;
                txtDiscount.Focus();
                return;
            }
            txtFinal.Value = txtTotal.Value - txtDiscount.Value;
            txtRemaining.Value = txtFinal.Value - txtPaid.Value;
        }
 
        private void txtPaid_ValueChanged(object sender, EventArgs e)
        {

            if (txtFinal.Value<txtPaid.Value)
            {
                MessageBox.Show("نابێ پارەی دراو گەورەتر بێ لە نرخی کۆتایی");
                txtPaid.Focus();
                txtPaid.Value = 0;
                return;

            }
                txtRemaining.Value = txtFinal.Value - txtPaid.Value;
        }
        #endregion

        #region datagrid clcik
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                ProductId = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtName.Text = ProductName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                ProductPrice = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtPrice.Value = decimal.Parse(ProductPrice);
                txtQuantity.Value = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region print
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandlerMessage.DatagridRowCount(dataGridView1);
                if (chkCustomer.Checked)
                {
                    ExHandlerMessage.SingleCheckIfNullOrEmpty(lblCustomer.Text, "تکایە ناوی کڕیار تۆمار بکە");
                    ExHandlerMessage.SingleCheckIfNullOrEmpty(txtForAddress.Text, "تکایە ناونیشان تۆمار بکە");
                }
                ExHandlerMessage.SingleCheckIfNullOrEmpty(dateTimePicker1.Text,"تکایە بەرواری فرۆشتن تۆمار بکە تۆمار بکە");

                PrintPreview pv = new PrintPreview(lblCustomer.Text,dateTimePicker1.Text,lblLastInvoiceId.Text,txtTotal.Value.ToString(),txtForAddress.Text);
                pv.ShowDialog();
            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                #region validation
                ExHandlerMessage.DatagridRowCount(dataGridView1);
                ExHandlerMessage.SingleCheckIfNullOrEmpty(wastaId, "تکایە وەستا هەڵبژێرە");
                ExHandlerMessage.SingleCheckIfNullOrEmpty(customerId, "تکایە کڕیار هەڵبژێرە");
                ExHandlerMessage.SingleCheckIfNullOrEmpty(txtForAddress.Text, "تکایە ناونیشان دیاری بکە");
                #endregion
                ex.Procedure("sp_finalOrder", new Dictionary<string, object> { 
                    { "date",dateTimePicker1.Text},
                    { "wastaId",wastaId},
                    { "customerId",customerId},
                    { "total",txtTotal.Value.ToString()},
                    { "billed",txtPaid.Value.ToString()},
                    { "discount", txtDiscount.Text} ,
                    { "remaining",txtRemaining.Value.ToString() },
                    { "forAddress",txtForAddress.Text},
                    { "gift",txtGift.Value.ToString()}
                });

                #region reset
            

                wastaId = string.Empty;
                wastaName = string.Empty;
                lblwasta.Text = string.Empty;
                txtGift.Value = 0;

                customerId = string.Empty;
                customerName = string.Empty;
                txtForAddress.Text = string.Empty;
                lblCustomer.Text = string.Empty;

                txtPaid.Value = 0;
                txtDiscount.Value = 0;
                txtRemaining.Value = 0;
                txtFinal.Value = 0;
                txtTotal.Value = 0;

                dateTimePicker1.Value = DateTime.Now;
                upperReset();
                getTemp();
                getInvoiceId();
                #endregion
                MessageBox.Show("سەرکەوتوو بوو");
        }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }









}

    
    }
}
