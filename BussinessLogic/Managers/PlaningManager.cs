using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using Services.Exceptions;
using Services.Managers;

namespace BussinessLogic.Managers
{
    public class PlaningManager: IPlaningManager
    {
        private readonly IPlanItemService _planItemService;

        public PlaningManager(IPlanItemService planItemService)
        {
            _planItemService = planItemService;
        }

        public Task CalculatePlaningBalance(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task ClosePlaningPeriod(string userId, DateTime date)
        {
            var previousDate = GetPreviousDate(date);
            Func<PlanItem, bool> predicate = p =>
                p.UserId == userId && p.Month.Month == previousDate.Month && p.Month.Year == previousDate.Year && p.Closed == false;

            try
            {
                var previousItems = (await _planItemService.GetListAsync(userId))
                    .Where(predicate)
                    .ToList();

                if (previousItems.Any())
                {
                    var firstPreviousItem = previousItems.FirstOrDefault();
                    if (firstPreviousItem != null)
                    {
                        var previousBalanceFact = firstPreviousItem.BalanceFact;
                        previousItems.ForEach(p =>
                        {
                            p.Closed = true;
                            p.BalanceFact = previousBalanceFact;
                        });
                    }
                }
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в классе {nameof(PlaningManager)} в методе {nameof(ClosePlaningPeriod)} при вызове сервиса {_planItemService.GetType().Name}", e);
            }
        }
 
        public Task<decimal> GetPlaningBalanceForMonth(string userId, DateTime month)
        {
            throw new NotImplementedException();
        }

        private DateTime GetPreviousDate(DateTime date)
        {
            return date.Month == 1 ? new DateTime(date.Year - 1, 12, 1) : new DateTime(date.Year, date.Month - 1, 1);
        }

        private DateTime GetNextDate(DateTime date)
        {
            return date.Month == 12 ? new DateTime(date.Year + 1, 1, 1) : new DateTime(date.Year, date.Month + 1, 1);
        }
    }
}