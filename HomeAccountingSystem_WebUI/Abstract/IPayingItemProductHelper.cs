using System.Threading.Tasks;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPayingItemProductHelper
    {
        Task CreatePayingItemProduct(PayingItemEditModel pItem);
        Task UpdatePayingItemProduct(PayingItemEditModel pItem);
        Task CreatePayingItemProduct(PayingItemModel pItem);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
