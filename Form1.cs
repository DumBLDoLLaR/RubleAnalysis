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
            button3.Click += Button3_Click;
            button5.Click += button5_Click;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            vvpTable.DisplayInDataGridView(dataGridView1);
        }
        private void Button3_Click(object sender, EventArgs e)
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

            // ��������� ������� �������
            var chartArea = new ChartArea("MainArea")
            {
                AxisX = { Title = "���", Interval = 1 },
                AxisY = { Title = "���� $" }
            };
            chart.ChartAreas.Add(chartArea);

            // ���������� ����� ������ (������ ��� � ���)
            AddChartSeries(chart, "��� (���� $)", Color.Blue, SeriesChartType.Line);
            AddChartSeries(chart, "��� (���� $)", Color.Green, SeriesChartType.Line);

            // ���������� �������
            FillChartData(chart);

            // ��������� �������
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            });

            chartForm.Controls.Add(chart);
            chartForm.ShowDialog();
        }

        private void AddChartSeries(Chart chart, string name, Color color, SeriesChartType type)
        {
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

        private void FillChartData(Chart chart)
        {
            var table = vvpTable.GetDataTable();

            foreach (DataRow row in table.Rows)
            {
                string year = row["���"].ToString();
                int vvp = Convert.ToInt32(row["��� (���� $)"]);
                int vnd = Convert.ToInt32(row["��� (���� $)"]);

                chart.Series["��� (���� $)"].Points.AddXY(year, vvp);
                chart.Series["��� (���� $)"].Points.AddXY(year, vnd);
            }
        }
    }
}
