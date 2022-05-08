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
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
            productPhotoTableAdapter1.Fill(awDataSet1.ProductPhoto);
            YearComboBox();
        }

        private void YearComboBox()
        {
            var query = from q in this.awDataSet1.ProductPhoto select q.ModifiedDate.Year;
            comboBox3.DataSource = query.Distinct().ToList();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in awDataSet1.ProductPhoto
                    select p;
            dataGridView1.DataSource = q.ToList();
            lblMaster.Text = " 總共:" + q.Count();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var q = from a in awDataSet1.ProductPhoto
                    where a.ModifiedDate >= dateTimePicker1.Value && a.ModifiedDate <= dateTimePicker2.Value
                    select a;
            dataGridView1.DataSource = q.ToList();
            lblMaster.Text = " 總共:" + q.Count();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var q = from a in awDataSet1.ProductPhoto
                    where a.ModifiedDate.Year == (int)comboBox3.SelectedItem
                    select a;
            dataGridView1.DataSource = q.ToList();
            lblMaster.Text += " 總共:" + q.Count();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int min = 3 * comboBox2.SelectedIndex;
            int max = 3 * (comboBox2.SelectedIndex + 1) + 1;
            var q = from a in awDataSet1.ProductPhoto
                    where a.ModifiedDate.Year == (int)comboBox3.SelectedItem
                    select a;
            if (comboBox2.Text != "All")
            {
                q = q.Where(m => m.ModifiedDate.Month > min && m.ModifiedDate.Month < max);
            }
            lblMaster.Text += " 總共:" + q.Count();
            dataGridView1.DataSource = q.ToList();
        }
    }
}
