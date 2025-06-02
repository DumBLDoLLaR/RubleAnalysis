using System;
using System.Data;

namespace RubleAnalysis
{
    public class zarplata_random
    {
        private DataTable dataTable;
        private int n; // Количество лет для скользящей средней

        public zarplata_random(DataTable table, int periods)
        {
            if (periods < 2)
                throw new ArgumentException("Количество лет для скользящей средней должно быть не меньше 2.");

            this.dataTable = table ?? throw new ArgumentNullException(nameof(table));
            this.n = periods;
        }

        // Метод для прогноза на 1 год вперёд
        public void ForecastNextYear()
        {
            if (dataTable.Rows.Count < n)
                throw new InvalidOperationException($"Недостаточно данных для расчёта скользящей средней. Требуется минимум {n} записей.");

            // Получаем последний год и зарплату
            int lastYear = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["Год"]);
            double lastSalary = Convert.ToDouble(dataTable.Rows[dataTable.Rows.Count - 1]["Медианная зарплата (руб.)"]);

            // Прогнозируем следующий год
            int newYear = lastYear + 1;
            double forecastSalary = CalculateMovingAverage("Медианная зарплата (руб.)");

            // Рассчитываем рост в процентах
            double growthPercent = lastSalary > 0 ? Math.Round((forecastSalary - lastSalary) / lastSalary * 100, 2) : 0;

            // Создаём новую строку с прогнозом
            DataRow newRow = dataTable.NewRow();
            newRow["Год"] = newYear;
            newRow["Медианная зарплата (руб.)"] = (int)Math.Round(forecastSalary);
            newRow["Рост (%)"] = growthPercent;

            // Добавляем строку в таблицу
            dataTable.Rows.Add(newRow);
        }

        private double CalculateMovingAverage(string columnName)
        {
            int startIndex = dataTable.Rows.Count - n;
            double sum = 0;

            for (int i = startIndex; i < dataTable.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dataTable.Rows[i][columnName]);
            }

            return sum / n;
        }
    }
}
