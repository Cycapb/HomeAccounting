using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemProductHelper
    {
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
