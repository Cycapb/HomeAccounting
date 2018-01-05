namespace Services.Caching
{
    public interface ICacheManager
    {
        void Set(object key, object value);
        object Get(object key);
        bool Remove(object key);
    }
}