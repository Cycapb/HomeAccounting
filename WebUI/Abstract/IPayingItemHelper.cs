using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {
        void CreateCommentWhileAdd(PayingItemModel model);
        void CreateCommentWhileEdit(PayingItemEditModel model);
        void CreatePayingItemProducts(PayingItemEditModel model);
        void UpdatePayingItemProduct(PayingItemEditModel model);
    }
}
