using System.Threading.Tasks;

namespace Services.BaseInterfaces
{
    public interface IUpdateDeleteCommandServiceAsync<T> where T : class
    {
        Task DeleteAsync(int id);

        Task UpdateAsync(T item);
    }
}
