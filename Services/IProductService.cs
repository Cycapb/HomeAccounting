using DomainModels.Model;
using Services.BaseInterfaces;

namespace Services
{
    public interface IProductService : IQueryService<Product>, IQueryServiceAsync<Product>, ICommandServiceAsync<Product>
    {
    }
}