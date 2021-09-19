using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace Point_Of_Sale
{
    public partial class Category : Form
    {
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");
        

        //suplier id
        private int supplier_ID;
        public Category()
        {
            InitializeComponent();
        }

        private void Category_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                loadSuppliers();

                disp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //get all suppliers
        private void loadSuppliers()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adpt = new SqlCeDataAdapter("select * from TblSupplier", con);
            adpt.Fill(dt);
            //check if data exists
            if (dt.Rows.Count > 0)
            {
                int i, j;
                i = 0;
                j = dt.Rows.Count;
                while(i < j )
                {
                    comboBox1.Items.Add(dt.Rows[i]["company"].ToString());

                    i++;
                }
            }
            else
            {
                MessageBox.Show("Please add supplier first");
            }
        }

        //get the id of the supplier company
        private void getSupplierId(object sender, EventArgs e)
        {
           //MessageBox.Show(comboBox1.Text.ToString());
            string companyName = comboBox1.Text.ToString();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adpt = new SqlCeDataAdapter("select supplier_id from TblSupplier where company='" + companyName + "' ", con);
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                { 
                    //id eexists
                    string s_id = dt.Rows[0]["supplier_id"].ToString();
                    supplier_ID = Convert.ToInt32(s_id);


                    //display the is
                    //MessageBox.Show(supplier_ID.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            textBox2.Clear();
            comboBox1.Text = "";
            textBox2.Focus();
        }

        //SHOW ALL CATEGORIES
        private void disp()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adpt = new SqlCeDataAdapter("select * from TblCategory ", con);
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand cmd = new SqlCeCommand("insert into TblCategory(category_name, supplier_id) values('"+ textBox2.Text.ToString() +"','"+ supplier_ID +"') ", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Category Saved", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp();
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void displayValues(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void displayData(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show("Test");
            if ((dataGridView1.RowCount - 1) > 0)
            {
               // MessageBox.Show(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
        }
    }
}
