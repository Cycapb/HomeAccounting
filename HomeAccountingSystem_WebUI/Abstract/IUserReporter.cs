using System.Threading.Tasks;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IReporter
    {
        Task Report(AccUserModel user, string address);
    }
}
