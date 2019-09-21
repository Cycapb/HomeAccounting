using System.Threading.Tasks;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {
        void CreateCommentWhileEdit(PayingItemEditViewModel model);
        Task CreatePayingItemProducts(PayingItemEditViewModel model);
        Task UpdatePayingItemProducts(PayingItemEditViewModel model);
        void SetSumForPayingItem(PayingItemEditViewModel model);
    }
}
