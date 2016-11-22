using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAccountingSystem_DAL.Repositories
{
    public interface IRepository<T>:IDisposable where T:class
    {
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        T GetItem(int id);
        Task<T> GetItemAsync(int id);
        void Create(T item);
        Task<T> CreateAsync(T item);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Update(T item);
        Task UpdateAsync(T item);
        void Save();
        Task SaveAsync();
        Task SaveAsync(IProgress<string> onComplete);
    }
}
