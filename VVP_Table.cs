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
            table.Columns.Add("Год", typeof(string));
            table.Columns.Add("ВВП (млрд ₽)", typeof(string));
            table.Columns.Add("ВВП (млрд $)", typeof(int));
            table.Columns.Add("Рост ВВП (%)", typeof(double));
            table.Columns.Add("ВНД (млрд $)", typeof(int));
            table.Columns.Add("Рост ВНД (%)", typeof(double));

            // Добавляем данные (целые числа для млрд $)
            table.Rows.Add("2010", "46 309", 1524, 4.5, 1528, 4.5);
            table.Rows.Add("2011", "55 967", 1900, 4.3, 1907, 5.1);
            table.Rows.Add("2012", "62 218", 2015, 3.7, 2022, 3.7);
            table.Rows.Add("2013", "66 755", 2097, 1.8, 2104, 1.8);
            table.Rows.Add("2014", "79 200", 1861, 0.7, 1860, 0.6);
            table.Rows.Add("2015", "83 233", 1366, -2.0, 1350, -2.8);
            table.Rows.Add("2016", "86 044", 1280, 0.2, 1280, 0.2);
            table.Rows.Add("2017", "92 037", 1570, 1.8, 1570, 1.8);
            table.Rows.Add("2018", "103 626", 1660, 2.5, 1660, 2.5);
            table.Rows.Add("2019", "109 362", 1690, 2.0, 1690, 2.0);
            table.Rows.Add("2020", "106 967", 1480, -3.0, 1480, -3.0);
            table.Rows.Add("2021", "131 015", 1780, 5.6, 1780, 5.6);
            table.Rows.Add("2022", "151 455", 1990, -2.1, 1990, -2.1);
            table.Rows.Add("2023*", "171 041", 2050, 2.2, 2050, 2.2);
            table.Rows.Add("2024*", "~183 000", 2100, 1.5, 2100, 1.5);
            table.Rows.Add("2025*", "~195 000", 2150, 1.3, 2150, 1.3);
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