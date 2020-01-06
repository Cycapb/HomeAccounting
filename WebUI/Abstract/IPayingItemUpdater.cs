using DomainModels.Model;
using System;
using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemUpdater : IDisposable
    {
        Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditViewModel model);
    }
}
