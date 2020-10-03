using System.Threading.Tasks;
using WebUI.Core.Models;

namespace WebUI.Core.Abstract
{
    public interface IReporter
    {
        Task Report(AccountingUserModel user, string address);
    }
}
