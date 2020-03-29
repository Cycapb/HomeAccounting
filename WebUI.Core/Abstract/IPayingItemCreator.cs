using DomainModels.Model;
using System;
using System.Threading.Tasks;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Core.Abstract
{
    public interface IPayingItemCreator : IDisposable
    {
        Task<PayingItem> CreatePayingItemFromViewModel(PayingItemModel model);
    }
}
