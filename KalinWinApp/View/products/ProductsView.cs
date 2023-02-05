using KalinWinApp.Database;
using KalinWinApp.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.View.products
{
    public partial class ProductsView : Form
    {
        #region initialize Objects
        DataTable dataTable;
        bool isVisible = false;
        string selectedId = null;
        bool fromSetting;
        string fromWhere = null;
        string ProductName;
        string ProductPrice;
        Excute ex = new Excute();
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

        #region hide show CRUD commands

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isVisible)
            {
                tblUpper.Hide();
                isVisible = false;
            }
            else
            {
                tblUpper.Visible = true;
                isVisible = true;
            }
        }
        #endregion

        #region Load needs
        public ProductsView()
        {
            InitializeComponent();
        }
        public ProductsView(string fromWhere)
        {
            this.fromWhere = fromWhere;
            
        }
        public ProductsView(bool isForPrint)
        {
            InitializeComponent();
            if (isForPrint)
            {
               fromSetting = true;
            }
        }

        public void getData()
        {
            #region set datagrid
            SelectAll selectAll = new SelectAll();
            dataTable = new DataTable();
            dataTable = selectAll.dataTable("select id, name as 'ناو', price as 'نرخ', deleted_at from products where deleted_at is NULL order by id desc", null);
            dataGridView1.DataSource = dataTable;
            #endregion
            #region hide columns
            this.dataGridView1.Columns["id"].Visible = false;
            this.dataGridView1.Columns["deleted_at"].Visible = false;
            #endregion
        }

        private void ProductsView_Load(object sender, EventArgs e)
        {
            getData();
            if (fromWhere=="fromOrder")
            {
                btnChoose.Visible = true;
            }
            if (fromSetting)
            {
                btnChoose.Visible = true;
            }
        } 
        #endregion

        #region placeholder
        private void textBox1_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی کاڵا");
            pc.RemovePlace();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی کاڵا");
            pc.MakePlace();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPrice, "نرخ");
            pc.RemovePlace();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPrice, "نرخ");
            pc.MakePlace();
        }
        #endregion

        #region search
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var filtered = dataTable.AsEnumerable()
                .Where(r => r.Field<String>("ناو").Contains(txtSearch.Text));
                dataGridView1.DataSource = filtered.AsDataView();
            }
            else
            {
                dataGridView1.DataSource = dataTable;
                #region hide columns
                this.dataGridView1.Columns["id"].Visible = false;
                this.dataGridView1.Columns["deleted_at"].Visible = false;
                #endregion
            }
        }
        #endregion

        #region datagrid
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            #region visibility of buttons
            tblUpper.Visible = true;
            isVisible = true;
            btnDelete.Visible = true;
            btnUpdate.Visible = true;
            #endregion

            pnlUpper.Visible = true;
            var id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            selectedId = id;
            ProductName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            ProductPrice = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtName.Text = ProductName;
            txtPrice.Text = ProductPrice;

        } 
        #endregion

        #region delete update insert
        void reset()
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            txtName.Text = "ناوی کاڵا";
            txtPrice.Text = "نرخ";
            selectedId = "";
        }
        private void btnAddProduct_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(txtName.Text) || txtName.Text == "ناوی کاڵا")
                {
                    throw new Exception("تکایە ناوی کاڵا دیاری بکە");
                }
                if (string.IsNullOrEmpty(txtPrice.Text) || txtPrice.Text == "نرخ")
                {
                    throw new Exception("تکایە نرخی کاڵا دیاری بکە");
                }
                ex.Command("insert into products values( (select isnull(max(id),0)+1 from products), @name,@price ,null);", new string[,] {
                {"name",txtName.Text },
                {"price",txtPrice.Text}
            });
                reset();
                getData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId))
            {
                ex.Command("update products set deleted_at = GETDATE() where id=@id", new string[,] { { "id", selectedId } });
                reset();
                getData();
            }
            else
            {
                MessageBox.Show("هەڵەیەک ڕویدا لە کارەکەدا تکایە دووبارە هەوڵبەرەوە");
                reset();
                getData();
            }
            
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId))
            {
                ex.Command("update products set name=@name, price=@price where id=@id", new string[,] { { "name", txtName.Text }, { "price", txtPrice.Text }, { "id", selectedId } });
            }
            reset();
            getData();
        }
        #endregion

        #region price validation
        private void txtPrice_KeyUp(object sender, KeyEventArgs e)
        {
            DecimalValidator validator = new DecimalValidator();
            validator.validate(txtPrice);
        }
        #endregion

        #region Choose
        private void btnChoose_Click(object sender, EventArgs e)
        {
            try
            {
               


                if (string.IsNullOrEmpty(selectedId) || string.IsNullOrEmpty(txtName.Text))
                {
                    throw new Exception("تکایە کاڵایەک هەڵبژێرە بۆ ئەوەی زیادبکرێ بۆ بەشی ڕێکخستن");
                }
                SelectOne selectOne = new SelectOne();
                string hasAnyExisting = selectOne.Select("select 1 from printSetting where productsId=@id",new string[,] { { "id",selectedId} });
                if (!string.IsNullOrEmpty(hasAnyExisting))
                {
                    throw new Exception("ئەم کاڵایە بونی هەیە لە بەشی ڕێکخستن");
                }
                PrintSetting.id = selectedId;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        #endregion
    }
}
