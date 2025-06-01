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
        }
        private void button1_Click(object sender, EventArgs e)
        {
            vvpTable.DisplayInDataGridView(dataGridView1);
        }
    }
}
