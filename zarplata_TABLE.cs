using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

public class zarplata_TABLE
{
    private DataTable table;

    public zarplata_TABLE()
    {
        InitializeTable();
    }

    public DataTable GetDataTable()
    {
        return table;
    }

    private void InitializeTable()
    {
        table = new DataTable("Медианные зарплаты");

        table.Columns.Add("Год", typeof(int));
        table.Columns.Add("Медианная зарплата (руб.)", typeof(int));
        table.Columns.Add("Рост (%)", typeof(double));

        // Заполнение данных
        table.Rows.Add(2010, 15000, 9.2);
        table.Rows.Add(2011, 16500, 10);
        table.Rows.Add(2012, 18000, 9);
        table.Rows.Add(2013, 20000, 11);
        table.Rows.Add(2014, 22000, 10);
        table.Rows.Add(2015, 23500, 6.8);
        table.Rows.Add(2016, 25000, 6.4);
        table.Rows.Add(2017, 27000, 8);
        table.Rows.Add(2018, 30000, 11);
        table.Rows.Add(2019, 32500, 8.3);
        table.Rows.Add(2020, 35472, 9.1);
        table.Rows.Add(2021, 40508, 14.2);
        table.Rows.Add(2022, 44724, 10.4);
        table.Rows.Add(2023, 52246, 16.8);
        table.Rows.Add(2024, 58000, 11);
        table.Rows.Add(2025, 63000, 8.6);
    }

    public void DisplayInDataGridView(DataGridView dataGridView)
    {
        if (dataGridView != null)
        {
            dataGridView.DataSource = table;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns["Рост (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns["Медианная зарплата (руб.)"].DefaultCellStyle.Format = "N0";
        }
    }

    public Dictionary<int, int> GetSalaryDataForChart()
    {
        var data = new Dictionary<int, int>();
        foreach (DataRow row in table.Rows)
        {
            int year = (int)row["Год"];
            int salary = (int)row["Медианная зарплата (руб.)"];
            data.Add(year, salary);
        }
        return data;
    }

    // Возвращает список годов из исходной таблицы (без прогнозных данных)
    public HashSet<int> GetOriginalYears()
    {
        var years = new HashSet<int>();
        foreach (DataRow row in table.Rows)
        {
            int year = (int)row["Год"];
            years.Add(year);
        }
        return years;
    }

    // Обновляет или добавляет запись по году, рассчитывая Рост (%) автоматически
    public void UpdateOrAddSalary(int year, double salary)
    {
        DataRow existingRow = null;
        foreach (DataRow row in table.Rows)
        {
            if ((int)row["Год"] == year)
            {
                existingRow = row;
                break;
            }
        }

        if (existingRow != null)
        {
            existingRow["Медианная зарплата (руб.)"] = (int)Math.Round(salary);
            // Пересчёт роста для этого года
            int prevYear = year - 1;
            DataRow prevRow = null;
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["Год"] == prevYear)
                {
                    prevRow = row;
                    break;
                }
            }
            if (prevRow != null)
            {
                double prevSalary = Convert.ToDouble(prevRow["Медианная зарплата (руб.)"]);
                double growth = prevSalary > 0 ? ((salary - prevSalary) / prevSalary) * 100 : 0;
                existingRow["Рост (%)"] = Math.Round(growth, 2);
            }
            else
            {
                existingRow["Рост (%)"] = 0;
            }
        }
        else
        {
            // Добавляем новую строку
            int growthPercent = 0;
            int prevYear = year - 1;
            DataRow prevRow = null;
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["Год"] == prevYear)
                {
                    prevRow = row;
                    break;
                }
            }
            if (prevRow != null)
            {
                double prevSalary = Convert.ToDouble(prevRow["Медианная зарплата (руб.)"]);
                growthPercent = prevSalary > 0 ? (int)Math.Round(((salary - prevSalary) / prevSalary) * 100) : 0;
            }
            table.Rows.Add(year, (int)Math.Round(salary), growthPercent);
        }
    }

    // Метод для получения списка значений роста (Рост %) с годами
    public Dictionary<int, double> GetGrowthData()
    {
        var growthData = new Dictionary<int, double>();
        foreach (DataRow row in table.Rows)
        {
            int year = (int)row["Год"];
            double growth = Convert.ToDouble(row["Рост (%)"]);
            growthData.Add(year, growth);
        }
        return growthData;
    }
}
