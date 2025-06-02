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
                MessageBox.Show("Введите корректное число лет (не менее 2) для скользящей средней.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();
            salaryData = new zarplata_TABLE();
            button2.Click += button2_Click;
            button4.Click += button4_Click;
            button6.Click += button6_Click; // Кнопка расчёта максимума и минимума роста
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
            chartForm.Text = "График изменения зарплат";
            chartForm.Width = 800;
            chartForm.Height = 600;

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            ChartArea chartArea = new ChartArea("SalaryChartArea");
            chartArea.AxisX.Title = "Год";
            chartArea.AxisY.Title = "Зарплата (руб.)";
            chart.ChartAreas.Add(chartArea);

            Series series = new Series("Медианная зарплата")
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
                MessageBox.Show("Данные о росте отсутствуют.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Находим максимальный и минимальный рост
            var maxGrowth = growthData.OrderByDescending(kvp => kvp.Value).First();
            var minGrowth = growthData.OrderBy(kvp => kvp.Value).First();

            string message = $"Максимальный рост зарплаты: {maxGrowth.Value:F2}% в {maxGrowth.Key} году.\n" +
                             $"Минимальный рост зарплаты: {minGrowth.Value:F2}% в {minGrowth.Key} году.";

            MessageBox.Show(message, "Результаты анализа", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
