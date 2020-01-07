using System;
using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemEditViewModelCreator : IDisposable
    {
        Task<PayingItemEditViewModel> CreateViewModel(int payingItemId);
    }
}
