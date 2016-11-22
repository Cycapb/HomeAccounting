using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic.Exceptions;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
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

            var newOrder = await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();
            return newOrder;
        }

        public async Task DeleteAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
            await _orderRepository.SaveAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();
        }

        public async Task<IEnumerable<Order>> GetList(string userId)
        {
            return (await _orderRepository.GetListAsync())
                .Where(x => x.UserId == userId);
        }

        public void SendByEmail(int orderId)
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
            foreach (var orderDetail in order.OrderDetail)
            {
                message.Append($"Название: {orderDetail.Product.ProductName}, Цена: {orderDetail.ProductPrice?.ToString("c")}, Количество: {orderDetail.Quantity}\n");
            }

            try
            {
                _emailSender.Send(message.ToString());
            }
            catch (Exception)
            {
                throw  new SendEmailException();
            }
        }

        public async Task CloseOrder(int id)
        {
            var order = await _orderRepository.GetItemAsync(id);
            if (order != null)
            {
                order.Active = false;
                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveAsync();
            }
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _orderRepository.GetItemAsync(id);
        }
    }
}