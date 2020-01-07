using System;
using System.Web.Caching;
using Services.Caching;

namespace BussinessLogic.Services.Caching
{
    public class MemoryCacheManager:ICacheManager
    {
        private readonly Cache _cache;

        public MemoryCacheManager(Cache cache)
        {
            _cache = cache;
        }

        public virtual void Set(string key, object value)
        {
            try
            {
                _cache.Insert(key, value);
            }
            catch (Exception e)
            {
                throw new Exception($"Возникла ошибка при помещении объекта в кэш в типе {nameof(MemoryCacheManager)} в методе {nameof(Set)}", e);
            }
        }

        public virtual object Get(string key)
        {
            try
            {
                return _cache.Get(key);
            }
            catch (Exception e)
            {
                throw new Exception($"Возникла ошибка при получении объекта из кэша в типе {nameof(MemoryCacheManager)} в методе {nameof(Get)}", e);
            }
        }

        public virtual bool Remove(string key)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Возникла ошибка при удалении объекта из кэша в типе {nameof(MemoryCacheManager)} в методе {nameof(Get)}", e);
            }
        }

        public void Set(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            try
            {
                _cache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
            }
            catch (Exception e)
            {
                throw new Exception($"Возникла ошибка при помещении объекта в кэш в типе {nameof(MemoryCacheManager)} в методе {nameof(Set)}", e);
            }
        }
    }
}