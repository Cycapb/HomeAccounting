using DomainModels.Model;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemEditViewModelCreator
    {
        Task<PayingItemEditViewModel> CreateViewModel(PayingItem payingItem);
    }
}
