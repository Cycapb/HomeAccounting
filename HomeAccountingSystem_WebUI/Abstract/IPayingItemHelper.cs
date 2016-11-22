using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPayingItemHelper
    {
        void CreateCommentWhileAdd(PayingItemModel model);

        void CreateCommentWhileEdit(PayingItemEditModel model);
    }
}
