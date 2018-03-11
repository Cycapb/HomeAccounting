using System.Threading.Tasks;

namespace Services
{
    public interface IDebtServicePartialCloser
    {
        Task CloseAsync(int debtId, decimal sum);
    }
}