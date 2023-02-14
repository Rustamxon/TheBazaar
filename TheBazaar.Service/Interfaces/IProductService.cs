using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IProductService
{
    Task<GenericResponse<Product>> CreateAsync(ProductDto product);
    Task<GenericResponse<Product>> DeleteAsync(long id);
    Task<GenericResponse<Product>> UpdateAsync(long id, ProductDto product);
    Task<GenericResponse<Product>> GetAsync(long id);
    Task<GenericResponse<List<Product>>> SearchAsync(string name, string categoryName, string minPrice, string maxPrice);
    Task<GenericResponse<List<Product>>> RecommendationsAsync(User user);
    Task<GenericResponse<List<Product>>> GetAllAsync();
}
