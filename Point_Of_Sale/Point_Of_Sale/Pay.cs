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
    public partial class Pay : Form
    {
        public Pay()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        public delegate void PaymentMadeEvent(object sender, PaymentMadeEventArgs e);
        public event PaymentMadeEvent paymentMade;

        private int payamount;
        public int payAmount {
            get {
                return payamount;
            }
            set {
                payamount = value;
                textBox1.Text = payamount.ToString();
                txtBalance.Text = payamount.ToString(); 
            }
        }

        int AmountToPay;
        //card payement
        private int cardpayment;
        public int cardPayment {
            get {
                return cardpayment;
            }
            set {
                cardpayment = value;
                txtCard.Text = cardpayment.ToString();
            }
        }

        private int mpesapayment;
        public int mpesaPayment {
            get
            {
                return mpesapayment;
            }
            set {
                mpesapayment = value;
                txtMpesa.Text = mpesapayment.ToString();
            }
        }

        //receipt
        private int receipt;
        public int Receipt {
            get {
                return receipt;
            }
            set {
                receipt = value;
                textBox7.Text = receipt.ToString();
            }
        }

        private void Pay_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                textBox6.Focus();
                txtCash.Text = "0";
                txtCard.Text = "0";
                txtMpesa.Text = "0";
                btnProcess.Enabled = false;
                AmountToPay = Convert.ToInt32(textBox1.Text);
                string date = DateTime.Now.Date.ToShortDateString();
                textBox8.Text = date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            paymentMade(this, new PaymentMadeEventArgs() { paySuccess = true });
            saveTransaction();
            this.Close();
        }

        private void saveTransaction()
        {
            //throw new NotImplementedException();
            try
            {
                SqlCeCommand com = new SqlCeCommand(" insert into TblReceipt(Receipt_Id,Amount,Cash,Mpesa,Card,date) values('" + textBox7.Text + "','" + textBox1.Text + "','" + txtCash.Text + "','" + txtMpesa.Text + "','" + txtCard.Text + "','" + textBox8.Text + "') ", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Save Payments Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void SelectMode(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int choice = Convert.ToInt32(textBox6.Text);
                switch (choice)
                { 
                    case 1:
                        txtCash.Enabled = true;
                        txtCash.Focus();
                        textBox6.Clear();
                        break;
                    case 2:
                        Card card = new Card();
                        card.Receipt = Convert.ToInt32(textBox7.Text);
                        card.ShowDialog();
                        if (card.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            txtCard.Text = card.cardAmount.ToString();
                           // MessageBox.Show(card.cardAmount.ToString());
                            int bal = Convert.ToInt32(txtBalance.Text) - Convert.ToInt32(card.cardAmount.ToString());
                            if (bal <= 0)
                            {
                                //amount is ok so process the sale
                                txtBalance.Text = bal.ToString();
                                btnProcess.Enabled = true;
                            }
                            else
                            {
                                txtBalance.Text = bal.ToString();
                            }
                        }
                        break;
                    case 3:
                        Mpesa mpesa = new Mpesa();
                        mpesa.Receipt = Convert.ToInt32(textBox7.Text);
                        mpesa.ShowDialog();
                        if (mpesa.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            txtMpesa.Text = mpesa.mpesaAmount.ToString();
                            int bal = Convert.ToInt32(txtBalance.Text) - Convert.ToInt32(mpesa.mpesaAmount.ToString());
                            if (bal <= 0)
                            {
                                //amount is ok so process the sale
                                txtBalance.Text = bal.ToString();
                                btnProcess.Enabled = true;
                            }
                            else
                            {
                                
                               
                                txtBalance.Text = bal.ToString();
                                
                            }
                        }

                        break;
                }
            }
        }

        private void CashPayment(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
                //check that amount is greater than zero
                if ((txtCash.Text != "") && Convert.ToInt32(txtCash.Text) > 0 ) {
                    int bal = Convert.ToInt32(txtBalance.Text) - Convert.ToInt32(txtCash.Text);
                    if (bal <= 0)
                    {
                      
                        //amount is ok so process the sale
                        txtBalance.Text = bal.ToString();
                        btnProcess.Enabled = true;
                    }
                    else
                    {
                        txtBalance.Text = bal.ToString();
                       
                    }
                }
            }
        }
    }

    public class PaymentMadeEventArgs : EventArgs
    {
        private bool paysuccess;
        public bool paySuccess{
            get {
                return paysuccess;
            }
            set {
                paysuccess = value;
            }
        
    }

    }
}
