using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.BaseInterfaces
{
    public interface IQueryService<T> where T : class
    {
        IEnumerable<T> GetList();

        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);

        T GetItem(int id);
    }
}
