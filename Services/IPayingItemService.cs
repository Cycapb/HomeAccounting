using DomainModels.Model;
using Services.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IPayingItemService :
        IQueryService<PayingItem>,
        IQueryServiceAsync<PayingItem>,
        IUpdateDeleteCommandServiceAsync<PayingItem>,
        ICreateCommandServiceAsync<PayingItem>,
        IDisposable
    {
        IEnumerable<PayingItem> GetListByTypeOfFlow(string userId, int typeOfFlow);

        Task<IEnumerable<PayingItem>> GetListByTypeOfFlowAsync(string userId, int typeOfFlow);
    }
}