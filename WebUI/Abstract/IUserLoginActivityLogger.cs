using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IUserLoginActivityLogger
    {
        Task Log(AccUserModel user, string ipAddress);
    }
}
