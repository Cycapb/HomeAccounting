using System.Threading.Tasks;

namespace Services.Triggers
{
    /// <summary>
    /// Представляет собой замену триггерам в базе данных
    /// </summary>
    /// <typeparam name="T">Любой тип сущности из базы данных</typeparam>
    public interface IServiceTrigger<in T>
    {
        /// <summary>
        /// Срабатывает при создании записи в базе данных
        /// </summary>
        /// <param name="insertedItem">Создаваемая сущность</param>
        /// <returns>Задачу</returns>
        Task Insert(T insertedItem);
        /// <summary>
        /// Срабатывает при удалении записи из базы данных
        /// </summary>
        /// <param name="deletedItem">Удаляемая сущность</param>
        /// <returns>Задачу</returns>
        Task Delete(T deletedItem);
        /// <summary>
        /// Срабатывает при обновлении записи в базе данных
        /// </summary>
        /// <param name="updatedItem">Обновляемая сущность</param>
        /// <returns>Задачу</returns>
        Task Update(T updatedItem);
    }
}
