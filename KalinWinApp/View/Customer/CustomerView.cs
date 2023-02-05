using KalinWinApp.Database;
using KalinWinApp.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.View.Customer
{
    public partial class CustomerView : Form
    {

        #region Initialize
        DataTable dataTable = new DataTable();
        Excute ex = new Excute();
        string selectedId = null;
        string selectedName = null;
        string fromWhere = null; 
        #endregion

        #region load
        public CustomerView()
        {
            InitializeComponent();
        }
        public CustomerView(string fromWhere)
        {
            InitializeComponent();
            this.fromWhere = fromWhere;
        }
        private void CustomerView_Load(object sender, EventArgs e)
        {
            setData();
            if (fromWhere=="fromOrder")
            {
                btnChoose.Visible = true;
            }
        }

        void getData()
        {
            dataGridView1.DataSource = dataTable;
            #region hide columns
            this.dataGridView1.Columns["id"].Visible = false;
            this.dataGridView1.Columns["deleted_at"].Visible = false;
            #endregion
        }
        public void setData()
        {
            #region set datagrid
            SelectAll selectAll = new SelectAll();
            dataTable = new DataTable();
            dataTable = selectAll.dataTable("select id, name as 'ناوی کڕیار', phone as 'مۆبایل',address as 'ناونیشان', deleted_at from customers where deleted_at is NULL order by id desc", null);
            dataGridView1.DataSource = dataTable;
            #endregion
            #region hide columns
            this.dataGridView1.Columns["id"].Visible = false;
            this.dataGridView1.Columns["deleted_at"].Visible = false;
            #endregion
        }
        #endregion

        #region exit minimize
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState= FormWindowState.Minimized;
        }

        #endregion

        #region clear
        void clear()
        {
            txtName.Text = "ناوی کڕیار";
            txtAddress.Text = "ناونیشان";
            txtPhone.Text = "مۆبایل";
            selectedId = null;
        }
        #endregion

        #region CRUD
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //ExHandler txtHandle = new ExHandler();
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtPhone }, new string[] { "ناوی کڕیار", "مۆبایل", "ناونیشان" });
                Excute excute = new Excute();
                excute.Command("insert into customers values((select ISNULL(max(id),0)+1 from customers),@name,@phone,@address,null);",
                    new string[,] {
                    {"@name" ,txtName.Text},
                    {"@phone" ,txtPhone.Text},
                    {"@address" ,txtAddress.Text},
                        }
                    );
                clear();
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
                #region Ex
                ExHandler.IsIdSelected(selectedId);
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtPhone }, new string[] { "ناوی کڕیار", "مۆبایل", "ناونیشان" });
                #endregion
                ex.Command("update customers set deleted_at = GETDATE() where id=@id", new string[,] { { "id", selectedId } });
                setData();
                clear();
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
                #region Ex
                ExHandler.IsIdSelected(selectedId);
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtPhone }, new string[] { "ناوی کڕیار", "مۆبایل", "ناونیشان" });
                # endregion
                ex.Command("update customers set name=@name, address = @address, phone = @phone where id=@id", new string[,] {
                    { "name", txtName.Text },  { "id", selectedId }, { "phone", txtPhone.Text }, { "address", txtAddress.Text }
                });
                setData();
                clear();
            }
            catch (Exception ex)
            {
                Connection.conn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    var filtered = dataTable.AsEnumerable()
                    .Where(r => r.Field<String>("ناوی کڕیار").Contains(txtSearch.Text) || r.Field<String>("مۆبایل").Contains(txtSearch.Text) || r.Field<String>("مۆبایل").Contains(txtSearch.Text));
                    dataGridView1.DataSource = filtered.AsDataView();
                }
                else
                {
                    getData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("هەڵەیەک ڕویدا تکایە دوبارە هەوڵبەرەوە");
            }
        }
        #endregion

        #region Choose
        private void btnChoose_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] { selectedId, selectedName }, "هیچ کڕیارێک هەڵنەبژاردووە");
                OrderView.customerId = selectedId;
                OrderView.customerName = selectedName;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Datagrid
      

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = selectedName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تکایە دووبارە هەوڵبەرەوە");
            }
        }
        #endregion

        #region Enter Leave
        private void txtName_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی کڕیار");
            pc.RemovePlace();
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی کڕیار");
            pc.MakePlace();
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPhone, "مۆبایل");
            pc.RemovePlace();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPhone, "مۆبایل");
            pc.MakePlace();
        }

        private void txtAddress_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtAddress, "ناونیشان");
            pc.RemovePlace();
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtAddress, "ناونیشان");
            pc.MakePlace();
        } 
        #endregion
    }
}
