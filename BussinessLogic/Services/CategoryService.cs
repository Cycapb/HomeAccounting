using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(Category item)
        {
            item.Active = true;
            await _categoryRepository.CreateAsync(item);
            await _categoryRepository.SaveAsync();
        }

        public async Task<Category> GetItemAsync(int id)
        {
            return await _categoryRepository.GetItemAsync(id);
        }

        public async Task<IEnumerable<Category>> GetListAsync()
        {
            return await _categoryRepository.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            await _categoryRepository.SaveAsync();
        }

        public async Task UpdateAsync(Category item)
        {
            await _categoryRepository.UpdateAsync(item);
        }

        public async Task SaveAsync()
        {
            await _categoryRepository.SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(int id)
        {
            return (await _categoryRepository.GetItemAsync(id)).Product;
        }

        public async Task<bool> HasDependencies(int id)
        {
            var cat = await _categoryRepository.GetItemAsync(id);
            return cat.PayingItem.Any();
        }

        public async Task<IEnumerable<Category>> GetActiveGategoriesByUser(string userId)
        {
            var cats = await _categoryRepository.GetListAsync();
            return cats?.Where(x => x.Active && x.UserId == userId);
        }
    }
}