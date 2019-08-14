using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemProductHelper
    {
        Task CreatePayingItemProduct(PayingItemEditModel model);
        Task UpdatePayingItemProduct(PayingItemEditModel model);
        Task CreatePayingItemProduct(PayingItemModel model);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
