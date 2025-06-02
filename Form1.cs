using System.Data;
using VVP_Table_App;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
            button3.Click += button3_Click;
            button5.Click += button5_Click;
            button7.Click += button7_Click;

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

            // Выводим результаты в TextBox'ы
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
                    MessageBox.Show("Пожалуйста, введите корректное количество периодов для скользящей средней (не менее 2).",
                                 "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"Прогноз на следующий год рассчитан с использованием {periods} периодов.",
                               "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
