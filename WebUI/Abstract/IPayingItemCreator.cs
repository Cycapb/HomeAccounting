using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPayingItemCreator
    {
        Task CreatePayingItemFromViewModel(PayingItemModel model);
    }
}
