using System.Threading.Tasks;

namespace Services
{
    public interface IDebtServicePartialCloser
    {
        Task Close(int debtId, decimal sum);
    }
}