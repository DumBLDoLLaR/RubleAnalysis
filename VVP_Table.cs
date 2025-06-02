using System;
using System.Data;
using System.Windows.Forms;

namespace VVP_Table_App
{
    public class VVP_TABLE
    {
        private DataTable table;

        public VVP_TABLE()
        {
            InitializeTable();
        }

        private void InitializeTable()
        {
            table = new DataTable("VVP_Data");

            // Создаем колонки с правильными типами данных
            table.Columns.Add("Год", typeof(int));
            table.Columns.Add("ВВП (млрд ₽)", typeof(int));
            table.Columns.Add("ВВП (млрд $)", typeof(int));
            table.Columns.Add("Рост ВВП (%)", typeof(double));
            table.Columns.Add("ВНД (млрд $)", typeof(int));
            table.Columns.Add("Рост ВНД (%)", typeof(double));

            // Добавляем данные с расчетными значениями роста
            table.Rows.Add(2010, 46309, 1524, 0.0, 1528, 0.0); // Нет данных для расчета роста для первого года
            table.Rows.Add(2011, 55967, 1900, 20.86, 1907, 24.80);
            table.Rows.Add(2012, 62218, 2015, 11.17, 2022, 6.03);
            table.Rows.Add(2013, 66755, 2097, 7.29, 2104, 4.05);
            table.Rows.Add(2014, 79200, 1861, 18.64, 1860, -11.60);
            table.Rows.Add(2015, 83233, 1366, 5.09, 1350, -27.42);
            table.Rows.Add(2016, 86044, 1280, 3.38, 1280, -5.19);
            table.Rows.Add(2017, 92037, 1570, 6.97, 1570, 22.66);
            table.Rows.Add(2018, 103626, 1660, 12.59, 1660, 5.73);
            table.Rows.Add(2019, 109362, 1690, 5.53, 1690, 1.81);
            table.Rows.Add(2020, 106967, 1480, -2.19, 1480, -12.43);
            table.Rows.Add(2021, 131015, 1780, 22.48, 1780, 20.27);
            table.Rows.Add(2022, 151455, 1990, 15.60, 1990, 11.80);
            table.Rows.Add(2023, 171041, 2050, 12.93, 2050, 3.02);
            table.Rows.Add(2024, 183000, 2100, 6.99, 2100, 2.44);
            table.Rows.Add(2025, 195000, 2150, 6.56, 2150, 2.38);
        }

        public void DisplayInDataGridView(DataGridView dataGridView)
        {
            if (dataGridView != null)
            {
                dataGridView.DataSource = table;

                // Настраиваем внешний вид DataGridView
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Центрируем столбцы с процентами
                dataGridView.Columns["Рост ВВП (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns["Рост ВНД (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
        public DataTable GetDataTable()
        {
            return table;
        }
        public DataTable GetDisplayTable()
        {
            DataTable displayTable = table.Clone();
            displayTable.Columns["Рост ВВП (%)"].DataType = typeof(string);
            displayTable.Columns["Рост ВНД (%)"].DataType = typeof(string);

            foreach (DataRow row in table.Rows)
            {
                var newRow = displayTable.NewRow();
                newRow["Год"] = row["Год"];
                newRow["ВВП (млрд ₽)"] = row["ВВП (млрд ₽)"];
                newRow["ВВП (млрд $)"] = ((int)row["ВВП (млрд $)"]).ToString("N0");
                newRow["Рост ВВП (%)"] = ((double)row["Рост ВВП (%)"]).ToString("+0.0;-0.0;0.0") + "%";
                newRow["ВНД (млрд $)"] = ((int)row["ВНД (млрд $)"]).ToString("N0");
                newRow["Рост ВНД (%)"] = ((double)row["Рост ВНД (%)"]).ToString("+0.0;-0.0;0.0") + "%";

                displayTable.Rows.Add(newRow);
            }

            return displayTable;
        }
        public (double max, double min) GetGrowthStats()
        {
            var growthValues = table.AsEnumerable()
                .Select(row => row.Field<double>("Рост ВВП (%)"))
                .ToList();

            return (growthValues.Max(), growthValues.Min());
        }
    }
}