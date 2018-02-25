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
        private readonly IRepository<Debt> _debtRepository;

        public CreateCloseDebtServiceDecorator(ICreateCloseDebtService createCloseDebtService, 
            IRepository<PayingItem> payingItemRepository, 
            IRepository<Category> categoryRepository,
            IRepository<Debt> debtRepository
            )
        {
            _createCloseDebtService = createCloseDebtService;
            _payingItemRepository = payingItemRepository;
            _categoryRepository = categoryRepository;
            _debtRepository = debtRepository;
        }
        public async Task CreateAsync(Debt debt)
        {
            await _createCloseDebtService.CreateAsync(debt);
            await CreateDebtPayingItem(debt, false);
        }

        public async Task CloseAsync(int id)
        {
            var debt = await _debtRepository.GetItemAsync(id);
            await _createCloseDebtService.CloseAsync(id);
            await CreateDebtPayingItem(debt, true);
        }

        private async Task CreateDebtPayingItem(Debt debt, bool closed)
        {
            var debtCategoryId = await GetDebtCategoryId(debt);

            var payingItem = new PayingItem()
            {
                Date = debt.DateEnd ?? DateTime.Now,
                AccountID = debt.AccountId,
                CategoryID = debtCategoryId,
                Comment = CreateComment(debt.TypeOfFlowId, closed),
                Summ = debt.Summ,
                UserId = debt.UserId
            };

            await _payingItemRepository.CreateAsync(payingItem);
            await _payingItemRepository.SaveAsync();
        }

        private string CreateComment(int typeOfFlowId, bool closed)
        {
            if (!closed)
            {
                return typeOfFlowId == 1 ? "Взял деньги в долг" : "Дал деньги в долг";
            }

            return typeOfFlowId == 1 ? "Закрыл свой долг" : "Мне вернули долг";
        }

        private async Task<int> GetDebtCategoryId(Debt debt)
        {
            var category =
                (await _categoryRepository.GetListAsync())
                .Where(x => x.UserId == debt.UserId && x.TypeOfFlowID == debt.TypeOfFlowId)
                .FirstOrDefault(c => c.Name.ToLower().Contains("Долг".ToLower()));
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