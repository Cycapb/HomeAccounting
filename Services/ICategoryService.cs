using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с Category
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Асинхронно создает Category
        /// </summary>
        /// <param name="item">Объект Category</param>
        /// <returns>Задачу</returns>
        Task CreateAsync(Category item);
        Category GetItem(int id);
        /// <summary>
        /// Асинхронно получает Category по Id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns>Задачу, которая возвращает тип Category</returns>
        Task<Category> GetItemAsync(int id);
        /// <summary>
        /// Асинхронно получает перечисление Category
        /// </summary>
        /// <returns>Задачу, которая возвращает перечисление Category</returns>
        Task<IEnumerable<Category>> GetListAsync();
        /// <summary>
        /// Возвращает перечисление Category
        /// </summary>
        /// <returns>Gеречисление Category</returns>
        IEnumerable<Category> GetList();
        /// <summary>
        /// Асинхронно удаляет Category по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Задачу</returns>
        Task DeleteAsync(int id);
        /// <summary>
        /// Асинхронно обновляет Category
        /// </summary>
        /// <param name="item">Category</param>
        /// <returns>Задачу</returns>
        Task UpdateAsync(Category item);
        /// <summary>
        /// Асинхронно сохраняет Category в базе
        /// </summary>
        /// <returns>Задачу</returns>
        Task SaveAsync();
        /// <summary>
        /// Возвращает список подкатегорий для Category по categoryId
        /// </summary>
        /// <param name="id">CategoryId</param>
        /// <returns>Задачу, которая возвращает перечисление подкатегорий для Category по categoryId</returns>
        Task<IEnumerable<Product>> GetProducts(int id);
        /// <summary>
        /// Проверяет есть ли подкатегории у данной категории
        /// </summary>
        /// <param name="id">CategoryId</param>
        /// <returns>Признак есть или нет подкатегории у данной Category</returns>
        Task<bool> HasDependencies(int id);
        /// <summary>
        /// Возвращает активные категории для пользователя
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>Задачу, которая возвращает перечисление активных категорий пользователя</returns>
        Task<IEnumerable<Category>> GetActiveGategoriesByUser(string userId);
    }
}