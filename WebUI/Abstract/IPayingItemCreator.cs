using DomainModels.Model;
using System;
using System.Threading.Tasks;
using WebUI.Models.PayingItemModels;

namespace WebUI.Abstract
{
    public interface IPayingItemCreator : IDisposable
    {
        Task<PayingItem> CreatePayingItemFromViewModel(PayingItemModel model);
    }
}
