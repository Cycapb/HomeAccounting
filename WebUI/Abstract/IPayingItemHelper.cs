using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {        
        void CreateCommentWhileEdit(PayingItemEditModel model);
        void CreatePayingItem(PayingItemModel model);
        Task CreatePayingItemProducts(PayingItemEditModel model);
        Task UpdatePayingItemProducts(PayingItemEditModel model);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
        void SetSumForPayingItem(PayingItemEditModel model);
    }
}
