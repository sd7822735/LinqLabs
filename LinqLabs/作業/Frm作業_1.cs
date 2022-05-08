using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();

            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);
            productsTableAdapter1.Fill(nwDataSet1.Products);
            LoadComboBox();

        }

        private void LoadComboBox()
        {
            var q = from o in nwDataSet1.Orders
                    select o.OrderDate.Year;
            foreach(var item in q.Distinct())
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
        int page = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)
            var q = from p in nwDataSet1.Products select p;
            int n = int.Parse(textBox1.Text);
            int totalpage = q.Count() / n;
            if (page <= totalpage)
            {
                dataGridView2.DataSource = q.Skip(n * page).Take(n).ToList();
                page ++;
            }

        }


        //Distinct()
   

        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles("*.log");

            this.dataGridView1.DataSource = files;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from f in files
                    where f.CreationTime.Year== 2019
                    select f;
            dataGridView2.DataSource = q.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            var q = dir.GetFiles().Where(f => f.Length > 1000000);
            dataGridView1.DataSource = q.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = nwDataSet1.Orders;
            dataGridView2.DataSource = nwDataSet1.Order_Details;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var q = from o in nwDataSet1.Orders
                    where o.OrderDate.Year == (int)comboBox1.SelectedItem
                    select o;
            dataGridView1.DataSource = q.ToList();
            //按時不會顯示detail
            var q1 = from od in nwDataSet1.Order_Details
                     where od.OrderID ==q.First().OrderID
                     select od;
            dataGridView2.DataSource = q1.ToList();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                int value = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                var q = from od in nwDataSet1.Order_Details
                        where od.OrderID == value
                        select od;
                dataGridView2.DataSource = q.ToList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            productsTableAdapter1.Fill(nwDataSet1.Products);
            dataGridView1.DataSource = nwDataSet1.Products;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var q = from p in nwDataSet1.Products select p;
            int n = int.Parse(textBox1.Text);
            int totalpage = q.Count() / n;
            if (page < totalpage)
            {
                --page;
                dataGridView2.DataSource = q.Skip(n * page).Take(n).ToList();
                
            }
        }
    }
}
