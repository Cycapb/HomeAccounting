namespace Services.BaseInterfaces
{
    public interface ICreateCommandService<T> where T : class
    {
        T Create(T item);
    }
}
