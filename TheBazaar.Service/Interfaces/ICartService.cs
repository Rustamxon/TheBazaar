using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface ICartService
{
    Task<GenericResponse<Cart>> CreateAsync(long userId);
    Task<GenericResponse<Cart>> UpdateAsync(Cart cart);
    Task<GenericResponse<Cart>> GetAsync(long userId);
    Task<GenericResponse<List<Cart>>> GetAllAsync(Predicate<Cart> predicate);
    Task<GenericResponse<decimal>> GetTotalPriceAsync(Cart cart);
}
