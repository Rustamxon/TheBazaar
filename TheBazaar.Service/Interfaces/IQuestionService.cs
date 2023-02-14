using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IQuestionService
{
    Task<GenericResponse<Question>> CreateAsync(QuestionDto question);
    Task<GenericResponse<Question>> DeleteAsync(long id);
    Task<GenericResponse<Question>> UpdateAsync(Question question);
    Task<GenericResponse<Question>> GetAsync(long id);
    Task<GenericResponse<List<Question>>> GetAllForAdminAsync(long userId);
}