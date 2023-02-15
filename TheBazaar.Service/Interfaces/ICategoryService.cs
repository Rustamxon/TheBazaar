using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface ICategoryService
{
    Task<GenericResponse<Category>> CreateAsync(CategoryDto categoryDto);
    Task<GenericResponse<Category>> DeleteAsync(long id);
    Task<GenericResponse<Category>> UpdateAsync(long id, CategoryDto categoryDto);
    Task<GenericResponse<Category>> GetAsync(long id);
    Task<GenericResponse<List<Category>>> GetAllAsync(Predicate<Category> predicate);
}
