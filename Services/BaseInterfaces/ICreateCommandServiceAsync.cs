using System.Threading.Tasks;

namespace Services.BaseInterfaces
{
    public interface ICreateCommandServiceAsync<T> where T : class
    {
        Task<T> CreateAsync(T item);
    }
}
