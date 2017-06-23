using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IReporter
    {
        Task Report(AccUserModel user, string address);
    }
}
