using DomainModels.Model;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemCreator
    {
        Task<PayingItem> CreatePayingItemFromViewModel(PayingItemViewModel model);
    }
}
