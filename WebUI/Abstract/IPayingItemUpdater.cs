using DomainModels.Model;
using System;
using System.Threading.Tasks;
using WebUI.Models.PayingItemModels;

namespace WebUI.Abstract
{
    public interface IPayingItemUpdater : IDisposable
    {
        Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditModel model);
    }
}
