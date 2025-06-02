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

            Chart chart = new Chart { Dock = DockStyle.Fill };
            ChartArea chartArea = new ChartArea("SalaryChartArea");
            chartArea.AxisX.Title = "���";
            chartArea.AxisY.Title = "�������� (���.)";
            chart.ChartAreas.Add(chartArea);

            Series actualSeries = new Series("����������� ������")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Blue,
                BorderWidth = 3
            };

            Series forecastSeries = new Series("�������")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Red,
                BorderWidth = 3
            };

            // ��������� ��������� ����� ����������� ������ � 2025
            int lastActualYear = 2025;

            var dgvData = new Dictionary<int, int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["���"].Value == null || row.Cells["��������� �������� (���.)"].Value == null)
                    continue;

                if (int.TryParse(row.Cells["���"].Value.ToString(), out int year) &&
                    int.TryParse(row.Cells["��������� �������� (���.)"].Value.ToString(), out int salary))
                {
                    dgvData[year] = salary;
                }
            }

            var sortedYears = dgvData.Keys.OrderBy(y => y).ToList();
            bool forecastStarted = false;

            foreach (var year in sortedYears)
            {
                int salary = dgvData[year];

                if (year <= lastActualYear)
                {
                    actualSeries.Points.AddXY(year, salary);
                }
                else
                {
                    if (!forecastStarted)
                    {
                        forecastSeries.Points.AddXY(lastActualYear, dgvData[lastActualYear]); // ������� �������
                        forecastStarted = true;
                    }
                    forecastSeries.Points.AddXY(year, salary);
                }
            }

            chart.Series.Add(actualSeries);
            chart.Series.Add(forecastSeries);
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
