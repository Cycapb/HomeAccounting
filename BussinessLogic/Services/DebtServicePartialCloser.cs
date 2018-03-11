using System;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class DebtServicePartialCloser:IDebtServicePartialCloser
    {
        private readonly IRepository<Debt> _debtRepository;

        public DebtServicePartialCloser(IRepository<Debt> debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task CloseAsync(int debtId, decimal sum)
        {
            var debt = await _debtRepository.GetItemAsync(debtId);
            if (sum > debt.Summ)
            {
                throw new ArgumentOutOfRangeException(nameof(sum), "Введенная сумма больше суммы долга");
            }
            debt.Summ -= sum;
            await _debtRepository.UpdateAsync(debt);
            await _debtRepository.SaveAsync();
        }
    }
}