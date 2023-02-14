using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface ICategoryService
{
    Task<GenericResponse<Category>> CreateAsync(CategoryDto categoryDto);
    Task<GenericResponse<Category>> DeleteAsync(string name);
    Task<GenericResponse<Category>> UpdateAsync(string name, CategoryDto categoryDto);
    Task<GenericResponse<Category>> GetAsync(string name);
    Task<GenericResponse<List<Category>>> GetAllAsync();
}
