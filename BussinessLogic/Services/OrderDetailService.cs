using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<PayingItemProduct> _pItemProductRepository;

        public OrderDetailService(IRepository<OrderDetail> orderDetailRepository, IRepository<PayingItemProduct> pItemProductRepository)
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

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            try
            {
                orderDetail.ProductPrice =
                (await _pItemProductRepository.GetListAsync()).LastOrDefault(p => p.ProductId == orderDetail.ProductId)?
                    .Price;

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