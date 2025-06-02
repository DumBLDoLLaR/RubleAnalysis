using System.Data;

public class VVP_Extra
{
    private DataTable dataTable;
    private int n; // Количество периодов для скользящей средней

    public VVP_Extra(DataTable table, int periods)
    {
        if (periods < 2)
            throw new ArgumentException("Количество периодов для скользящей средней должно быть не меньше 2.");

        this.dataTable = table;
        this.n = periods;
    }

    // Метод для прогнозирования на 1 год вперед
    public void Forecast()
    {
        // Проверяем, что в таблице достаточно данных
        if (dataTable.Rows.Count < n)
            throw new InvalidOperationException($"Недостаточно данных для расчета скользящей средней. Требуется минимум {n} записей.");

        // Получаем последний год из таблицы
        int lastYear = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["Год"]);
        decimal lastVVP_RUB = Convert.ToDecimal(dataTable.Rows[dataTable.Rows.Count - 1]["ВВП (млрд ₽)"]);
        decimal lastVVP_USD = Convert.ToDecimal(dataTable.Rows[dataTable.Rows.Count - 1]["ВВП (млрд $)"]);
        decimal lastVND_USD = Convert.ToDecimal(dataTable.Rows[dataTable.Rows.Count - 1]["ВНД (млрд $)"]);

        // Прогнозируем на следующий год
        int newYear = lastYear + 1;

        // Прогнозируем показатели
        decimal forecastVVP_RUB = CalculateMovingAverage("ВВП (млрд ₽)");
        decimal forecastVVP_USD = CalculateMovingAverage("ВВП (млрд $)");
        decimal forecastVND_USD = CalculateMovingAverage("ВНД (млрд $)");

        // Рассчитываем рост в процентах
        decimal vvpGrowth = Math.Round((forecastVVP_RUB - lastVVP_RUB) / lastVVP_RUB * 100, 2);
        decimal vndGrowth = Math.Round((forecastVND_USD - lastVND_USD) / lastVND_USD * 100, 2);

        // Добавляем новую строку в таблицу
        DataRow newRow = dataTable.NewRow();
        newRow["Год"] = newYear;
        newRow["ВВП (млрд ₽)"] = forecastVVP_RUB;
        newRow["ВВП (млрд $)"] = forecastVVP_USD;
        newRow["Рост ВВП (%)"] = vvpGrowth;
        newRow["ВНД (млрд $)"] = forecastVND_USD;
        newRow["Рост ВНД (%)"] = vndGrowth;

        dataTable.Rows.Add(newRow);
    }

    private decimal CalculateMovingAverage(string columnName)
    {
        // Берем последние n значений (реальные + прогнозные)
        int startIndex = dataTable.Rows.Count - n;
        decimal sum = 0;

        for (int i = startIndex; i < dataTable.Rows.Count; i++)
        {
            sum += Convert.ToDecimal(dataTable.Rows[i][columnName]);
        }

        return sum / n;
    }
}