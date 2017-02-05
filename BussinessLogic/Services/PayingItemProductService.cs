using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class PayingItemProductService:IPayingItemProductService
    {
        private readonly IRepository<PaiyngItemProduct> _pItemRepository;

        public PayingItemProductService(IRepository<PaiyngItemProduct> pItemRepository)
        {
            _pItemRepository = pItemRepository;
        }

        public async Task CreateAsync(PaiyngItemProduct product)
        {
            await _pItemRepository.CreateAsync(product);
        }

        public async Task<PaiyngItemProduct> GetItemAsync(int id)
        {
            return await _pItemRepository.GetItemAsync(id);
        }

        public IEnumerable<PaiyngItemProduct> GetList()
        {
            return _pItemRepository.GetList();
        }

        public async Task<IEnumerable<PaiyngItemProduct>> GetListAsync()
        {
            return await _pItemRepository.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _pItemRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(PaiyngItemProduct item)
        {
            await _pItemRepository.UpdateAsync(item);
        }

        public async Task SaveAsync()
        {
            await _pItemRepository.SaveAsync();
        }
    }
}
