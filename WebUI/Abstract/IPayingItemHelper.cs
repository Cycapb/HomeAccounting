using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {        
        Task CreatePayingItemProducts(PayingItemEditViewModel model);
        Task UpdatePayingItemProducts(PayingItemEditViewModel model);        
    }
}
