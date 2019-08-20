using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemProductHelper
    {
        Task CreatePayingItemProduct(PayingItemEditModel model);
        void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId);
    }
}
