using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {
        void CreateCommentWhileAdd(PayingItemModel model);
        void CreateCommentWhileEdit(PayingItemEditModel model);
        void CreatePayingItemProducts(PayingItemModel model);
        Task CreatePayingItemProducts(PayingItemEditModel model);
        Task UpdatePayingItemProducts(PayingItemEditModel model);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
