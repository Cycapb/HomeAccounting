using DomainModels.Model;
using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemUpdater
    {
        Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditViewModel model);
    }
}
