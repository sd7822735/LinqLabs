using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//組件 System.Core.dll,
//namespace {}  System.Linq
//public static class Enumerable
//


//public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

//1. 泛型 (泛用方法)                                          (ex.  void SwapAnyType<T>(ref T a, ref T b)
//2. 委派參數 Lambda Expression (匿名方法簡潔版)               (ex.  MyWhere(nums, n => n %2==0);
//3. 回傳 Iterator                                            (ex.  MyIterator)
//4. 擴充方法                                                  (ex. WordCount()  Chars(2))

namespace Starter
{


    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int n1, n2;
            n1 = 100;
            n2 = 200;

            MessageBox.Show(n1 + "," + n2);

            Swap(ref n1, ref n2);

            MessageBox.Show(n1 + "," + n2);
            //=======================
            string s1, s2;
            s1 = "aaaa";
            s2 = "bbbb";
            MessageBox.Show(s1 + "," + s2);
            Swap(ref s1, ref s2);
            MessageBox.Show(s1 + "," + s2);

        }

        void SwapObject(ref object n1, ref object n2)
        {
          
            object temp = n2;
            n2 = n1;
            n1 = temp;
        }


        static  void SwapAnyType<T>(ref T n1, ref T n2)
        {
            T temp = n2;
            n2 = n1;
            n1 = temp;
        }

        void Swap(ref string n1, ref string n2)
        {
            string temp = n2;
            n2 = n1;
            n1 = temp;
        }
        void Swap(ref int n1, ref int n2)
        {
            int temp = n2;
            n2 = n1;
            n1 = temp;
        }
        void Swap(ref Point n1, ref Point n2)
        {
            Point temp = n2;
            n2 = n1;
            n1 = temp;
        }
        void Swap(int n1, int n2, out int n3, out int n4)
        {
            n3 = n2;
            n4 = n1;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int n1, n2;
            n1 = 100;
            n2 = 200;
            MessageBox.Show(n1 + "," + n2);
            //SwapAnyType<int>(ref n1, ref n2);
          
            SwapAnyType(ref n1, ref n2); // 推斷型別

            MessageBox.Show(n1 + "," + n2);

            //==============================
            string s1, s2;
            s1 = "aaa";
            s2 = "bbb";
            MessageBox.Show(s1 + "," + s2);
          
            SwapAnyType(ref s1, ref s2);

            MessageBox.Show(s1 + "," + s2);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //            嚴重性 程式碼 說明 專案  檔案 行   隱藏項目狀態
            //錯誤  CS0123  'ButtonX_Click' 沒有任何多載符合委派 'EventHandler'   LinqLabs C:\shared\LINQ\LinqLabs(Solution)\LinqLabs\2.FrmLangForLINQ.cs    108 作用中

            //            this.buttonX.Click += ButtonX_Click;

            //C# 1.0 具名方法
            this.buttonX.Click += new EventHandler(aaa);
            this.buttonX.Click += bbb;   

            //===============================
            //C# 2.0 匿名方法
            this.buttonX.Click += delegate (object sender1, EventArgs e1)
                                      {
                                          MessageBox.Show("C# 2.0 匿名方法");
                                      };

            //C# 3.0 匿名方法  lambda 運算式 => goes to
            this.buttonX.Click += (object sender1, EventArgs e1) => 
                                 // { 
                                      MessageBox.Show("C# 2.0 匿名方法 lambda ");
                                  //};


        }

        private void ButtonX_Click()
        {
            MessageBox.Show("buttonX click");
        }

        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }
        private void bbb(object sender, EventArgs e)
        {
            MessageBox.Show("bbb");
        }

        bool Test(int n)
        {
            //if (n > 5)
            //    return true;
            //else
            //    return false;

            return n > 5;
        }

        bool Test1(int n) //, int x)
        {
            return n % 2 == 0;
        }

        //Step 1: create delegate 型別
        //Step 2: create delegate Object (new ...)
        //Step 3: invoke / call method

        delegate bool MyDelegate(int n);

        private void button9_Click(object sender, EventArgs e)
        {
            bool result = Test(4);
            MessageBox.Show("result = " + result);
            //==========================

            MyDelegate delegateObj = new MyDelegate(Test);
            result = delegateObj.Invoke(7); //call method (_)
            MessageBox.Show("result = " + result);

            //=============================
            delegateObj = Test1;  //syntax sugar
            result= delegateObj(3);
            MessageBox.Show("result = " + result);


            //===============================
            //C# 2.0 匿名方法
            delegateObj = delegate (int n)
                                   {
                                       return n > 5;
                                   };

            result =  delegateObj(6);
            MessageBox.Show("result = " + result);

            //====================================
            //C# 3.0 匿名方法簡潔版 labmda expression 
            //Lambda 運算式是建立委派最簡單的方法 (參數型別也沒寫 / return 也沒寫 => 非常高階的抽象)
            delegateObj = n => n > 5;
           result = delegateObj(1);

            MessageBox.Show("result = " + result);

        }

        List<int> MyWhere(int[] nums,  MyDelegate delegateObj)
        {
            List<int> list = new List<int>();
            //.....
            foreach (int n in nums)
            {
                if ( delegateObj(n))
                {
                    list.Add(n);
                }  
            }
            return list;
        }


        private void button10_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ,11,13,15};

            List<int> Large_list =   MyWhere(nums, Test1);

            //foreach (int n in Large_list)
            //{
            //    this.listBox1.Items.Add(n);
            //}
            //=================================

            List<int> list1 =  MyWhere(nums, n => n > 5);
            List<int> oddList = MyWhere(nums, n => n %2==1);
            List<int> evenList = MyWhere(nums, n => n %2==0);
          
            foreach (int n in oddList)
            {
                this.listBox1.Items.Add(n);
            }

            foreach (int n in evenList)
            {
                this.listBox2.Items.Add(n);
            }
        }


        IEnumerable<int> MyIterator(int[] nums, MyDelegate delegateObj)
        {
            foreach (int n in nums)
            {
                if (delegateObj(n)) //call method
                {
                    yield return n;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            IEnumerable<int> q =  MyIterator(nums, n => n > 5);

            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //var q = from n in nums
            //        where n > 5
            //        select n;

            IEnumerable<int> q = nums.Where<int>(n => n > 5);

            foreach(int n in q)
            {
                this.listBox1.Items.Add(n);
            }

            //============================================================
            string[] words = { "aaa", "bbbbbb", "ccccccc" };

            IEnumerable<string> q1 = words.Where<string>(w => w.Length > 3);

            foreach (string w in q1)
            {
                this.listBox2.Items.Add(w);
            }

           this.dataGridView1.DataSource =  q1.ToList();

            //====================================
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

            var q2 = this.nwDataSet1.Products.Where(p => p.UnitPrice > 30);

            this.dataGridView2.DataSource =  q2.ToList();
         
            //foreach (.... in q2)
            //{

            //}
        
        }

        private void button45_Click(object sender, EventArgs e)
        {
            //var 懶得寫(x)
            //========================
            //var 型別難寫
            //var for 匿名型別

            var n = 100;

            var s = "abc";
            MessageBox.Show(s.ToUpper());

            var p = new Point(100, 100);
            MessageBox.Show(p.X + "," + p.Y);
           
        }

        private void button41_Click(object sender, EventArgs e)
        {
            MyPoint pt1= new MyPoint(); //constructor
          
            pt1.P1 = 100;    //set
            int w =  pt1.P1; //get

            pt1.P2 =200;
            //MessageBox.Show(pt1.P2.ToString());  //get

            //===============================================
            List<MyPoint> list = new List<MyPoint>();

            //() constructor 建構子方法
            list.Add(new MyPoint());
            list.Add(new MyPoint(100));
            list.Add(new MyPoint(99,99));
            list.Add(new MyPoint("xxxx"));

            //{} object initialize 物件初始化
            list.Add(new MyPoint { P1 = 1, P2 = 1, Field1 = "aaa", Field2 = "aaa" });
            list.Add(new MyPoint { P1 = 333 });
            list.Add(new MyPoint { P1 = 3, P2 = 3, Field1 = "aaa", Field2 = "aaa" });


            this.dataGridView1.DataSource = list;

            //=============================
            List<MyPoint> list2 = new List<MyPoint>
            {
                new MyPoint { P1=1, P2=2, Field1="xxx"},
                new MyPoint { P1=11, P2=2, Field1="xxx"},
                new MyPoint { P1=111, P2=2, Field1="xxx"},
                new MyPoint { P1=11111, P2=2, Field1="xxx"},
            };
            this.dataGridView2.DataSource = list2;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var  x =  new  { P1 = 99, P2 = 88, P3 = 33 };
            //x.P1 = 9999;
            int w = x.P1;
          
            
            var y = new { P1 = 99, P2 = 88, P3 = 33 };

            var z = new { UserName = "aaa", Passwor = "bbb" };

            this.listBox1.Items.Add(x.GetType());
            this.listBox1.Items.Add(y.GetType());
            this.listBox1.Items.Add(z.GetType());

            //===========================
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

           
            //var q = from n in nums
            //        where n > 5
            //        select new {  N = n, S = n * n, C = n * n * n };

            var q = nums.Where(n => n > 5).Select(n => new { N = n, S = n * n, C = n * n * n });


            this.dataGridView1.DataSource =  q.ToList();
            //==================================
          
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            
            var q2 = from p in this.nwDataSet1.Products
                     where p.UnitPrice > 30
                     select new 
                     {
                         ID = p.ProductID,
                         產品名稱 = p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice = $"{p.UnitPrice * p.UnitsInStock:c2}" 
                     };

            this.dataGridView2.DataSource =  q2.ToList();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            //具名型別陣列
            Point[] pts = new Point[]{
                                 new Point(10,10),
                                 new Point(20, 20)
                                };

            //匿名型別陣列
            var arr = new[] {
                                new { x = 1, y = 1 },
                                new { x = 2, y = 2 }
                             };

        }

        private void button32_Click(object sender, EventArgs e)
        {
            string s1 = "abcd";
            int n = s1.WordCount();

            MessageBox.Show("WordCount = " + n);
            //=====================
            string s2 = "123456789";
            n = s2.WordCount();
            MessageBox.Show("WordCount = " + n);

            n= MyStringExtend.WordCount(s2);

            //=================================
            char ch = s2.Chars(3);
            MessageBox.Show("ch = " + ch);

            //char ch = MyStringExtend.Chars(s2, 3);
            //MyString s1
        }
    }

}

public static class MyStringExtend
{
    public static int WordCount(this string s)
    {
        return s.Length;
    }

    public static char Chars(this string s, int index)
    {
        return s[index];
    }

}

//嚴重性 程式碼	說明	專案	檔案	行	隱藏項目狀態
//錯誤	CS0509	'MyString': 無法衍生自密封類型 'string'  LinqLabs C:\shared\LINQ\LinqLabs(Solution)\LinqLabs\2.FrmLangForLINQ.cs    449 作用中

//class MyString :string
//{

//}

public class MyPoint
{
    public MyPoint()
    {

    }

    public MyPoint(int p1)
    {
        P1 = p1;
    }

    public MyPoint(int p1, int p2)
    {
        this.P1 = p1;
        this.P2 = p2;
    }
    
    public MyPoint(string fielde1)
    {

    }
    private int m_p1;

    public string Field1="xxx", Field2="yyyy";

    public int P1
    {
        get
        {
            //logic ....
            return m_p1;
        }
        set
        {
          //logic ......
          m_p1 = value;
        }
    }

    public int P2 { get; set; }
}