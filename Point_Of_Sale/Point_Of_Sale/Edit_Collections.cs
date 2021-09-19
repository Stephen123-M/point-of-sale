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
    public partial class Edit_Collections : Form
    {
        public Edit_Collections()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");

        private void Edit_Collections_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                textBox1.Text = DateTime.Now.Date.ToShortDateString();
                get_collections();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        //get the collections
        private void get_collections()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblReceipt", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int cash = 0, card = 0, mpesa = 0;
                    int j = 0;
                    while (j < dt.Rows.Count)
                    {
                        cash = cash + Convert.ToInt32(dt.Rows[j]["Cash"].ToString());
                        card = card + Convert.ToInt32(dt.Rows[j]["Card"].ToString());
                        mpesa = mpesa + Convert.ToInt32(dt.Rows[j]["Mpesa"].ToString());

                        j++;
                    }

                    textBox2.Text = cash.ToString();
                    textBox3.Text = card.ToString();
                    textBox4.Text = mpesa.ToString();

                    textBox5.Text = (cash + card + mpesa).ToString();

                }
                else
                {
                    //no sales
                    textBox2.Text = "0";
                    textBox3.Text = "0";
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error getting payments", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("insert into TblCollections(Cash,Card,Mpesa,Total,date,cash_counted,card_counted,mpesa_counted,cash_dif,card_dif,mpesa_dif) values('" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox1.Text + "','" + textBox8.Text + "','" + textBox7.Text + "','" + textBox6.Text + "','" + textBox11.Text + "','" + textBox10.Text + "','" + textBox9.Text + "') ", con);
                com.ExecuteNonQuery();
                MessageBox.Show("Collections Saved ", "Success", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error Saving Collections", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exit Colletions","Exit", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
                this.Dispose();
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }
    }
}
