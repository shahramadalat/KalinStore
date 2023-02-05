using KalinWinApp.Database;
using KalinWinApp.Helper;
using KalinWinApp.View.products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KalinWinApp.View
{
    public partial class PrintSetting : Form
    {
        DataTable dataTable;
        public static string id;
        string selectedId = null;
        string fromWhere;
        string ProductName;
        string ProductPrice;
        Excute ex = new Excute();

        #region Load
        public PrintSetting()
        {
            InitializeComponent();
        }
        public PrintSetting(string fromWhere)
        {
            InitializeComponent();
            this.fromWhere = fromWhere;
            if (fromWhere=="fromOrder")
            {
                btnChoose.Visible = true;
            }
        }
        private void PrintSetting_Load(object sender, EventArgs e)
        {
            setData();
            hide();
        }
        void getData()
        {
            dataGridView1.DataSource = dataTable;
            this.dataGridView1.Columns["id"].Visible = false;
        }
        void setData()
        {
            #region set datagrid
            SelectAll selectAll = new SelectAll();
            dataTable = new DataTable();
            dataTable = selectAll.dataTable("select productsId, name as 'ناو', price as 'نرخ', place as 'ڕیزبەندی' from vw_printSetting order by place", null);
            dataGridView1.DataSource = dataTable;
            #endregion
            #region hide columns
            this.dataGridView1.Columns["productsId"].Visible = false;
            #endregion
        }
        #endregion

        #region show hide selections

        void show()
        { 
            lblName.Visible = true;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            txtPlace.Visible = true;
        }
        void hide()
        {
            lblName.Hide();
            btnUpdate.Hide();
            btnDelete.Hide();
            txtPlace.Hide();
        }
        #endregion

        #region CRUD
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsView pv = new ProductsView(true);
                pv.ShowDialog();
                if (string.IsNullOrEmpty(id) )
                {
                    throw new Exception("هیچ کاڵایەک هەڵنەبژێردرا");
                }
                ex.Command("insert into printSetting values(@id, (select ISNULL(max(place),0)+1 from printSetting) )", new string[,] { { "id", id } });
                
                id= null;

                setData();
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
                if (string.IsNullOrEmpty(selectedId))
                {
                    throw new Exception("هەڵەیەک ڕویدا، تکایە دوبارە هەوڵ بدەوە");
                }
                if (string.IsNullOrEmpty(txtPlace.Text))
                {
                    txtPlace.Focus();
                    throw new Exception("تکایە ڕیزبەندی دیاری بکە");
                }
                ex.Command("delete from printSetting where productsId=@id", new string[,] { { "id", selectedId } });
                hide();
                setData();
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
                if (string.IsNullOrEmpty(selectedId))
                {
                    throw new Exception("تکایە ئەو کاڵایە هەڵبژێرە کە دەتەوێ دەستکاری بکەی");
                }
                if (string.IsNullOrEmpty(txtPlace.Text))
                {
                    txtPlace.Focus();
                    throw new Exception("تکایە ڕیزبەندی دیاری بکە");
                }

                SelectOne selectOne = new SelectOne();
                // 3 -> 4
                // 2 -> 1

                // 3 = 2 then 2 = 3

                // id(2)
                string hasAnySamePlace =  selectOne.Select("SELECT productsId FROM printSetting WHERE place = @place ", new string[,] { { "place",txtPlace.Text.Trim()} });
                if (string.IsNullOrEmpty(hasAnySamePlace))
                {
                    ex.Command("update printSetting set place = @place where productsId=@id", new string[,] { { "id", selectedId }, { "place", txtPlace.Text.Trim() } });
                    setData();
                    hide();
                }
                else
                {
                    //string exsitedPlaceId = selectOne.Select("select productsId from printSetting where place=@place",new string[,] { { "place",hasAnySamePlace} });

                    // set exsiting one's place to the new one
                    // then change the new one to the existing one
                    ex.Command("update printSetting set place = (select place from printSetting where productsId=@newId) where productsId=@exsisted", new string[,] { { "newId", selectedId }, { "exsisted", hasAnySamePlace } });
                    ex.Command("update printSetting set place = @place where productsId=@newId", new string[,] { { "newId", selectedId }, { "place", txtPlace.Text.Trim() } });
                    setData();
                    hide();
                }
                


            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region txtPlace Validation
        private void txtPlace_KeyUp(object sender, KeyEventArgs e)
        {
            DecimalValidator validator = new DecimalValidator();
            validator.onlyNumber(txtPlace);
        } 
        #endregion

        #region minimize and maximize
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region placeholder
        private void txtPlace_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPlace, "ڕیزبەندی");
            pc.MakePlace();
        }

        private void txtPlace_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPlace, "ڕیزبەندی");
            pc.RemovePlace();
        }

        #endregion

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            selectedId = id;
            lblName.Text = ProductName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            ProductPrice = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPlace.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            show();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
                ExHandlerMessage.SingleCheckIfNullOrEmpty(selectedId,"تکایە کاڵایەک هەڵبژێرە بۆ فرۆشتن");
                OrderView.ProductId = selectedId;
                OrderView.ProductName = ProductName;
                OrderView.ProductPrice = ProductPrice;
                this.Close();
                return;
        }
    }
}
