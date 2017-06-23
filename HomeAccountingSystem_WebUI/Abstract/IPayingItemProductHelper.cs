using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemProductHelper
    {
        Task CreatePayingItemProduct(PayingItemEditModel pItem);
        Task UpdatePayingItemProduct(PayingItemEditModel pItem);
        Task CreatePayingItemProduct(PayingItemModel pItem);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
