using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceAsync<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync();

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    }
}
