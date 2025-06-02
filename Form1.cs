<<<<<<< zarplata
using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using VVP_Table_App;
using System.Drawing;
namespace RubleAnalysis
{
    public partial class Form1 : Form
    {
<<<<<<< zarplata
        private zarplata_TABLE salaryData;
        private VVP_TABLE vvpTable;

        private void button8_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox6.Text, out int windowSize) || windowSize < 2)
            {
                MessageBox.Show("Ââåäèòå êîððåêòíîå ÷èñëî ëåò (íå ìåíåå 2) äëÿ ñêîëüçÿùåé ñðåäíåé.", "Îøèáêà", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(ex.Message, "Îøèáêà", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();
            salaryData = new zarplata_TABLE();
            button2.Click += button2_Click;
            button4.Click += button4_Click;
            button6.Click += button6_Click; // Êíîïêà ðàñ÷¸òà ìàêñèìóìà è ìèíèìóìà ðîñòà
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

            // Âûâîäèì ðåçóëüòàòû â TextBox'û
            textBox1.Text = $"+{max:0.0}%";
            textBox3.Text = $"{min:0.0}%"; // äëÿ îòðèöàòåëüíûõ çíà÷åíèé çíàê áóäåò àâòîìàòè÷åñêè
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Ïîëó÷àåì êîëè÷åñòâî ïåðèîäîâ äëÿ ñêîëüçÿùåé ñðåäíåé
                if (!int.TryParse(textBox5.Text, out int periods) || periods < 2)
                {
                    MessageBox.Show("Ïîæàëóéñòà, ââåäèòå êîððåêòíîå êîëè÷åñòâî ïåðèîäîâ äëÿ ñêîëüçÿùåé ñðåäíåé (íå ìåíåå 2).",
                                 "Îøèáêà", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ïîëó÷àåì DataTable èç vvpTable
                DataTable table = vvpTable.GetDataTable();

                // Ñîçäàåì ýêçåìïëÿð êëàññà äëÿ ïðîãíîçèðîâàíèÿ
                VVP_Extra forecaster = new VVP_Extra(table, periods);

                // Âûïîëíÿåì ïðîãíîçèðîâàíèå íà 1 ãîä
                forecaster.Forecast();

                // Îáíîâëÿåì ïðèâÿçêó äàííûõ â DataGridView
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;

                MessageBox.Show($"Ïðîãíîç íà ñëåäóþùèé ãîä ðàññ÷èòàí ñ èñïîëüçîâàíèåì {periods} ïåðèîäîâ.",
                               "Óñïåõ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Îøèáêà ïðè ðàñ÷åòå ïðîãíîçà: {ex.Message}",
                               "Îøèáêà", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowChart()
        {
            var chartForm = new Form
            {
                Text = "Äèíàìèêà ÂÂÏ è ÂÍÄ (ìëðä $)",
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
                AxisX = { Title = "Ãîä", Interval = 1 },
                AxisY = { Title = "ìëðä $" }
            };
            chart.ChartAreas.Add(chartArea);

            DataTable displayTable = (DataTable)dataGridView1.DataSource;

            AddChartSeries(chart, "ÂÂÏ (ìëðä $)", Color.Blue, SeriesChartType.Line);
            AddChartSeries(chart, "ÂÍÄ (ìëðä $)", Color.Green, SeriesChartType.Line);

            foreach (DataRow row in displayTable.Rows)
            {
                int year = Convert.ToInt32(row["Ãîä"]);
                int vvp = Convert.ToInt32(row["ÂÂÏ (ìëðä $)"].ToString().Replace(",", ""));
                int vnd = Convert.ToInt32(row["ÂÍÄ (ìëðä $)"].ToString().Replace(",", ""));

                // Äîáàâëÿåì òî÷êó äëÿ ÂÂÏ
                int indexVvp = chart.Series["ÂÂÏ (ìëðä $)"].Points.AddXY(year.ToString(), vvp);
                DataPoint pointVvp = chart.Series["ÂÂÏ (ìëðä $)"].Points[indexVvp];

                // Äîáàâëÿåì òî÷êó äëÿ ÂÍÄ
                int indexVnd = chart.Series["ÂÍÄ (ìëðä $)"].Points.AddXY(year.ToString(), vnd);
                DataPoint pointVnd = chart.Series["ÂÍÄ (ìëðä $)"].Points[indexVnd];

                if (year > 2025)
                {
                    // Öâåò ëèíèè è òî÷êè äëÿ ïðîãíîçà ÂÂÏ
                    pointVvp.Color = Color.Red;
                    pointVvp.MarkerColor = Color.Red;

                    // Öâåò ëèíèè è òî÷êè äëÿ ïðîãíîçà ÂÍÄ
                    pointVnd.Color = Color.DarkRed;
                    pointVnd.MarkerColor = Color.DarkRed;
                }
            }

            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            });
            // Ïðîâåðèì íàëè÷èå ïðîãíîçíûõ äàííûõ
            bool hasForecast = displayTable.AsEnumerable().Any(r => Convert.ToInt32(r["Ãîä"]) > 2025);

            if (hasForecast)
            {
                // Äîáàâëÿåì ôèêòèâíóþ òî÷êó äëÿ ëåãåíäû ïðîãíîçà ÂÂÏ
                var forecastVvpLegend = new Series("Ïðîãíîç ÂÂÏ (ìëðä $)")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Red,
                    BorderWidth = 3,
                    IsVisibleInLegend = true,
                    IsValueShownAsLabel = false
                };
                forecastVvpLegend.Points.AddXY("", 0); // íå îòîáðàçèòñÿ íà ãðàôèêå
                forecastVvpLegend.Points[0].IsVisibleInLegend = true;
                forecastVvpLegend.Points[0].IsValueShownAsLabel = false;
                forecastVvpLegend.Points[0].MarkerStyle = MarkerStyle.Circle;
                forecastVvpLegend.Points[0].MarkerSize = 8;
                forecastVvpLegend.Points[0].Color = Color.Red;
                forecastVvpLegend.Points[0].MarkerColor = Color.Red;

                chart.Series.Add(forecastVvpLegend);

                // Ïðîãíîç ÂÍÄ
                var forecastVndLegend = new Series("Ïðîãíîç ÂÍÄ (ìëðä $)")
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
            chartForm.Text = "Ãðàôèê èçìåíåíèÿ çàðïëàò";
            chartForm.Width = 800;
            chartForm.Height = 600;

            Chart chart = new Chart { Dock = DockStyle.Fill };
            ChartArea chartArea = new ChartArea("SalaryChartArea");
            chartArea.AxisX.Title = "Ãîä";
            chartArea.AxisY.Title = "Çàðïëàòà (ðóá.)";
            chart.ChartAreas.Add(chartArea);

            Series actualSeries = new Series("Ôàêòè÷åñêèå äàííûå")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Blue,
                BorderWidth = 3
            };

            Series forecastSeries = new Series("Ïðîãíîç")
            {
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Red,
                BorderWidth = 3
            };

            // Óñòàíîâèì ïîñëåäíþþ òî÷êó ôàêòè÷åñêèõ äàííûõ  2025
            int lastActualYear = 2025;

            var dgvData = new Dictionary<int, int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Ãîä"].Value == null || row.Cells["Ìåäèàííàÿ çàðïëàòà (ðóá.)"].Value == null)
                    continue;

                if (int.TryParse(row.Cells["Ãîä"].Value.ToString(), out int year) &&
                    int.TryParse(row.Cells["Ìåäèàííàÿ çàðïëàòà (ðóá.)"].Value.ToString(), out int salary))
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
        private void CalculateAndShowMaxMinGrowth()
        {
            var growthData = salaryData.GetGrowthData();

            if (growthData.Count == 0)
            {
                MessageBox.Show("Äàííûå î ðîñòå îòñóòñòâóþò.", "Èíôîðìàöèÿ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Íàõîäèì ìàêñèìàëüíûé è ìèíèìàëüíûé ðîñò
            var maxGrowth = growthData.OrderByDescending(kvp => kvp.Value).First();
            var minGrowth = growthData.OrderBy(kvp => kvp.Value).First();

            string message = $"Ìàêñèìàëüíûé ðîñò çàðïëàòû: {maxGrowth.Value:F2}% â {maxGrowth.Key} ãîäó.\n" +
                             $"Ìèíèìàëüíûé ðîñò çàðïëàòû: {minGrowth.Value:F2}% â {minGrowth.Key} ãîäó.";

            MessageBox.Show(message, "Ðåçóëüòàòû àíàëèçà", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
