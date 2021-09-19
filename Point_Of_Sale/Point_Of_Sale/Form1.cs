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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(dateTimePicker1.Text.ToString());
            //MessageBox.Show(con.State.ToString());
            //add a new supplier
            Clear();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //open connection to the database
                con.Open();

                //load suppliers
                disp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //display all suppliers
        public void disp()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            SqlCeDataAdapter adpt = new SqlCeDataAdapter("select * from TblSupplier", con);
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        //clear all inputs for new record
        public void Clear()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.ResetText();
            textBox2.Focus();
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("insert into TblSupplier(first_name,last_name,company,email,phone,date_joined) values('" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + dateTimePicker1.Text + "')", con);
                com.ExecuteNonQuery();
                MessageBox.Show("Data saved","SAVED", MessageBoxButtons.OK,MessageBoxIcon.Information);
                disp();
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
