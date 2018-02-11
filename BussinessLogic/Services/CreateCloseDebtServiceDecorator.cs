using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class CreateCloseDebtServiceDecorator:ICreateCloseDebtService
    {
        private readonly ICreateCloseDebtService _createCloseDebtService;
        private readonly IRepository<PayingItem> _payingItemRepository;
        private readonly IRepository<Category> _categoryRepository;

        public CreateCloseDebtServiceDecorator(ICreateCloseDebtService createCloseDebtService, IRepository<PayingItem> payingItemRepository, IRepository<Category> categoryRepository)
        {
            _createCloseDebtService = createCloseDebtService;
            _payingItemRepository = payingItemRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task CreateAsync(Debt debt)
        {
            await _createCloseDebtService.CreateAsync(debt);
        }

        public async Task CloseAsync(int id)
        {
            await _createCloseDebtService.CloseAsync(id);
        }

        private async Task CreateDebtPayingItem(Debt debt)
        {
            var debtCategoryId = await GetDebtCategoryId(debt);

            var payingItem = new PayingItem()
            {
                Date = debt.DateEnd ?? DateTime.Now,
                AccountID = debt.AccountId,
                CategoryID = debtCategoryId,
                Comment = debt.TypeOfFlowId == 1? "Взял деньги в долг" : "Отдал денежный долг",
                Summ = debt.Summ,
                UserId = debt.UserId
            };

            await _payingItemRepository.CreateAsync(payingItem);
            await _payingItemRepository.SaveAsync();
        }

        private async Task<int> GetDebtCategoryId(Debt debt)
        {
            var category =
                (await _categoryRepository.GetListAsync()).FirstOrDefault(c => c.Name.ToLower().Contains("Долг"));
            if (category != null)
            {
                return category.CategoryID;
            }
            category = new Category()
            {
                Active = true,
                Name = "Долг",
                TypeOfFlowID = debt.TypeOfFlowId,
                UserId = debt.UserId,
                ViewInPlan = false
            };
            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveAsync();
            return category.CategoryID;
        }
    }
}