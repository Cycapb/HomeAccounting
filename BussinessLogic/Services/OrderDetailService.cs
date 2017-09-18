using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Services
{
    public class OrderDetailService:IOrderDetailService
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<PaiyngItemProduct> _pItemProductRepository;

        public OrderDetailService(IRepository<OrderDetail> orderDetailRepository, IRepository<PaiyngItemProduct> pItemProductRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _pItemProductRepository = pItemProductRepository;
        }

        public async Task<OrderDetail> GetItemAsync(int id)
        {
            try
            {
                return await _orderDetailRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderDetailService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _orderDetailRepository.DeleteAsync(id);
                await _orderDetailRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderDetailService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetListAsync()
        {
            try
            {
                return await _orderDetailRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderDetailService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            try
            {
                orderDetail.ProductPrice =
                (await _pItemProductRepository.GetListAsync()).LastOrDefault(p => p.ProductID == orderDetail.ProductId)?
                    .Summ;

                await _orderDetailRepository.CreateAsync(orderDetail);
                await _orderDetailRepository.SaveAsync();

                return orderDetail;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderDetailService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }
    }
}