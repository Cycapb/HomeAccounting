using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class CreateCloseDebtServicePayingItemDecorator:ICreateCloseDebtService
    {
        private readonly ICreateCloseDebtService _createCloseDebtService;
        private readonly IRepository<PayingItem> _payingItemRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Debt> _debtRepository;

        public CreateCloseDebtServicePayingItemDecorator(ICreateCloseDebtService createCloseDebtService, 
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
            await CreateOpenedDebtPayingItem(debt);
        }

        public async Task CloseAsync(int id)
        {
            var debt = await _debtRepository.GetItemAsync(id);
            await _createCloseDebtService.CloseAsync(id);
            await CreateClosedDebtPayingItem(debt);
        }

        public async Task PartialCloseAsync(int debtId, decimal sum)
        {
            var debt = await _debtRepository.GetItemAsync(debtId);
            if (debt.Summ < sum)
            {
                throw new ArgumentOutOfRangeException(nameof(sum), "Введенная сумма больше суммы долга");
            }
            await _createCloseDebtService.PartialCloseAsync(debtId, sum);
            await CreateClosedDebtPayingItem(debt, sum);
        }

        private async Task CreateClosedDebtPayingItem(Debt debt, decimal sum = 0M)
        {
            var typeOfFlowId = debt.TypeOfFlowId == 1 ? 2 : 1;
            var categoryId = await GetClosedDebtCategoryId(debt.UserId, typeOfFlowId);

            var payingItem = new PayingItem()
            {
                AccountID = debt.AccountId,
                Date = debt.DateEnd ?? DateTime.Now,
                CategoryID = categoryId,
                Comment = debt.TypeOfFlowId == 1 ? "Закрыл свой долг" : "Мне вернули долг",
                Summ = sum == 0M? debt.Summ : sum,
                UserId = debt.UserId
            };

            await _payingItemRepository.CreateAsync(payingItem);
            await _payingItemRepository.SaveAsync();
        }

        private async Task CreateOpenedDebtPayingItem(Debt debt)
        {
            var debtCategoryId = await GetDebtCategoryId(debt);

            var payingItem = new PayingItem()
            {
                Date = debt.DateEnd ?? DateTime.Now,
                AccountID = debt.AccountId,
                CategoryID = debtCategoryId,
                Comment = debt.TypeOfFlowId == 1 ? "Взял деньги в долг" : "Дал деньги в долг",
                Summ = debt.Summ,
                UserId = debt.UserId
            };

            await _payingItemRepository.CreateAsync(payingItem);
            await _payingItemRepository.SaveAsync();
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

        private async Task<int> GetClosedDebtCategoryId(string userId, int typeOfFlowId)
        {
            var category =
                (await _categoryRepository.GetListAsync())
                .Where(x => x.UserId == userId && x.TypeOfFlowID == typeOfFlowId)
                .FirstOrDefault(c => c.Name.ToLower().Contains("Долг".ToLower()));
                return category.CategoryID;
        }
    }
}