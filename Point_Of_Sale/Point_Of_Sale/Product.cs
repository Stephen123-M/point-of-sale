using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace Point_Of_Sale
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        FileStream fs;
        BinaryReader br;
        private byte[] byt;
        private int categoryId;

        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpeg | *.jpg";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //MessageBox.Show("Test");
                fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                byt = br.ReadBytes((int)fs.Length);
                pictureBox1.Image = Image.FromStream(fs);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                disp();
                getCategory();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //display all the products
        private void disp()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblProduct", con);
            adt.Fill(dt);
            dataGridView1.DataSource = dt;
            
        }

        //get category
        private void getCategory()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adt = new SqlCeDataAdapter("select category_name from TblCategory", con);
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
        }

        private void getCategoryId(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adt = new SqlCeDataAdapter("select category_id from TblCategory where category_name = '"+ comboBox1.Text.ToString()+ "' ", con);
            adt.Fill(dt);
            string cat_id = dt.Rows[0]["category_id"].ToString();
            categoryId = Convert.ToInt32(cat_id);
           // MessageBox.Show(categoryId.ToString());


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearInputs();
            btnSave.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand(" insert into TblProduct(Product_Name,Price,category_id,Image) values('" + textBox1.Text + "','" + textBox2.Text + "','" + categoryId + "',@img)  ", con);
                com.Parameters.Add(new SqlCeParameter("@img",byt));
                com.ExecuteNonQuery();
                MessageBox.Show("Product Saved","SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Information);
                disp();
                btnSave.Enabled = false;
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //CLEAR THE INPUTS
        private void ClearInputs()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.Text = "";
            //pictureBox1.Image = Image.FromStream("");
            textBox1.Focus();
        }
    }
}
