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
    public partial class TILL : Form
    {
        public TILL()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\user\documents\visual studio 2010\Projects\Point_Of_Sale\Point_Of_Sale\PointOfSale.sdf");
       //get price and item name
        private int prc = 0;
        private string item_name = null;
        
        private void TILL_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                //auto generate the first receipt id
                getReceipt();
                textBox2.Focus();

                //get date
                string date = DateTime.Now.Date.ToShortDateString();
                string time = DateTime.Now.ToString("HH:mm:ss tt");
                textBox5.Text = time;
                textBox6.Text = date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //get max receipt id

        private void getReceipt()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlCeCommand com = new SqlCeCommand(" select max(Till_Id) from TblTill ", con);
                SqlCeDataReader reader = com.ExecuteReader();
                if (reader.Read())
                {
                    //get the max value
                    string receipt = reader[0].ToString();
                    if (receipt == "")
                    {
                        textBox1.Text = "1";
                    }
                    else
                    {
                        //increament the value by 1
                        int receipt_no = Convert.ToInt32(receipt) + 1;
                        //display the value
                        textBox1.Text = receipt_no.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void getItem(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if ((dataGridView1.RowCount - 1) > 0)
            {
                //MessageBox.Show("Hello " + dataGridView1.CurrentRow.Cells[0].Value);
               // bool found = false;
                
                get_item_details(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                if ((prc != 0) && (item_name != null))
                {
                    
                    bool found = false;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToString(row.Cells[1].Value) == (string)item_name)
                        {
                            found = true;
                            row.Cells[2].Value = 1 + Convert.ToInt32(row.Cells[2].Value);
                            //dataGridView1.CurrentRow.Cells[2].Value = 1 + Convert.ToInt32(dataGridView1.CurrentRow.Cells[2].Value);
                            row.Cells[4].Value = Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(row.Cells[3].Value);

                            //dataGridView1.Rows[row.Index - 1].Cells[0].Selected = true;
                            int currentRows = dataGridView1.RowCount-1;
                            dataGridView1.Rows.RemoveAt(currentRows);

                        }
                    }
                    if (!found)
                    {
                        dataGridView1.CurrentRow.Cells[1].Value = item_name;
                        dataGridView1.CurrentRow.Cells[2].Value = 1;
                        dataGridView1.CurrentRow.Cells[3].Value = prc;
                        dataGridView1.CurrentRow.Cells[4].Value = prc;
                        //Convert.ToInt32(dataGridView1.CurrentRow.Cells[2].Value) * Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value);
                    }
                
                }
                else
                {
                    //item not found;
                    get_item_details("");
                    return;
                }
                
                
            }
             * 
             */
            

        }

        //fetch the item
        private void get_item_details()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select Product_Name, Price from TblProduct where Product_Id ='" + Convert.ToInt32(textBox2.Text) + "'  ", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    item_name = dt.Rows[0]["Product_Name"].ToString();
                    prc = Convert.ToInt32(dt.Rows[0]["Price"].ToString());
                }
                else
                {

                    MessageBox.Show("Item not available");
                    prc = 0;
                    item_name = "";
                    textBox2.Clear();
                    textBox2.Focus();
                    return;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GetITems(object sender, KeyPressEventArgs e)
        {
            
        }

        private void SearchItem(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Enter valid product code");
                    textBox2.Focus();
                    return;
                }
                else
                {
                    get_item_details();
                    if ((item_name != "") && (prc != 0))
                    {
                        //valid details found procced and add to the list
                        //MessageBox.Show("Product " + item_name + " Price " + prc.ToString());

                        bool found = false;
                        if(dataGridView1.Rows.Count > 0)
                        {
                        
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            //check if product in list
                            if (Convert.ToString(row.Cells[1].Value) == (string)item_name)
                            {
                                //item exists so update quantity and price
                                row.Cells[2].Value = 1 + Convert.ToInt32(row.Cells[2].Value);
                                row.Cells[4].Value = Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(row.Cells[3].Value);
                                found = true;
                            }
                            
                        }
                        if (!found)
                        {
                            //add the items
                            dataGridView1.Rows.Add(textBox2.Text, item_name, 1, prc, prc);
                        }

                    }
                        else
                        {
                            dataGridView1.Rows.Add(textBox2.Text, item_name, 1, prc, prc);
                        }


                        textBox2.Clear();
                        textBox2.Focus();

                        int amount = 0;
                        int items = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++ )
                        {
                            amount = amount + Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                            items = items + Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                        }

                        //dispaly amount
                        textBox3.Text = amount.ToString();
                        textBox4.Text = items.ToString();
                    }
                   
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter a valid sale to continue");
                textBox2.Focus();
                return;
            }
            else
            {
                Pay p = new Pay();
                p.payAmount = Convert.ToInt32(textBox3.Text);
                p.Receipt = Convert.ToInt32(textBox1.Text);
                p.paymentMade += new Pay.PaymentMadeEvent(p_paymentMade);
                p.ShowDialog();
            }
        }

        void p_paymentMade(object sender, PaymentMadeEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.paySuccess == true)
            {
                MessageBox.Show("Transaction Complete","SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information  );

                //clear all text inputs and start a new sale
                //save transation details
                saveSale();
                //clear inputs
                dataGridView1.Rows.Clear();
                textBox4.Clear();
                textBox3.Clear();
                textBox2.Focus();
                getReceipt();

               
            }
        }

        //private void save sale
        private void saveSale()
        {
            try
            {
                SqlCeCommand com;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    com = new SqlCeCommand("insert into TblTill( Till_Id,Product_Id,Product_Name,Qty,Amount,Total,Date ) values('" + textBox1.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','" + textBox6.Text + "')  ", con);
                    com.ExecuteNonQuery();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void tillCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Till_Collections collection = new Till_Collections();
            collection.ShowDialog();
        }

        private void editCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit_Collections edit = new Edit_Collections();
            edit.ShowDialog();
        }

        private void endOfDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Please note that you won't be able to make sales agian", "CLOSE TILL", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //MessageBox.Show("Close");
                try
                {
                   // SqlCeCommand com = new SqlCeCommand("insert into TblTillClose select * from TblTill ", con);
                   // com.ExecuteNonQuery();
                   // MessageBox.Show("Test");
                    Till_Close();
                    Receipt_Close();
                    Card_Close();
                    Mpesa_Close();

                    //get the new receipt id
                    getReceipt();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),"Till Data Not Cleared", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Test");
            }
        }

        //end of day transactions
        private void Till_Close()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblTill", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //save all the data
                    SqlCeCommand com = new SqlCeCommand("insert into TblTillClose select * from TblTill ", con);
                    com.ExecuteNonQuery();

                   // StreamWriter sw = new StreamWriter("../../till_close

                    //delete the data

                    Clear_Till_Data();

                }
                else
                {
                    //populate the table with zero data

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Till Not Cleared",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void Receipt_Close()
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
                    //save all the data

                    SqlCeCommand com = new SqlCeCommand("insert into TblReceiptClose select * from TblReceipt ", con);
                     com.ExecuteNonQuery();

                    //delete data

                     Clear_Receipt_Data();
                }
                else
                { 
                    //populate the table with zero data

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Receipt Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Card_Close()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblCard", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //save all the data
                    SqlCeCommand com = new SqlCeCommand("insert into TblCardClose select * from TblCard ", con);
                    com.ExecuteNonQuery();

                    //clear the table
                    Clear_Card_Data();
                }
                else
                {
                    //populate the table with zero data

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Cards Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Mpesa_Close()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                SqlCeDataAdapter adt = new SqlCeDataAdapter("select * from TblMpesa", con);
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //save all the data
                    SqlCeCommand com = new SqlCeCommand("insert into TblMpesClose select * from TblMpesa ", con);
                    com.ExecuteNonQuery();

                    //delete the data
                    Clear_Mpesa_Data();


                }
                else
                {
                    //populate the table with zero data

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mpesa Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //clear all the data
        private void Clear_Till_Data()
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("delete from TblTill", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Till Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear_Receipt_Data()
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("delete from TblReceipt", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Receipt Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear_Card_Data()
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("delete from TblCard", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Card Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear_Mpesa_Data()
        {
            try
            {
                SqlCeCommand com = new SqlCeCommand("delete from TblMpesa", con);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mpesa Not Cleared", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


    }
}
