
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Добавьте эту директиву

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
            button4.Click += button4_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            salaryData.DisplayInDataGridView(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowSalaryChart();
        }

        private void ShowSalaryChart()
        {
            Form chartForm = new Form();
            chartForm.Text = "График изменения зарплат";
            chartForm.Width = 800;
            chartForm.Height = 600;

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill // Устанавливаем заполнение всей формы
            };

            ChartArea chartArea = new ChartArea
            {
                Name = "SalaryChartArea",
                AxisX = { Title = "Год" },
                AxisY = { Title = "Зарплата (руб.)" }
            };
            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Медианная зарплата",
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Blue
            };

            var salaryData = this.salaryData.GetSalaryDataForChart();
            foreach (var item in salaryData)
            {
                series.Points.AddXY(item.Key, item.Value);
            }

            chart.Series.Add(series);
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }
    }
}