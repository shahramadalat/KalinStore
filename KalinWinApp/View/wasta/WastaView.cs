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

namespace KalinWinApp.View.wasta
{
    public partial class WastaView : Form
    {

        #region initialize
        DataTable dataTable = new DataTable();
        string selectedId = null;
        string selectedName=null;
        string fromWhere=null;

        #endregion
        #region load
        public WastaView()
        {
            InitializeComponent();
        }
        public WastaView(string fromWhere)
        {
            InitializeComponent();
            this.fromWhere = fromWhere;
        }
        private void WastaView_Load(object sender, EventArgs e)
        {
            setData();
            if (fromWhere=="fromOrder")
            {
                btnChoose.Visible= true;
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
            dataTable = selectAll.dataTable("select id, name as 'ناو', job as 'پیشە',phone as 'مۆبایل',address as 'ناونیشان', deleted_at from wasta where deleted_at is NULL order by id desc", null);
            dataGridView1.DataSource = dataTable;
            #endregion
            #region hide columns
            this.dataGridView1.Columns["id"].Visible = false;
            this.dataGridView1.Columns["deleted_at"].Visible = false;
            #endregion
        }

        #endregion
        #region hide show buttons
        void show()
        {
            btnDelete.Visible = true;
            btnUpdate.Visible = true;
        }

        void hide()
        {
            btnUpdate.Hide();
            btnDelete.Hide();
        }
        #endregion
        #region minimize maximize
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #endregion
        #region leave enter
        private void txtName_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی وەستا");
            pc.MakePlace();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtName, "ناوی وەستا");
            pc.RemovePlace();
        }

        private void txtJob_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtJob, "پیشە");
            pc.MakePlace();
        }

        private void txtJob_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtJob, "پیشە");
            pc.RemovePlace();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPhone, "مۆبایل");
            pc.MakePlace();
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtPhone, "مۆبایل");
            pc.RemovePlace();
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtAddress, "ناونیشان");
            pc.MakePlace();
        }

        private void txtAddress_Enter(object sender, EventArgs e)
        {
            Placeholder pc = new Placeholder(txtAddress, "ناونیشان");
            pc.RemovePlace();
        }


        #endregion
        #region clear
        void clear()
        {
            txtName.Text = "ناوی وەستا";
            txtAddress.Text = "ناونیشان";
            txtJob.Text = "پیشە";
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
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtJob, txtPhone }, new string[] { "ناوی وەستا", "پیشە", "مۆبایل", "ناونیشان" });
                Excute excute = new Excute();
                excute.Command("insert into wasta values((select ISNULL(max(id),0)+1 from wasta),@name, @job,@phone,@address,null);",
                    new string[,] {
                    {"@name" ,txtName.Text},
                    {"@job" ,txtJob.Text},
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandler.IsIdSelected(selectedId);
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtJob, txtPhone }, new string[] { "ناوی وەستا", "پیشە", "مۆبایل", "ناونیشان" });
                Excute ex = new Excute();
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ex.Command("update wasta set name=@name, job=@job, address = @address, phone = @phone where id=@id", new string[,] { { "name", txtName.Text }, { "job", txtJob.Text }, { "id", selectedId }, { "phone", txtPhone.Text }, { "address", txtAddress.Text } });
                }
                setData();
                clear();
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
                ExHandler.IsIdSelected(selectedId);
                ExHandler.CheckIfNullOrEmpty(new Control[] { txtName, txtAddress, txtJob, txtPhone }, new string[] { "ناوی وەستا", "پیشە", "مۆبایل", "ناونیشان" });
                Excute ex = new Excute();
                ex.Command("update wasta set deleted_at = GETDATE() where id=@id", new string[,] { { "id", selectedId } });
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
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var filtered = dataTable.AsEnumerable()
                .Where(r => r.Field<String>("ناو").Contains(txtSearch.Text) || r.Field<String>("پیشە").Contains(txtSearch.Text) || r.Field<String>("مۆبایل").Contains(txtSearch.Text));
                dataGridView1.DataSource = filtered.AsDataView();
            }
            else
            {
                getData();
            }
        } 
        #endregion

        #region datagrid
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = selectedName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtJob.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        #endregion

        private void btnChoose_Click(object sender, EventArgs e)
        {
            try
            {
                ExHandlerMessage.MultipleCheckIfNullOrEmpty(new string[] {selectedId,selectedName },"هیچ وەستایەکت هەڵنەبژاردووە");
                OrderView.wastaId = selectedId;
                OrderView.wastaName = selectedName;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
