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

            // ������� ���������� � TextBox'�
            textBox1.Text = $"+{max:0.0}%";
            textBox3.Text = $"{min:0.0}%"; // ��� ������������� �������� ���� ����� �������������
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // �������� ���������� �������� ��� ���������� �������
                if (!int.TryParse(textBox5.Text, out int periods) || periods < 2)
                {
                    MessageBox.Show("����������, ������� ���������� ���������� �������� ��� ���������� ������� (�� ����� 2).",
                                 "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // �������� DataTable �� vvpTable
                DataTable table = vvpTable.GetDataTable();

                // ������� ��������� ������ ��� ���������������
                VVP_Extra forecaster = new VVP_Extra(table, periods);

                // ��������� ��������������� �� 1 ���
                forecaster.Forecast();

                // ��������� �������� ������ � DataGridView
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;

                MessageBox.Show($"������� �� ��������� ��� ��������� � �������������� {periods} ��������.",
                               "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ������� ��������: {ex.Message}",
                               "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowChart()
        {
            var chartForm = new Form
            {
                Text = "�������� ��� � ��� (���� $)",
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
                AxisX = { Title = "���", Interval = 1 },
                AxisY = { Title = "���� $" }
            };
            chart.ChartAreas.Add(chartArea);

            DataTable displayTable = (DataTable)dataGridView1.DataSource;

            AddChartSeries(chart, "��� (���� $)", Color.Blue, SeriesChartType.Line);
            AddChartSeries(chart, "��� (���� $)", Color.Green, SeriesChartType.Line);

            foreach (DataRow row in displayTable.Rows)
            {
                int year = Convert.ToInt32(row["���"]);
                int vvp = Convert.ToInt32(row["��� (���� $)"].ToString().Replace(",", ""));
                int vnd = Convert.ToInt32(row["��� (���� $)"].ToString().Replace(",", ""));

                // ��������� ����� ��� ���
                int indexVvp = chart.Series["��� (���� $)"].Points.AddXY(year.ToString(), vvp);
                DataPoint pointVvp = chart.Series["��� (���� $)"].Points[indexVvp];

                // ��������� ����� ��� ���
                int indexVnd = chart.Series["��� (���� $)"].Points.AddXY(year.ToString(), vnd);
                DataPoint pointVnd = chart.Series["��� (���� $)"].Points[indexVnd];

                if (year > 2025)
                {
                    // ���� ����� � ����� ��� �������� ���
                    pointVvp.Color = Color.Red;
                    pointVvp.MarkerColor = Color.Red;

                    // ���� ����� � ����� ��� �������� ���
                    pointVnd.Color = Color.DarkRed;
                    pointVnd.MarkerColor = Color.DarkRed;
                }
            }

            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            });
            // �������� ������� ���������� ������
            bool hasForecast = displayTable.AsEnumerable().Any(r => Convert.ToInt32(r["���"]) > 2025);

            if (hasForecast)
            {
                // ��������� ��������� ����� ��� ������� �������� ���
                var forecastVvpLegend = new Series("������� ��� (���� $)")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Red,
                    BorderWidth = 3,
                    IsVisibleInLegend = true,
                    IsValueShownAsLabel = false
                };
                forecastVvpLegend.Points.AddXY("", 0); // �� ����������� �� �������
                forecastVvpLegend.Points[0].IsVisibleInLegend = true;
                forecastVvpLegend.Points[0].IsValueShownAsLabel = false;
                forecastVvpLegend.Points[0].MarkerStyle = MarkerStyle.Circle;
                forecastVvpLegend.Points[0].MarkerSize = 8;
                forecastVvpLegend.Points[0].Color = Color.Red;
                forecastVvpLegend.Points[0].MarkerColor = Color.Red;

                chart.Series.Add(forecastVvpLegend);

                // ������� ���
                var forecastVndLegend = new Series("������� ��� (���� $)")
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
