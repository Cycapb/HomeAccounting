using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemEditViewModelCreator
    {
        Task<PayingItemEditViewModel> CreateViewModel(int payingItemId);
    }
}
