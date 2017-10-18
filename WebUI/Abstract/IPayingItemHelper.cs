using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemHelper
    {
        void CreateCommentWhileAdd(PayingItemModel model);

        void CreateCommentWhileEdit(PayingItemEditModel model);
    }
}
