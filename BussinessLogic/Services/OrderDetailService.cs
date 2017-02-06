using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

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
            return await _orderDetailRepository.GetItemAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderDetailRepository.DeleteAsync(id);
            await _orderDetailRepository.SaveAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetListAsync()
        {
            return await _orderDetailRepository.GetListAsync();
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail orderDetail)
        {
            orderDetail.ProductPrice =
                (await _pItemProductRepository.GetListAsync()).LastOrDefault(p => p.ProductID == orderDetail.ProductId)?
                    .Summ;

            await _orderDetailRepository.CreateAsync(orderDetail);
            await _orderDetailRepository.SaveAsync();

            return orderDetail;
        }
    }
}