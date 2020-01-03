using DomainModels.Model;
using Services.BaseInterfaces;
using System.Collections.Generic;

namespace Services
{
    public interface IPayingItemService : IQueryService<PayingItem>, IQueryServiceAsync<PayingItem>, ICommandServiceAsync<PayingItem>
    {
        IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow);
    }
}