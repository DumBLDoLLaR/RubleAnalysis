using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace RubleAnalysis
{
    public partial class Form1 : Form
    {
        private zarplata_TABLE salaryData;

        private void button8_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox6.Text, out int windowSize) || windowSize < 2)
            {
                MessageBox.Show("������� ���������� ����� ��� (�� ����� 2) ��� ���������� �������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var forecast = new zarplata_random(salaryData.GetDataTable(), windowSize);
            try
            {
                forecast.ForecastNextYear();
                salaryData.DisplayInDataGridView(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();
            salaryData = new zarplata_TABLE();
            button2.Click += button2_Click;
            button4.Click += button4_Click;
            button6.Click += button6_Click; // ������ ������� ��������� � �������� �����
            button8.Click += button8_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            salaryData.DisplayInDataGridView(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowSalaryChart();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CalculateAndShowMaxMinGrowth();
        }

        private void ShowSalaryChart()
        {
            Form chartForm = new Form();
            chartForm.Text = "������ ��������� �������";
            chartForm.Width = 800;
            chartForm.Height = 600;

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            ChartArea chartArea = new ChartArea("SalaryChartArea");
            chartArea.AxisX.Title = "���";
            chartArea.AxisY.Title = "�������� (���.)";
            chart.ChartAreas.Add(chartArea);

            Series series = new Series("��������� ��������")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Blue
            };

            var salaryDict = this.salaryData.GetSalaryDataForChart();
            foreach (var item in salaryDict.OrderBy(kvp => kvp.Key))
            {
                series.Points.AddXY(item.Key, item.Value);
            }

            chart.Series.Add(series);
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }

        private void CalculateAndShowMaxMinGrowth()
        {
            var growthData = salaryData.GetGrowthData();

            if (growthData.Count == 0)
            {
                MessageBox.Show("������ � ����� �����������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ������� ������������ � ����������� ����
            var maxGrowth = growthData.OrderByDescending(kvp => kvp.Value).First();
            var minGrowth = growthData.OrderBy(kvp => kvp.Value).First();

            string message = $"������������ ���� ��������: {maxGrowth.Value:F2}% � {maxGrowth.Key} ����.\n" +
                             $"����������� ���� ��������: {minGrowth.Value:F2}% � {minGrowth.Key} ����.";

            MessageBox.Show(message, "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
