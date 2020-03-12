using DomainModels.Model;
using Services.BaseInterfaces;
using System;
using System.Collections.Generic;

namespace Services
{
    public interface IPayingItemService :
        IQueryService<PayingItem>,
        IQueryServiceAsync<PayingItem>,
        IUpdateDeleteCommandServiceAsync<PayingItem>,
        ICreateCommandServiceAsync<PayingItem>,
        IDisposable
    {
        IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow);
    }
}