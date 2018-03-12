using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Services
{
    public class DebtServicePartialCloser:IDebtServicePartialCloser
    {
        private readonly IRepository<Debt> _debtRepository;
        private readonly IRepository<PayingItem> _payingItemRepository;
        private readonly IRepository<Category> _categoryRepository;

        public DebtServicePartialCloser(IRepository<Debt> debtRepository, 
            IRepository<PayingItem> payingItemRepository, 
            IRepository<Category> categoryRepository)
        {
            _debtRepository = debtRepository;
            _payingItemRepository = payingItemRepository;
            _categoryRepository = categoryRepository;
        }

        public virtual async Task CloseAsync(int debtId, decimal sum)
        {
            Debt debt;
            try
            {
                debt = await _debtRepository.GetItemAsync(debtId);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в типе {nameof(DebtServicePartialCloser)} в методе {nameof(CloseAsync)}", e);
            }
            
            if (debt == null)
            {
                return;
            }
            if (sum > debt.Summ)
            {
                throw new ArgumentOutOfRangeException(nameof(sum), "Введенная сумма больше суммы долга");
            }
            debt.Summ -= sum;
            await _debtRepository.UpdateAsync(debt);
            await _debtRepository.SaveAsync();
            await CreatePayingItem(debt, sum);
        }

        private async Task CreatePayingItem(Debt debt, decimal sum)
        {
            var typeOfFlowId = debt.TypeOfFlowId == 1 ? 2 : 1;
            var categoryId = await GetClosedDebtCategoryId(debt.UserId, typeOfFlowId);
            var payingItem = new PayingItem()
            {
                Summ = sum,
                AccountID = debt.AccountId,
                Date = debt.DateEnd ?? DateTime.Now,
                Comment = debt.TypeOfFlowId == 1 ? "Частично закрыл свой долг" : "Мне частично вернули долг",
                UserId = debt.UserId,
                CategoryID = categoryId
            };

            await _payingItemRepository.CreateAsync(payingItem);
            await _payingItemRepository.SaveAsync();
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