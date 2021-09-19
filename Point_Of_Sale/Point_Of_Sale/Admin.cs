using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Point_Of_Sale
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users user = new Users();
            user.ShowDialog();
        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exit system","EXIT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
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

        private void manageOutletsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutLet outlet = new OutLet();
            outlet.ShowDialog();
        }

        private void productCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            category.ShowDialog();
        }

        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            product.ShowDialog();
        }

        private void transferStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Purchase_Stock ps = new Purchase_Stock();
            ps.ShowDialog();
        }
    }
}
