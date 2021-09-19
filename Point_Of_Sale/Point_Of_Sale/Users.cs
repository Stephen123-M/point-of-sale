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
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        string choice;
        public int level;

        private void Users_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("ADMINISTRATOR");
            comboBox1.Items.Add("STANDARD");

            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            try
            {
                con.Open();
                disp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error Establishing Connection",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void SetLevel(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBox1.Text.ToString());
            choice = comboBox1.Text.ToString();
            switch (choice)
            { 
                case "ADMINISTRATOR":
                    level = 1;
                    break;
                case "STANDARD":
                    level = 2;
                    break;
                default:
                    MessageBox.Show("Select Role","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Add new user","NEW",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
               // MessageBox.Show("Test");
                btnSave.Enabled = true;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Add new user", "NEW", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    SqlCeCommand com = new SqlCeCommand(" insert into TblUser(user_name,user_pass,email,access_level) values('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + level.ToString() + "') ", con);
                    com.ExecuteNonQuery();


                    //clear data grid
                    dataGridView1.Rows.Clear();
                    disp();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),"Error Adding User",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }

        //display the results
        private void disp()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select * from TblUSer ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //display available records
                    int j = 0;
                    string uname, psw, em, access, id;
                    while (j < dt.Rows.Count)
                    {
                        id = dt.Rows[j]["user_id"].ToString();
                        uname = dt.Rows[j]["user_name"].ToString();
                        psw = dt.Rows[j]["user_pass"].ToString();
                        em = dt.Rows[j]["email"].ToString();
                        access = dt.Rows[j]["access_level"].ToString();

                        dataGridView1.Rows.Add(id, uname, psw, em, access);


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
                MessageBox.Show(ex.Message.ToString(),"Error fetching user data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
