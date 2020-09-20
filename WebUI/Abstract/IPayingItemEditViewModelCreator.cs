using System;
using System.Threading.Tasks;
using WebUI.Models.PayingItemModels;

namespace WebUI.Abstract
{
    public interface IPayingItemEditViewModelCreator : IDisposable
    {
        Task<PayingItemEditModel> CreateViewModel(int payingItemId);
    }
}
