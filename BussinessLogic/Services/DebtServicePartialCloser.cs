using System.Threading.Tasks;
using Services;

namespace BussinessLogic.Services
{
    public class DebtServicePartialCloser:IDebtServicePartialCloser
    {
        public Task Close(int debtId, decimal sum)
        {
            throw new System.NotImplementedException();
        }
    }
}