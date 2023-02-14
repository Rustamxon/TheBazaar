using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class CartService : ICartService
    {
        private IGenericRepo<Cart> CartRepo = new GenericRepo<Cart>();
        public async Task<GenericResponse<decimal>> GetTotalPriceAsync(Cart cart)
        {
            decimal shippingPrice = 50_000;

            decimal totalPrice = 0;

            foreach (var pro in cart.Items)
            {
                totalPrice += pro.Price;
            }

            return new GenericResponse<decimal>
            {
                StatusCode = 200,
                Message = "Success",
                Value = totalPrice + shippingPrice
            };
        }
        public async Task<GenericResponse<Cart>> CleareAsync(long userId)
        {
            var cart = (await CartRepo.GetAllAsync()).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
                return new GenericResponse<Cart>()
                {
                    StatusCode = 404,
                    Message = "Not Found",
                    Value = null
                };

            cart.Items = new List<Product>();
            cart.UpdatedAt = DateTime.UtcNow;
            var result = await CartRepo.UpdateAsync(cart);
            return new GenericResponse<Cart>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = result    
            };
        }
        public async Task<GenericResponse<Cart>> CreateAsync(long userId)
        {
            var cart = (await CartRepo.GetAllAsync()).FirstOrDefault(c => c.UserId == userId);
            if (cart is not null)
                return new GenericResponse<Cart>
                {
                    StatusCode = 405,
                    Message = "Cart is already created",
                    Value = null
                };

            var result = await CartRepo.CreateAsync(new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Items = new List<Product>()
            });

            return new GenericResponse<Cart>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = result
            };
        }

        public async Task<GenericResponse<Cart>> GetAsync(long userId)
        {
            var cart = (await CartRepo.GetAllAsync()).FirstOrDefault(c => c.UserId == userId);

            if (cart is null)
                return new GenericResponse<Cart>
                {
                    StatusCode = 404,
                    Message = "Not found",
                    Value = null
                };

            return new GenericResponse<Cart>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = cart
            };
        }

        public async Task<GenericResponse<Cart>> UpdateAsync(Cart cart)
        {
            var cartUpdate = (await CartRepo.GetAllAsync()).FirstOrDefault(c => c.Id == cart.Id);

            if (cartUpdate is null)
                return new GenericResponse<Cart>
                {
                    StatusCode = 404,
                    Message = "Not Found",
                    Value = null
                };

            cart.UpdatedAt = DateTime.UtcNow;
            var result = await CartRepo.UpdateAsync(cart);
            
            return new GenericResponse<Cart>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = result
            };
        }
    }
}
