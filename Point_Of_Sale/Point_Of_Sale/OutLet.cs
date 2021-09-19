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
    public partial class OutLet : Form
    {
        public OutLet()
        {
            InitializeComponent();
        }

        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void OutLet_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                getStoreId();
                GetStores();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error establishing connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //get id
        private void getStoreId()
        {
            try
            {

                SqlCeDataReader dr;
                SqlCeCommand com = new SqlCeCommand("select max(store_id) from TblStore", con);
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    string id = dr[0].ToString();
                    if (id == "")
                    {
                        textBox1.Text = "1";
                    }
                    else
                    {
                        int store = 1 + Convert.ToInt32(id);
                        textBox1.Text =store.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Unable to generate store id",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetStores()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblStore", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string id, name, loc;
                    int j = 0;
                    while (j < dt.Rows.Count)
                    {
                        id = dt.Rows[j]["store_id"].ToString();
                        name = dt.Rows[j]["store_name"].ToString();
                        loc = dt.Rows[j]["store_location"].ToString();
                        dataGridView1.Rows.Add(id, name, loc);
                        j++;
                    }
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Unable to get store data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand(" insert into TblStore(store_id,store_name,store_location) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "') ", con);
                com.ExecuteNonQuery();

                dataGridView1.Rows.Clear();

                GetStores();
                getStoreId();
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Store not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Add new store","NEW", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                textBox2.Clear();
                textBox3.Clear();
                btnSave.Enabled = true;
                textBox2.Focus();
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }
    }
}
