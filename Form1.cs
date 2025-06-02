using System.Windows.Forms;

namespace RubleAnalysis;



public partial class Form1 : Form
{
    private zarplata_TABLE salaryData;

    public Form1()
    {
        InitializeComponent();
        salaryData = new zarplata_TABLE();
        button2.Click += button2_Click;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        salaryData.DisplayInDataGridView(dataGridView1);
    }
}