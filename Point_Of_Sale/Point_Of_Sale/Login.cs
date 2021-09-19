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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        public string username = "admin";
        public string password = "password";
        public int tries = 3;
        int access_level;


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string uname = textBox1.Text.ToString();
                string psw = textBox2.Text.ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select * from TblUser where user_name ='"+ uname +"' and user_pass = '"+ psw+"' ", con);
                adt.Fill(dt);
                //check if user is valid
                if (dt.Rows.Count > 0)
                { 
                    //valid user
                    access_level = Convert.ToInt32(dt.Rows[0]["access_level"].ToString());
                    //check if admin or starndard user

                    //save time open and date
                    string date = DateTime.Now.Date.ToShortDateString();
                    string time = DateTime.Now.ToString("HH:mm:ss tt");
                    string user = dt.Rows[0]["user_name"].ToString();
                    //SqlCeCommand com = new SqlCeCommand("insert into TblTime(user_name,open_time,date) values('" + user + "','" + time + "','" + date + "')", con);
                    //com.ExecuteNonQuery();

                    if (access_level == 1)
                    { 
                        //admin
                        Admin admin = new Admin();
                        admin.ShowDialog();
                        this.Close();
                        this.Dispose();
                    }
                    else if(access_level == 2)
                    {
                        TILL till = new TILL();
                        till.ShowDialog();
                        this.Close();
                        this.Dispose();
                    }

                }
                else
                {
                    tries--;
                    MessageBox.Show("Wrong username and password","Wrong Details", MessageBoxButtons.RetryCancel,MessageBoxIcon.Warning);
                    textBox1.Clear();
                    textBox2.Clear();
                }
                
                
                /*if (uname == username && psw == password)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    tries--;
                    MessageBox.Show("Error");
                } */
                if (tries == 0)
                {
                    button1.Enabled = false;
                    MessageBox.Show("System will terminate");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error fetching user data",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Establishing connection failed", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
