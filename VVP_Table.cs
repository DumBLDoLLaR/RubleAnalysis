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

            // Создаем колонки
            table.Columns.Add("Год", typeof(string));
            table.Columns.Add("ВВП (млрд ₽)", typeof(string));
            table.Columns.Add("ВВП (млрд $)", typeof(string));
            table.Columns.Add("Рост ВВП (%)", typeof(string));
            table.Columns.Add("ВНД (млрд $)", typeof(string));
            table.Columns.Add("Рост ВНД (%)", typeof(string));

            // Добавляем данные
            table.Rows.Add("2010", "46 309", "1 524", "+4,5%", "1 528", "+4,5%");
            table.Rows.Add("2011", "55 967", "1 900", "+4,3%", "1 907", "+5,1%");
            table.Rows.Add("2012", "62 218", "2 015", "+3,7%", "2 022", "+3,7%");
            table.Rows.Add("2013", "66 755", "2 097", "+1,8%", "2 104", "+1,8%");
            table.Rows.Add("2014", "79 200", "1 861", "+0,7%", "1 860", "+0,6%");
            table.Rows.Add("2015", "83 233", "1 366", "-2,0%", "1 350", "-2,8%");
            table.Rows.Add("2016", "86 044", "1 280", "+0,2%", "1 280", "+0,2%");
            table.Rows.Add("2017", "92 037", "1 570", "+1,8%", "1 570", "+1,8%");
            table.Rows.Add("2018", "103 626", "1 660", "+2,5%", "1 660", "+2,5%");
            table.Rows.Add("2019", "109 362", "1 690", "+2,0%", "1 690", "+2,0%");
            table.Rows.Add("2020", "106 967", "1 480", "-3,0%", "1 480", "-3,0%");
            table.Rows.Add("2021", "131 015", "1 780", "+5,6%", "1 780", "+5,6%");
            table.Rows.Add("2022", "151 455", "1 990", "-2,1%", "1 990", "-2,1%");
            table.Rows.Add("2023", "171 041", "2 050", "+2,2%", "2 050", "+2,2%");
            table.Rows.Add("2024", "~183 000", "2 100", "+1,5%", "2 100", "+1,5%");
            table.Rows.Add("2025", "~195 000", "2 150", "+1,3%", "2 150", "+1,3%");
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
    }
}