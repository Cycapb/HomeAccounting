using System;

namespace Services.Caching
{
    public interface ICacheManager
    {
        void Set(string key, object value);

        object Get(string key);

        bool Remove(string key);

        void Set(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration);
    }
}