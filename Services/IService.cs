using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetList();
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    }
}
