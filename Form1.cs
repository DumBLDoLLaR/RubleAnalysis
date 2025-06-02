using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VVP_Table_App;

namespace RubleAnalysis
{
    public enum DataMode { Salary, VVP }

    public partial class Form1 : Form
    {
        private zarplata_TABLE salaryData;
        private VVP_TABLE vvpData;
        private DataMode currentMode;

        public Form1()
        {
            InitializeComponent();

            salaryData = new zarplata_TABLE();
            vvpData = new VVP_TABLE();
            currentMode = DataMode.Salary;

            comboBox1.Items.AddRange(new string[] { "Медианная зарплата", "ВВП и ВНД" });
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            buttonUpdate.Click += buttonUpdate_Click;
            buttonChart.Click += buttonChart_Click;
            buttonForecast.Click += buttonForecast_Click;
            buttonStats.Click += buttonStats_Click;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMode = comboBox1.SelectedIndex == 0 ? DataMode.Salary : DataMode.VVP;
            if (currentMode == DataMode.Salary)
                salaryData.DisplayInDataGridView(dataGridView1);
            else
                vvpData.DisplayInDataGridView(dataGridView1);
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (currentMode == DataMode.Salary)
                salaryData.DisplayInDataGridView(dataGridView1);
            else
                vvpData.DisplayInDataGridView(dataGridView1);
        }

        private void buttonChart_Click(object sender, EventArgs e)
        {
            if (currentMode == DataMode.Salary)
                ShowSalaryChart();
            else
                ShowVVPChart();
        }

        private void buttonForecast_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxWindow.Text, out int windowSize) || windowSize < 2)
            {
                MessageBox.Show("Введите корректное значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (currentMode == DataMode.Salary)
                {
                    var forecast = new zarplata_random(salaryData.GetDataTable(), windowSize);
                    forecast.ForecastNextYear();
                    salaryData.DisplayInDataGridView(dataGridView1);
                }
                else
                {
                    var forecast = new VVP_Extra(vvpData.GetDataTable(), windowSize);
                    forecast.Forecast();
                    vvpData.DisplayInDataGridView(dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка прогноза: " + ex.Message);
            }
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            if (currentMode == DataMode.Salary)
            {
                var stats = salaryData.GetGrowthData();
                if (stats.Count == 0)
                {
                    MessageBox.Show("Нет данных роста.");
                    return;
                }

                var max = stats.OrderByDescending(k => k.Value).First();
                var min = stats.OrderBy(k => k.Value).First();
                MessageBox.Show($"Максимальный рост: {max.Value:F2}% ({max.Key} г.)\nМинимальный рост: {min.Value:F2}% ({min.Key} г.)");
            }
            else
            {
                var (max, min) = vvpData.GetGrowthStats();
                MessageBox.Show($"Макс: {max:0.0}%\nМин: {min:0.0}%");
            }
        }
        

        private void ShowSalaryChart()
        {
            Form chartForm = new Form { Text = "График зарплат", Width = 800, Height = 600 };
            Chart chart = new Chart { Dock = DockStyle.Fill };
            chart.ChartAreas.Add(new ChartArea("Area")
            {
                AxisX = { Title = "Год" },
                AxisY = { Title = "Зарплата (руб.)" }
            });

            Series actual = new Series("Факт") { ChartType = SeriesChartType.Line, Color = Color.Blue, BorderWidth = 3 };
            Series forecast = new Series("Прогноз") { ChartType = SeriesChartType.Line, Color = Color.Red, BorderWidth = 3 };

            var rows = dataGridView1.Rows;
            var data = rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["Год"].Value != null)
                .ToDictionary(
                    r => Convert.ToInt32(r.Cells["Год"].Value),
                    r => Convert.ToInt32(r.Cells["Медианная зарплата (руб.)"].Value));

            var sortedYears = data.Keys.OrderBy(y => y);
            int lastActualYear = 2025;
            bool startedForecast = false;

            foreach (var year in sortedYears)
            {
                int salary = data[year];
                if (year <= lastActualYear)
                    actual.Points.AddXY(year, salary);
                else
                {
                    if (!startedForecast)
                    {
                        forecast.Points.AddXY(lastActualYear, data[lastActualYear]);
                        startedForecast = true;
                    }
                    forecast.Points.AddXY(year, salary);
                }
            }

            chart.Series.Add(actual);
            chart.Series.Add(forecast);
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }

        private void ShowVVPChart()
        {
            var chartForm = new Form { Text = "График ВВП и ВНД", Width = 900, Height = 600 };
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };
            var area = new ChartArea("Area") { AxisX = { Title = "Год" }, AxisY = { Title = "млрд $" } };
            chart.ChartAreas.Add(area);

            var vvpSeries = new Series("ВВП (млрд $)") { ChartType = SeriesChartType.Line, Color = Color.Blue, BorderWidth = 3 };
            var vndSeries = new Series("ВНД (млрд $)") { ChartType = SeriesChartType.Line, Color = Color.Green, BorderWidth = 3 };

            DataTable data = vvpData.GetDataTable();
            foreach (DataRow row in data.Rows)
            {
                int year = Convert.ToInt32(row["Год"]);
                int vvp = Convert.ToInt32(row["ВВП (млрд $)"].ToString().Replace(",", ""));
                int vnd = Convert.ToInt32(row["ВНД (млрд $)"].ToString().Replace(",", ""));

                var pointVvp = vvpSeries.Points.AddXY(year.ToString(), vvp);
                var pointVnd = vndSeries.Points.AddXY(year.ToString(), vnd);

                if (year > 2025)
                {
                    vvpSeries.Points[pointVvp].Color = Color.Red;
                    vndSeries.Points[pointVnd].Color = Color.DarkRed;
                }
            }

            chart.Series.Add(vvpSeries);
            chart.Series.Add(vndSeries);
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }
    }
}