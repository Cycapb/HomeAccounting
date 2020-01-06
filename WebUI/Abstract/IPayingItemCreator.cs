using DomainModels.Model;
using System;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemCreator : IDisposable
    {
        Task<PayingItem> CreatePayingItemFromViewModel(PayingItemViewModel model);
    }
}
