using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqLabs;

namespace MyHomeWork
{
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();

            //productsTableAdapter1.Fill(nwDataSet1.Products);
            //ordersTableAdapter1.Fill(nwDataSet1.Orders);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 ,16};

            //較複雜
            //var q = from n in nums
            //        group n by Mykey(n) into g
            //        select new
            //        {
            //            MyKey = g.Key,
            //            MyCount = g.Count(),
            //            MyGroup = g
            //        };

            //treeView1.Nodes.Clear();
            //foreach(var group in q)
            //{
            //    string s = $"{group.MyKey}  ({group.MyCount})";
            //    TreeNode node = treeView1.Nodes.Add(group.MyKey, s);
            //    foreach(var item in group.MyGroup)
            //    {
            //        node.Nodes.Add(item.ToString());
            //    }
            //}

            //========================================================
            //較簡單
            var q1 = from n in nums
                     group n by Mykey(n) into g
                     select g;

            foreach(var group in q1)
            {
                TreeNode node = treeView1.Nodes.Add(group.Key);
                foreach(var item in group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }       
        }

        private string Mykey(int n)
        {
            if (n <= 5)
            {
                return "低段";
            }
            else if(n > 5 && n <= 10)
            {
                return "中段";
            }
            else
            {
                return "高段";
            }

        }


        NorthwindEntities dbContext = new NorthwindEntities();
        private void button8_Click(object sender, EventArgs e)
        {
            var q = from p in dbContext.Products.AsEnumerable()
                    where p.UnitPrice != null
                    group p by MyPriceLevel(p.UnitPrice) into g
                    select new
                    {
                        Key = g.Key,
                        Count = g.Count()
                    };

            dataGridView1.DataSource = q.ToList();
 
        }

        private string MyPriceLevel(decimal? n)
        {
            if (n <= 20)
            {
                return "(低) 價格";
            }
            else if (n <= 50 && n > 20)
            {
                return "(中) 價格";
            }
            else if(n>50)
            {
                return "(高) 價格";
            }
            else
            {
                return "空值";
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var q = from o in dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    orderby g.Key 
                    select new
                    {
                        g.Key,
                        Count=g.Count()
                    };

            dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var q = from o in dbContext.Orders
                    group o by new { o.OrderDate.Value.Year,o.OrderDate.Value.Month } into g
                    orderby g.Key
                    select new
                    {
                        g.Key,
                        Count = g.Count()
                    };

            dataGridView2.DataSource = q.ToList();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from f in files
                    group f by f.Length into g
                    orderby g.Key descending
                    select new
                    {
                        Key = g.Key,
                        Count = g.Count()
                    };
            dataGridView1.DataSource = q.ToList();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from f in files
                    group f by f.LastWriteTime into g
                    orderby g.Key descending
                    select g;
                   
            dataGridView2.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q1 = from o in dbContext.Order_Details
                     select new
                     {
                         o.Product.ProductName,
                         o.UnitPrice,
                         o.Quantity,
                         o.Discount
                     };
            dataGridView1.DataSource = q1.ToList();


            var q = dbContext.Order_Details.Sum(n => (double)n.UnitPrice * n.Quantity * (1 - n.Discount));
            MessageBox.Show("總銷售金額為 : " + q);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //todo未完成
            var q = (from d in dbContext.Order_Details.AsEnumerable()
                    from o in dbContext.Orders
                    group d by $"{o.Employee.FirstName}  {o.Employee.LastName}" into g
                    select new
                    {Name =g.Key,Money = g.Sum(d => (int)d.UnitPrice*d.Quantity*(1-d.Discount))})
                    .OrderByDescending(d=>d.Money).Take(5);
            dataGridView1.DataSource = q.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var q = dbContext.Products.Where(p => p.UnitPrice > 300);
            MessageBox.Show(q.ToList().Count()>0?"True":"False");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var q = (dbContext.Products.OrderByDescending(p => p.UnitPrice).Select(p => 
            new { 商品=p.ProductName,單價=p.UnitPrice,種類=p.Category.CategoryName })).Take(5) ;
            dataGridView2.DataSource = q.ToList();

        }
    }
}
