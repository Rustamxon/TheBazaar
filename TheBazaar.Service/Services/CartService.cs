using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class CartService : ICartService
    {
        private IGenericRepo<Cart> CartRepo;
        public CartService()
        {
            CartRepo = new GenericRepo<Cart>();
        }
        public async Task<GenericResponse<decimal>> GetTotalPriceAsync(Cart cart)
        {
            decimal shippingPrice = 100;

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
        public async Task<GenericResponse<Cart>> CreateAsync(long userId)
        {
            var cart = (await CartRepo.GetAllAsync(c => c.UserId == userId)).FirstOrDefault();

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
                CreatedAt = DateTime.Now,
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
            var cart = (await CartRepo.GetAllAsync(c => c.UserId == userId)).FirstOrDefault();

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
            var cartUpdate = (await CartRepo.GetAllAsync(c => c.Id == cart.Id)).FirstOrDefault();

            if (cartUpdate is null)
                return new GenericResponse<Cart>
                {
                    StatusCode = 404,
                    Message = "Not Found",
                    Value = null
                };

            cart.UpdatedAt = DateTime.Now;
            var result = await CartRepo.UpdateAsync(cart);
            
            return new GenericResponse<Cart>
            {
                StatusCode = 200,
                Message = "Succes",
                Value = result
            };
        }

        public async Task<GenericResponse<List<Cart>>> GetAllAsync(Predicate<Cart> predicate)
        {
            var models = await CartRepo.GetAllAsync(predicate);

            return new GenericResponse<List<Cart>>
            {
                StatusCode = 200,
                Message = "Success",
                Value = models
            };
        }
    }
}
