using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//entity data model 特色
//1. App.config 連接字串
//2. Package 套件下載, 參考 EntityFramework.dll, EntityFramework.SqlServer.dll
//3. 導覽屬性 關聯
//物件關聯對映（英語：Object Relational Mapping，簡稱ORM，或O / RM，或O / R mapping），


//4. DataSet model 需要處理 DBNull; Entity Model  不需要處理 DBNull (DBNull 會被 ignore)
//5. IQuerable<T> query 執行時會轉成 => T-SQL


namespace Starter
{
    public partial class FrmLinq_To_Entity : Form
    {
        public FrmLinq_To_Entity()
        {
            InitializeComponent();

            Console.Write("xxxx...opep()..select * from products....close()............");

            this.dbContext.Database.Log = Console.Write ;
        }
        
        //in Memory DB - dbContext;
        NorthwindEntities dbContext = new NorthwindEntities();
       
        private void button1_Click(object sender, EventArgs e)
        {
           
            var q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;

           this.dataGridView1.DataSource =  q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             
            this.dataGridView1.DataSource = this.dbContext.Categories.First().Products.ToList();

            MessageBox.Show(this.dbContext.Products.First().Category.CategoryName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           this.dataGridView1.DataSource =  this.dbContext.Sales_by_Year(new DateTime(1997, 1, 1), DateTime.Now).ToList();
        }

        private void button22_Click(object sender, EventArgs e)
        {
           // System.NotSupportedException: 'LINQ to Entities 無法辨識方法 'System.String Format(System.String, System.Object)' 方法，而且這個方法無法轉譯成存放區運算式。'
           //=> AsEnumerable()

            var q = from p in this.dbContext.Products.AsEnumerable()
                    orderby p.UnitsInStock descending, p.ProductID descending
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = $"{p.UnitPrice * p.UnitsInStock:c2}" 
                    };


            this.dataGridView1.DataSource = q.ToList();
         
            //===========================

            var q2 = this.dbContext.Products.OrderByDescending(p => p.UnitsInStock).ThenByDescending(p => p.ProductID);
            this.dataGridView2.DataSource = q2.ToList();

            var q3 = dbContext.Products.OrderBy(n => n.ProductName).ThenBy(p => p.UnitPrice);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ////============================
            ////自訂 compare logic
            var q3 = dbContext.Products.AsEnumerable().OrderBy(p => p, new MyComparer()).ToList();
            this.dataGridView2.DataSource = q3.ToList();
        }

        class MyComparer : IComparer<Product>
        {

            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            var q2 = from c in this.dbContext.Categories
                     join p in this.dbContext.Products
                     on c.CategoryID equals p.CategoryID
                     select new { c.CategoryID, c.CategoryName, p.ProductName, p.UnitPrice };
                  
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbContext.Products
                    select new { p.CategoryID, p.Category.CategoryName, p.ProductName, p.UnitPrice };

            this.dataGridView3.DataSource = q.ToList();

        }

        private void button21_Click(object sender, EventArgs e)
        {
            //inner join --oo 物件化
            var q = from c in this.dbContext.Categories
                    from p in c.Products
                    select new { c.CategoryID, c.CategoryName, p.ProductName, p.UnitPrice };

            this.dataGridView1.DataSource = q.ToList();
           
            //=========================================
            //this.dbContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice});

            //cross join
            var q2 = from c in this.dbContext.Categories
                     from p in this.dbContext.Products
                     select new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock };
            MessageBox.Show("q2.count() =" + q2.Count());
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbContext.Products
                    where p.Category !=null
                    group p by p.Category.CategoryName into g
                    select new {CategoryName= g.Key, AvgUnitPrice= g.Average(p => p.UnitPrice) };

            this.dataGridView1.DataSource = q.ToList();

        }

        private void button14_Click(object sender, EventArgs e)
        {
            bool? b;
            b = true;
            b = false;
            b = null;


            var q = from o in this.dbContext.Orders
                    group o by o.OrderDate.Value.Year into g
                    select new { Year= g.Key, Count= g.Count() };

            this.dataGridView1.DataSource = q.ToList();

            //====================
            var q2 = from o in this.dbContext.Orders
                    group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                    select new { Year = g.Key, Count = g.Count() };

            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            //Add Product

            Product product = new Product { ProductName = "Test " + DateTime.Now.ToString(), Discontinued = true };
            this.dbContext.Products.Add(product);

            this.dbContext.SaveChanges();

            this.Read_RefreshDataGridView();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            //update
            var product = (from p in this.dbContext.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();

            if (product == null) return; //exit method

            product.ProductName = "Test" + product.ProductName;

            this.dbContext.SaveChanges();

            this.Read_RefreshDataGridView();
        }

        private void button53_Click(object sender, EventArgs e)
        {

            //delete one product
            var product = (from p in this.dbContext.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();

            if (product == null) return;

            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();

            this.Read_RefreshDataGridView();
        }

        void Read_RefreshDataGridView()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.dbContext.Products.ToList();

        }
    }
}