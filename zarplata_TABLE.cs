using System;
using System.Data;

public class zarplata_TABLE
{
    private DataTable table;

    public zarplata_TABLE()
    {
        // Инициализация таблицы при создании объекта
        InitializeTable();
    }

    private void InitializeTable()
    {
        table = new DataTable("Медианные зарплаты");

        // Создаем колонки
        table.Columns.Add("Год", typeof(int));
        table.Columns.Add("Медианная зарплата (руб.)", typeof(int));
        table.Columns.Add("Рост (%)", typeof(string));

        // Заполняем данные
        table.Rows.Add(2010, 15000, "-");
        table.Rows.Add(2011, 16500, "+10%");
        table.Rows.Add(2012, 18000, "+9%");
        table.Rows.Add(2013, 20000, "+11%");
        table.Rows.Add(2014, 22000, "+10%");
        table.Rows.Add(2015, 23500, "+6.8%");
        table.Rows.Add(2016, 25000, "+6.4%");
        table.Rows.Add(2017, 27000, "+8%");
        table.Rows.Add(2018, 30000, "+11%");
        table.Rows.Add(2019, 32500, "+8.3%");
        table.Rows.Add(2020, 35472, "+9.1%");
        table.Rows.Add(2021, 40508, "+14.2%");
        table.Rows.Add(2022, 44724, "+10.4%");
        table.Rows.Add(2023, 52246, "+16.8%");
        table.Rows.Add(2024, 58000, "+11%");
        table.Rows.Add(2025, 63000, "+8.6%");
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

            // Центрируем столбец с процентами
            dataGridView.Columns["Рост (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Форматируем числовые значения
            dataGridView.Columns["Медианная зарплата (руб.)"].DefaultCellStyle.Format = "N0";
        }
    }
}