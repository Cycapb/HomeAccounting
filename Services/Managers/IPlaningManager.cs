using System;
using System.Threading.Tasks;

namespace Services.Managers
{
    /// <summary>
    /// Интерфейс менеджера планирования
    /// </summary>
    public interface IPlaningManager
    {

        /// <summary>
        /// Расчет баланса для планирования
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Task</returns>
        Task CalculatePlaningBalance(string userId);

        /// <summary>
        /// Закрытие периода для планирования
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="date">Дата закрытия периода планирования</param>
        /// <returns>Task</returns>
        Task ClosePlaningPeriod(string userId, DateTime date);

        /// <summary>
        /// Получение планируемого баланса на заданный месяц
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="month">Месяц</param>
        /// <returns>Асинхронно возвращает планируемый остаток на заданный месяц</returns>
        Task<decimal> GetPlaningBalanceForMonth(string userId, DateTime month);
    }
}