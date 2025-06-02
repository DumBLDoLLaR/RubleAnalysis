using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using VVP_Table_App;
using System.Drawing;
using zarplata_Table_App;


namespace RubleAnalysis
{
    public enum DataMode { Salary, VVP }
    public partial class Form1 : Form
    {
        private zarplata_TABLE salaryData;
        private VVP_TABLE vvpTable;

        private VVP_TABLE vvpData;
        private DataMode currentMode;

        public Form1()
        {
            InitializeComponent();
            salaryData = new zarplata_TABLE();
            button2.Click += button2_Click;
            button4.Click += button4_Click;
            button8.Click += button8_Click;
            
            vvpTable = new VVP_TABLE();
            button1.Click += button1_Click;
            button3.Click += button3_Click;
            button5.Click += button5_Click;
            button7.Click += button7_Click;
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
        private void button8_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox6.Text, out int windowSize) || windowSize < 2)
            {
                MessageBox.Show("Количество дней для экстраполяции должно быть больше 2", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void button1_Click(object sender, EventArgs e)
        {
            vvpTable.DisplayInDataGridView(dataGridView1);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ShowChart();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            var (max, min) = vvpTable.GetGrowthStats();

            // Отображение экстремумов 
            textBox1.Text = $"+{max:0.0}%";
            textBox3.Text = $"{min:0.0}%"; // для отрицательных значений знак будет автоматически
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем количество периодов для скользящей средней
                if (!int.TryParse(textBox5.Text, out int periods) || periods < 2)
                {
                    MessageBox.Show("Количество дней для экстраполяции должно быть больше 2", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Получаем DataTable из vvpTable
                DataTable table = vvpTable.GetDataTable();

                // Создаем экземпляр класса для прогнозирования
                VVP_Extra forecaster = new VVP_Extra(table, periods);

                // Выполняем прогнозирование на 1 год
                forecaster.Forecast();

                // Обновляем привязку данных в DataGridView
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете прогноза: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowChart()
        {
            var chartForm = new Form
            {
                Text = "Динамика ВВП и ВНД (млрд $)",
                Width = 900,
                Height = 600,
                StartPosition = FormStartPosition.CenterParent
            };

            var chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            var chartArea = new ChartArea("MainArea")
            {
                AxisX = { Title = "Год", Interval = 1 },
                AxisY = { Title = "млрд $" }
            };
            chart.ChartAreas.Add(chartArea);

            DataTable displayTable = (DataTable)dataGridView1.DataSource;

            AddChartSeries(chart, "ВВП (млрд $)", Color.Blue, SeriesChartType.Line);
            AddChartSeries(chart, "ВНД (млрд $)", Color.Green, SeriesChartType.Line);

            foreach (DataRow row in displayTable.Rows)
            {
                int year = Convert.ToInt32(row["Год"]);
                int vvp = Convert.ToInt32(row["ВВП (млрд $)"].ToString().Replace(",", ""));
                int vnd = Convert.ToInt32(row["ВНД (млрд $)"].ToString().Replace(",", ""));

                // Добавляем точку для ВВП
                int indexVvp = chart.Series["ВВП (млрд $)"].Points.AddXY(year.ToString(), vvp);
                DataPoint pointVvp = chart.Series["ВВП (млрд $)"].Points[indexVvp];

                // Добавляем точку для ВНД
                int indexVnd = chart.Series["ВНД (млрд $)"].Points.AddXY(year.ToString(), vnd);
                DataPoint pointVnd = chart.Series["ВНД (млрд $)"].Points[indexVnd];

                if (year > 2025)
                {
                    // Цвет линии и точки для прогноза ВВП
                    pointVvp.Color = Color.Red;
                    pointVvp.MarkerColor = Color.Red;

                    // Цвет линии и точки для прогноза ВНД
                    pointVnd.Color = Color.DarkRed;
                    pointVnd.MarkerColor = Color.DarkRed;
                }
            }

            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            });
            // Проверим наличие прогнозных данных
            bool hasForecast = displayTable.AsEnumerable().Any(r => Convert.ToInt32(r["Год"]) > 2025);

            if (hasForecast)
            {
                // Добавляем фиктивную точку для легенды прогноза ВВП
                var forecastVvpLegend = new Series("Прогноз ВВП (млрд $)")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Red,
                    BorderWidth = 3,
                    IsVisibleInLegend = true,
                    IsValueShownAsLabel = false
                };
                forecastVvpLegend.Points.AddXY("", 0); // не отобразится на графике
                forecastVvpLegend.Points[0].IsVisibleInLegend = true;
                forecastVvpLegend.Points[0].IsValueShownAsLabel = false;
                forecastVvpLegend.Points[0].MarkerStyle = MarkerStyle.Circle;
                forecastVvpLegend.Points[0].MarkerSize = 8;
                forecastVvpLegend.Points[0].Color = Color.Red;
                forecastVvpLegend.Points[0].MarkerColor = Color.Red;

                chart.Series.Add(forecastVvpLegend);

                // Прогноз ВНД
                var forecastVndLegend = new Series("Прогноз ВНД (млрд $)")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.DarkRed,
                    BorderWidth = 3,
                    IsVisibleInLegend = true,
                    IsValueShownAsLabel = false
                };
                forecastVndLegend.Points.AddXY("", 0);
                forecastVndLegend.Points[0].IsVisibleInLegend = true;
                forecastVndLegend.Points[0].IsValueShownAsLabel = false;
                forecastVndLegend.Points[0].MarkerStyle = MarkerStyle.Circle;
                forecastVndLegend.Points[0].MarkerSize = 8;
                forecastVndLegend.Points[0].Color = Color.DarkRed;
                forecastVndLegend.Points[0].MarkerColor = Color.DarkRed;

                chart.Series.Add(forecastVndLegend);
            }
            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }
        private void ShowSalaryChart()
        {
            Form chartForm = new Form();
            chartForm.Text = "График колебания зарплат в разные годы";
            chartForm.Width = 800;
            chartForm.Height = 600;

            Chart chart = new Chart { Dock = DockStyle.Fill };
            ChartArea chartArea = new ChartArea("SalaryChartArea");
            chartArea.AxisX.Title = "Год";
            chartArea.AxisY.Title = "Зарплата (руб.)";
            chart.ChartAreas.Add(chartArea);

            Series actualSeries = new Series("Факт")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Blue,
                BorderWidth = 3
            };

            Series forecastSeries = new Series("Прогноз")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Red,
                BorderWidth = 3
            };

            // Максимально допустимый год без экстраполяции - 2025
            int lastActualYear = 2025;

            var dgvData = new Dictionary<int, int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Год"].Value == null || row.Cells["Медианная зарплата (руб.)"].Value == null)
                    continue;

                if (int.TryParse(row.Cells["Год"].Value.ToString(), out int year) &&
                    int.TryParse(row.Cells["Медианная зарплата (руб.)"].Value.ToString(), out int salary))
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
                        forecastSeries.Points.AddXY(lastActualYear, dgvData[lastActualYear]); // ïëàâíûé ïåðåõîä
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
        private void AddChartSeries(Chart chart, string name, Color color, SeriesChartType type)
        {
            if (chart.Series.Any(s => s.Name == name)) return;

            var series = new Series(name)
            {
                ChartType = type,
                Color = color,
                BorderWidth = 3,
                XValueType = ChartValueType.String,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = color
            };

            chart.Series.Add(series);
        }
        private void CalculateAndShowMaxMinGrowth()
        {
            var growthData = salaryData.GetGrowthData();

            if (growthData.Count == 0)
            {
                MessageBox.Show("Данные в росте отсутствуют.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Находим максимальный и минимальный рост
            var maxGrowth = growthData.OrderByDescending(kvp => kvp.Value).First();
            var minGrowth = growthData.OrderBy(kvp => kvp.Value).First();

            string message = $"Максимальный рост зарплаты: {maxGrowth.Value:F2}% в {maxGrowth.Key} году.\n" +
                             $"Минималтный рост зарплаты: {minGrowth.Value:F2}% в {minGrowth.Key} году.";

            MessageBox.Show(message, "Результаты анализа", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
