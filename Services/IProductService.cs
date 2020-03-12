using DomainModels.Model;
using Services.BaseInterfaces;
using System;

namespace Services
{
    public interface IProductService :
        IQueryService<Product>,
        IQueryServiceAsync<Product>,
        IUpdateDeleteCommandServiceAsync<Product>,
        ICreateCommandServiceAsync<Product>,
        IDisposable
    {
    }
}