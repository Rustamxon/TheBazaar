using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IOrderService
{
    Task<GenericResponse<Order>> CreateAsync(OrderDto order);
    Task<GenericResponse<Order>> DeleteAsync(long id);
    Task<GenericResponse<Order>> UpdateAsync(long id, OrderDto order);
    Task<GenericResponse<Order>> GetAsync(long id);
    Task<GenericResponse<Order>> UpdateToNextProcessAsync(long id);
    Task<GenericResponse<List<Order>>> GetAllAsync(Predicate<Order> predicate);
}
