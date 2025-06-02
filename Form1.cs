using System.Windows.Forms;

namespace RubleAnalysis
{
    public partial class Form1 : Form
    {
        private zarplata_TABLE salaryData;

        public Form1()
        {
            InitializeComponent();
            salaryData = new zarplata_TABLE();
            button2.Click += button2_Click;
            button6.Click += button6_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            salaryData.DisplayInDataGridView(dataGridView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var (maxGrowth, minGrowth) = salaryData.GetGrowthStats();
            textBox2.Text = maxGrowth.ToString("0.0") + "%";
            textBox4.Text = minGrowth.ToString("0.0") + "%";
        }
    }
}