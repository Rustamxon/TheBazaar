using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services;

public class QuestionService : IQuestionService
{
    private IGenericRepo<Question> questionRepo = new GenericRepo<Question>();
    public async Task<GenericResponse<Question>> CreateAsync(QuestionDto question)
    {
        var que = new Question
        {
            UserId = question.UserId,
            QuestionText = question.QuestionText,
            Progress = QuestionProgressType.Pending,
            CreatedAt = DateTime.Now
        };

        var res = await questionRepo.CreateAsync(que);

        return new GenericResponse<Question>
        {
            StatusCode = 200,
            Message = "Success",
            Value = res
        };
    }

    public async Task<GenericResponse<Question>> DeleteAsync(long id)
    {
        var res = await questionRepo.DeleteAsync(id);

        if (res == false)
            return new GenericResponse<Question>
            {
                StatusCode = 404,
                Message = "Not found",
                Value = null
            };

        return new GenericResponse<Question>
        {
            StatusCode = 200,
            Message = "Success",
            Value = null
        };
    }

    public async Task<GenericResponse<List<Question>>> GetAllForAdminAsync()
    {
        var result = (await questionRepo.GetAllAsync()).FindAll(u => u.Progress == QuestionProgressType.Pending);

        return new GenericResponse<List<Question>>
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }

    public async Task<GenericResponse<List<Question>>> GetAllUserQuestionsAsync(long userId)
    {
        var result = (await questionRepo.GetAllAsync()).FindAll(q => q.UserId == userId);

        return new GenericResponse<List<Question>>
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }

    public async Task<GenericResponse<Question>> GetAsync(long id)
    {
        var result = await questionRepo.GetAsync(id);

        if (result is null)
            return new GenericResponse<Question>
            {
                StatusCode = 404,
                Message = "Success",
                Value = null
            };
        
        return new GenericResponse<Question>
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }

    public async Task<GenericResponse<Question>> UpdateAsync(Question question)
    {
        question.UpdatedAt = DateTime.Now;
        var res = await questionRepo.UpdateAsync(question);

        if (res is null)
            return new GenericResponse<Question>
            {
                StatusCode = 400,
                Message = "Something went wrong",
                Value = null
            };

        return new GenericResponse<Question>
        {
            StatusCode = 200,
            Message = "Success",
            Value = res
        };
    }
}
