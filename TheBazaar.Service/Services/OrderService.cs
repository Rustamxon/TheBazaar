using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class OrderService : IOrderService
    {
        private IGenericRepo<Order> orderRepo = new GenericRepo<Order>();
        public async Task<GenericResponse<Order>> CreateAsync(OrderDto order)
        {
            var orderCreate = (await orderRepo.GetAllAsync()).FirstOrDefault(p => p.UserId == order.UserId);

            if (orderCreate is not null)
                return new GenericResponse<Order>
                {
                    StatusCode= 405,
                    Message = "Order is already created",
                    Value = null
                };
            
            orderCreate.CreatedAt = DateTime.UtcNow;
            var result = await orderRepo.CreateAsync(orderCreate);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message = "Order is succces created",
                Value = result
            };
        }

        public async Task<GenericResponse<Order>> DeleteAsync(long id)
        {
            var order = (await orderRepo.GetAllAsync()).FirstOrDefault(o => o.Id == id);
            
            if(order is null)
            {
                return new GenericResponse<Order>
                {
                    StatusCode = 405,
                    Message = "Order is not found",
                    Value = null
                };
            }

            var result = await orderRepo.DeleteAsync(order.Id);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message = "Order is succes deleted",
                Value = order
            };
        }

        public async Task<GenericResponse<Order>> GetAsync(long id)
        {
            var order = (await orderRepo.GetAllAsync()).FirstOrDefault(o => o.Id == id);

            if(order is null)
            {
                return new GenericResponse<Order>
                {
                    StatusCode = 404,
                    Message = "Order is not null",
                    Value = null
                };
            }

            var result = await orderRepo.GetAsync(id);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message = "Order succes is found",
                Value = result
            };
        }

        public async Task<GenericResponse<List<Order>>> GetUsersAllOrdersAsync(long userId)
        {
            var order = (await orderRepo.GetAllAsync()).FindAll(o => o.UserId == userId);
            return new GenericResponse<List<Order>>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = order
            };
        }

        public async Task<GenericResponse<Order>> UpdateAsync(long id, OrderDto order)
        {
            var orderUpdate = (await orderRepo.GetAllAsync()).FirstOrDefault(o => o.Id == id);

            if (orderUpdate is null)
                return new GenericResponse<Order>
                {
                    StatusCode = 404,
                    Message = "User is not found",
                    Value = null
                };

            orderUpdate.Progress = order.Progress;
            orderUpdate.Address = order.Address;
            orderUpdate.UserId = order.UserId;
            orderUpdate.Items = order.Items;
            orderUpdate.UpdatedAt = DateTime.UtcNow;
            orderUpdate.PaymentType = order.PaymentType;

            var result = await orderRepo.UpdateAsync(orderUpdate);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message= "Order is succes updated",
                Value = result
            };
        }
    }
}
