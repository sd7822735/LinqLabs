using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //public interface IEnumerable<T>
            
            //摘要:
            //公開支援指定類型集合上簡單反覆運算的列舉值。

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //syntax sugar
            foreach (int n in nums)
            {
                this.listBox1.Items.Add(n);
            }
//            嚴重性 程式碼 說明 專案  檔案 行   隱藏項目狀態
//錯誤  CS1579 因為 'int' 不包含 'GetEnumerator' 的公用執行個體或延伸模組定義，所以 foreach 陳述式無法在型別 'int' 的變數上運作 LinqLabs    C:\shared\LINQ\LinqLabs(Solution)\LinqLabs\1.FrmHelloLinq.cs  35  作用中

//            int a = 999;
//            foreach (int n in a)
//            {

//            }
         
            //========================================
            //C# 內部轉譯
            this.listBox1.Items.Add("=====================");

            System.Collections.IEnumerator en = nums.GetEnumerator();

            while( en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,33 };

            foreach (int n in list)
            {
                this.listBox1.Items.Add(n);
            }
            //==========================n
            int n2 = 100;
            var n1 = 200;
            this.listBox1.Items.Add("====================");
           
            List<int>.Enumerator en = list.GetEnumerator();
            while (en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Step 1: define Data Source
            //Step 2: define query
            //Step 3: execute query

            //Step 1: define Data Source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12 };


            //Setp2: Define Query
            //define query  (IEnumerable<int> q 是一個  Iterator 物件), 如陣列集合一般 (陣列集合也是一個  Iterator 物件)
            //迭代器（iterator）

            //IEnumerable<int> q -  公開支援指定型別集合上簡單反覆運算的列舉值。
            IEnumerable<int> q = from  n in nums
                                  //where (n >= 5 && n<=8 ) && (n%2==0)
                                  where n<3 || n>8
                                  select n;

            //Step 3: Execute Query
            //execute query(執行 iterator - 逐一查看集合的item)
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            //Step 1: define Data Source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


            //Setp2: Define Query
            //define query  (IEnumerable<int> q 是一個  Iterator 物件), 如陣列集合一般 (陣列集合也是一個  Iterator 物件)
            //迭代器（iterator）

            //IEnumerable<int> q -  公開支援指定型別集合上簡單反覆運算的列舉值。
            IEnumerable<int> q = from n in nums
                                 where IsEven(n)
                                 select n;

            //Step 3: Execute Query
            //execute query(執行 iterator - 逐一查看集合的item)
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        bool IsEven(int n)
        {
            //if (n%2==0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //return (n % 2 == 0) ? true : false;
            return n % 2 == 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            IEnumerable<Point> q = from n in nums
                                   where n > 5
                                   select new Point(n, n * n);


            //execute query
            foreach (Point pt in q)
            {
                this.listBox1.Items.Add(pt.X + ", " + pt.Y);
            }

            //===================================
            //execute query
            List<Point> list =  q.ToList();  //foreach(..item...in q..){ list.Add(item)}....return list;
            this.dataGridView1.DataSource = list;

            //=========================

            this.chart1.DataSource = list;
            this.chart1.Series[0].XValueMember = "X";
            this.chart1.Series[0].YValueMembers = "Y";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line; ;

            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[0].BorderWidth = 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] words = { "aaa", "Apple", "pineApple", "xxxapple" };

           IEnumerable<string> q = from w in words
                                   where w.ToLower().Contains("apple") && w.Length >5
                                   select w;

            foreach (string s in q)
            {
                this.listBox1.Items.Add(s);
            }

            //=========================================
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //this.dataGridView1.DataSource = this.nwDataSet1.Products;

            IEnumerable<global::LinqLabs.NWDataSet.ProductsRow> q = from p in this.nwDataSet1.Products
                                                                     where ! p.IsUnitPriceNull() &&  p.UnitPrice > 30 && p.UnitPrice<50 && p.ProductName.StartsWith("M")
                                                                     select p;

            this.dataGridView1.DataSource=  q.ToList();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var q = from o in this.nwDataSet1.Orders
                    where o.OrderDate.Year == 1997
                    orderby o.OrderDate descending
                    select o;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5,6,7,8,9,10 };

            //var q = from n in nums
            //        where n > 5
            //        select n;

           // IEnumerable<int> q = nums.Where(.......delegate.....).Select(....);

        }

        private void button49_Click(object sender, EventArgs e)
        {
//            #region 組件 System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//            // C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.Core.dll
//            #endregion

          

//namespace System.Linq
        }
    }
}
