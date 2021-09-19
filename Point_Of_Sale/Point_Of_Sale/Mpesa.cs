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
    public partial class Mpesa : Form
    {
        public Mpesa()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");
       // DateTime.Now.Date.ToShortDateString();
        private int mpesaamout;
        public int mpesaAmount
        {
            get
            {
                return mpesaamout;
            }
            set
            {
                mpesaamout = value;
            }

        }

        int receiptNo;
        private int receipt;
        public int Receipt
        {
            get
            {
                return receipt;
            }
            set
            {
                receipt = value;
                receiptNo = receipt;
            }
        }

        string date;
        private void Mpesa_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                date = DateTime.Now.Date.ToShortDateString(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Enter Valid Amount","INVALID AMOUNT",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    mpesaamout = Convert.ToInt32(textBox2.Text);

                    SqlCeCommand com = new SqlCeCommand(" insert into TblMpesa(id,transaction_ref,amount,date) values('" + receiptNo.ToString() + "','" + textBox1.Text + "','" + textBox2.Text + "','" + date+ "') ", con);
                    com.ExecuteNonQuery();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),"MPESA ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
    }
}
