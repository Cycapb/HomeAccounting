using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using Services.BaseInterfaces;

namespace Services
{
    public interface ICategoryService : IQueryService<Category>, IQueryServiceAsync<Category>, ICommandServiceAsync<Category>
    {
        /// <summary>
        /// Checks if there are any subcategories for current category
        /// </summary>
        /// <param name="id">CategoryId</param>
        /// <returns>If there are any subcategories for current category</returns>
        Task<bool> HasDependencies(int id);

        /// <summary>
        /// Returns active user's categories
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>Task which returns enumeration of user's categories</returns>
        Task<IEnumerable<Category>> GetActiveGategoriesByUser(string userId);
    }
}