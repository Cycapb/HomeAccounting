using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IEmailSender _emailSender;
        private bool _disposed;

        public OrderService(IRepository<Order> orderRepository, IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            try
            {
                var newOrder = _orderRepository.Create(order);
                await _orderRepository.SaveAsync();
                return newOrder;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
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
                _orderRepository.Update(order);
                await _orderRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task SendByEmail(int orderId, string mailTo)
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

                await _emailSender.SendAsync(message.ToString(), mailTo);

            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(SendByEmail)} при обращении к БД", e);
            }
            catch (SendEmailException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(SendByEmail)} при отправке почты", e);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Возникла ошибка в сервисе {nameof(OrderService)} в методе {nameof(SendByEmail)} при отправке почты", ex);
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
                    _orderRepository.Update(order);
                    await _orderRepository.SaveAsync();
                }
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(CloseOrder)} при обращении к БД", e);
            }

        }

        public async Task<IEnumerable<Order>> GetListAsync()
        {
            try
            {
                return await _orderRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Order>> GetListAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                return await _orderRepository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<Order> GetItemAsync(int id)
        {
            try
            {
                return await _orderRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(OrderService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _orderRepository.Dispose();
                    _emailSender.Dispose();
                }

                _disposed = true;
            }
        }
    }
}