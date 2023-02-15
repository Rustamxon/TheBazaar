using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class OrderService : IOrderService
    {
        private IGenericRepo<Order> orderRepo;
        private ICartService cartService;
        private IProductService productService;
        public OrderService() 
        {
            orderRepo = new GenericRepo<Order>();
            cartService = new CartService();
            productService = new ProductService();
        }
        public async Task<GenericResponse<Order>> CreateAsync(OrderDto order)
        {
            var mapped = new Order
            {
                Address = order.Address,
                CreatedAt = DateTime.Now,
                Items = order.Items,
                PaymentType = order.PaymentType,
                Progress = OrderProgressType.Pending,
                UserId = order.UserId
            };

            var result = await orderRepo.CreateAsync(mapped);

            var cart = (await cartService.GetAsync(order.UserId)).Value;
            cart.Items = new List<Product>();
            await cartService.UpdateAsync(cart);

            foreach (var pro in result.Items)
            {
                var rPro = (await productService.GetAsync(pro.Id)).Value;
                rPro.Count -= pro.Count;

                var mappedToDto = new ProductDto
                {
                    CategoryId = rPro.CategoryId,
                    Count = rPro.Count,
                    Description = rPro.Description,
                    Name = rPro.Name,
                    Price = rPro.Price,
                    SearchTags = rPro.SearchTags,
                };

                await productService.UpdateAsync(pro.Id, mappedToDto);
            }

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

        public async Task<GenericResponse<List<Order>>> GetAllAsync(Predicate<Order> predicate)
        {
            var order = await orderRepo.GetAllAsync(predicate);
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
            orderUpdate.UpdatedAt = DateTime.Now;
            orderUpdate.PaymentType = order.PaymentType;

            var result = await orderRepo.UpdateAsync(orderUpdate);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message= "Order is succes updated",
                Value = result
            };
        }

        public async Task<GenericResponse<Order>> UpdateToNextProcessAsync(long id)
        {
            var order = (await GetAsync(id)).Value;

            if (order is null)
            {
                return new GenericResponse<Order>
                {
                    StatusCode = 404,
                    Message = "Not found",
                    Value = null
                };
            }

            if (order.Progress == OrderProgressType.Pending)
                order.Progress = OrderProgressType.Processing;
            else if (order.Progress == OrderProgressType.Processing)
                order.Progress = OrderProgressType.Delivered;

            var result = await orderRepo.UpdateAsync(order);

            return new GenericResponse<Order>
            {
                StatusCode = 200,
                Message = "Success",
                Value = result
            };
        }
    }
}
