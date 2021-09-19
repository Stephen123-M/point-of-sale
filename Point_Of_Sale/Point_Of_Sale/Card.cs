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
    public partial class Card : Form
    {
        public Card()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        private int cardamount;
        public int cardAmount {
            get {
                return cardamount;
            }
            set {
                cardamount = value;
            }
        }

        string date;
        int receiptNo;
        private int receipt;
        public int Receipt {
            get {
                return receipt;
            }
            set {
                receipt = value;
                receiptNo = receipt;
            }
        }
        private void Card_Load(object sender, EventArgs e)
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

        private void btnCardPay_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter valid amount", "INVALID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Pay pay = new Pay();
                // pay.cardPayment = Convert.ToInt32(textBox3.Text);
                cardamount = Convert.ToInt32(textBox3.Text);

                saveDetails();
                this.Close();
            }
        }

        private void saveDetails()
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand(" insert into TblCard(card_id,name,card_no,date,amount) values('" + receiptNo + "','" + textBox1.Text + "','" + textBox2.Text + "','" + date + "','" + textBox3.Text + "') ", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"CARD PAYMENT ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
