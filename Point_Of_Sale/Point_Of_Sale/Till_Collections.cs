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
    public partial class Till_Collections : Form
    {
        public Till_Collections()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");
        //all totals
        int Total_Amount = 0;
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Till_Collections_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                textBox1.Text = DateTime.Now.Date.ToShortDateString();

                getCardPayments();
                getTotalCashMpesa();
                Total_Amount = Total_Amount + Convert.ToInt32(textBox2.Text) + Convert.ToInt32(textBox4.Text) +Convert.ToInt32(textBox3.Text);
                textBox5.Text = Total_Amount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //get total amount of cash and mpesa
        private void getTotalCashMpesa()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select * from TblReceipt ", con);
                adt.Fill(dt);
                int cash = 0;
                int mpesa = 0;
                if (dt.Rows.Count > 0)
                {
                   
                    int k = 0;
                    while (k < dt.Rows.Count)
                    {
                        cash = cash + Convert.ToInt32(dt.Rows[k]["Cash"].ToString());
                        mpesa = mpesa + Convert.ToInt32(dt.Rows[k]["Mpesa"].ToString());
                        k++;
                    }
                    textBox2.Text = cash.ToString();
                    textBox3.Text = mpesa.ToString();

                }
                else 
                { 
                    //do data
                    textBox2.Text = cash.ToString();
                    textBox3.Text = mpesa.ToString();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"PAYMENT ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //get all card payments
        private void getCardPayments()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter(" select * from TblCard ", con);
                adt.Fill(dt);
                //check if there are any card payments
                if (dt.Rows.Count > 0)
                {
                    string card_id, name, card_no, date, amount;
                    int j = 0;
                    int Total = 0;
                    while(j < dt.Rows.Count){
                        card_id = dt.Rows[j]["card_id"].ToString();
                        name = dt.Rows[j]["name"].ToString();
                        card_no = dt.Rows[j]["card_no"].ToString();
                        date = dt.Rows[j]["date"].ToString();
                        amount = dt.Rows[j]["amount"].ToString();
                        Total = Total + Convert.ToInt32(amount);

                        //add the values to the grid
                        dataGridView1.Rows.Add(card_id, name, card_no, date, amount);
                        j++;
                    }

                    textBox4.Text = Total.ToString();

                }
                else
                {
                    textBox4.Text = "0";
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Card Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
