using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IQuestionService
{
    Task<GenericResponse<Product>> CreateAsync(QuestionDto question);
    Task<GenericResponse<Product>> DeleteAsync(long id);
    Task<GenericResponse<Product>> UpdateAsync(long id, QuestionDto question);
    Task<GenericResponse<Category>> GetAsync(long id);
    Task<GenericResponse<List<Category>>> GetAllForAdminAsync(long userId);
}