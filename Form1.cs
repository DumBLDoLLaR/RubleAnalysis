using VVP_Table_App;

namespace RubleAnalysis
{
    public partial class Form1 : Form
    {
        private VVP_TABLE vvpTable;
        public Form1()
        {
            InitializeComponent();
            vvpTable = new VVP_TABLE();
            button1.Click += button1_Click;
            button5.Click += button5_Click;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            vvpTable.DisplayInDataGridView(dataGridView1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var (max, min) = vvpTable.GetGrowthStats();

            // ¬ыводим результаты в TextBox'ы
            textBox1.Text = $"+{max:0.0}%";
            textBox3.Text = $"{min:0.0}%"; // дл€ отрицательных значений знак будет автоматически
        }
    }
}
