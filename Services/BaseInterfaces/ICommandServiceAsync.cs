using System.Threading.Tasks;

namespace Services.BaseInterfaces
{
    public interface ICommandServiceAsync<T> where T : class
    {
        Task DeleteAsync(int id);

        Task UpdateAsync(T item);

        Task<T> CreateAsync(T item);
    }
}
