using System;
using System.Threading.Tasks;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Core.Abstract
{
    public interface IPayingItemEditViewModelCreator : IDisposable
    {
        Task<PayingItemEditModel> CreateViewModel(int payingItemId);
    }
}
