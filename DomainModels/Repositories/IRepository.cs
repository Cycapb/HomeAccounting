using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainModels.Repositories
{
    public interface IRepository<T>:IDisposable where T:class
    {
        IEnumerable<T> GetList();
        IEnumerable<T> GetList(Expression<Func<T,bool>> predicate);
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
        void DeleteRange(IEnumerable<T> items);
        Task DeleteRangeAsync(IEnumerable<T> items);
    }
}
