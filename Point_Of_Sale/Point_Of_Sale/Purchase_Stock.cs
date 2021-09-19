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
    public partial class Purchase_Stock : Form
    {
        public Purchase_Stock()
        {
            InitializeComponent();
        }

        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");


        string id;
        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please select category","CATEGORY", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            dataGridView1.Rows.Clear();
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select Product_Name,Product_Id from TblProduct where category_id = '"+ id +"' ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < dt.Rows.Count)
                    {

                        dataGridView1.Rows.Add(dt.Rows[i]["Product_Id"].ToString(), dt.Rows[i]["Product_Name"].ToString());
                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("No products for the selected category","Add Product First",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Category Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void Purchase_Stock_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                getCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error connecting",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        //get the categories
        private void getCategory()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select category_name from TblCategory ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < dt.Rows.Count)
                    {

                        comboBox1.Items.Add(dt.Rows[i]["category_name"].ToString());
                        i++;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Category Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //get products by category name
        private void getProduct()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select category_name from TblCategory ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < dt.Rows.Count)
                    {

                        
                        i++;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Product Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getProductId(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select category_id from TblCategory where category_name = '"+ comboBox1.Text +"' ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["category_id"].ToString();
                   // MessageBox.Show(id);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Category Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            //save the purchasae order
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < dataGridView1.Rows.Count)
                    {

                        i++;
                    }
                }
                else
                {
                    MessageBox.Show("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"unable to update products",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

    }
}
