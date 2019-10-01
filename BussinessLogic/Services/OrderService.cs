using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using Services.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class OrderService:IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IEmailSender _emailSender;

        public OrderService(IRepository<Order> orderRepository, IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
        }

        public async Task<Order> CreateOrderAsync(DateTime orderDate, string userId)
        {
            var order = new Order()
            {
                OrderDate = orderDate,
                UserId = userId,
                Active = true
            };

            try
            {
                var newOrder = await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveAsync();
                return newOrder;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(CreateOrderAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _orderRepository.DeleteAsync(id);
                await _orderRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
            
        }

        public async Task UpdateAsync(Order order)
        {
            try
            {
                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Order>> GetList(string userId)
        {
            try
            {
                return (await _orderRepository.GetListAsync())
                    .Where(x => x.UserId == userId);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public void SendByEmail(int orderId, string mailTo)
        {            
            try
            {
                var order = _orderRepository.GetItem(orderId);

                if (order == null)
                {
                    return;
                }

                var message = new StringBuilder();
                message.Append($"Дата списка: {order.OrderDate}\n");
                message.Append($"Номер списка: {order.OrderID}\n");
                message.Append("Список покупок:\n");

                foreach (var orderDetail in order.OrderDetails)
                {
                    message.Append($"Название: {orderDetail.Product.ProductName}, Цена: {orderDetail.ProductPrice?.ToString("c")}\n");
                }
                message.Append("----------------------------------\n");
                message.Append("");
                message.Append($"Итого: {order.OrderDetails.Sum(x => x.ProductPrice)?.ToString("F")}");

                _emailSender.Send(message.ToString(), mailTo);

            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(SendByEmail)} при обращении к БД", e);
            }
            catch (Exception ex)
            {
                throw new SendEmailException($"Возникла ошибка в сервисе {nameof(OrderService)} в методе {nameof(SendByEmail)} при отправке почты", ex);
            }            
        }

        public async Task CloseOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetItemAsync(id);
                if (order != null)
                {
                    order.Active = false;
                    await _orderRepository.UpdateAsync(order);
                    await _orderRepository.SaveAsync();
                }
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(CloseOrder)} при обращении к БД", e);
            }
            
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            try
            {
                return await _orderRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(GetOrderAsync)} при обращении к БД", e);
            }
        }
    }
}