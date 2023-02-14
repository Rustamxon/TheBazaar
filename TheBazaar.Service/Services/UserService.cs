using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services;

public class UserService : IUserInterface
{
    private readonly IGenericRepo<User> userRepository = new GenericRepo<User>();
    public async Task<GenericResponse<User>> CreateAsync(UserDto userDto)
    {
        var user  = (await userRepository.GetAllAsync()).FirstOrDefault(u => u.Username == userDto.Username);
        if (user is not null)
        {
            return new GenericResponse<User>
            {
                StatusCode = 405,
                Message = "User is already created",
                Value = null
            };
        }

        user.CreatedAt = DateTime.UtcNow;
        var  result = await userRepository.CreateAsync(user);

        return new GenericResponse<User>
        {
            StatusCode = 200,
            Message  = "Succes created",
            Value = result
        };
    }
    public async Task<GenericResponse<User>> DeleteAsync(long id)
    {
        var model = await this.userRepository.GetAsync(id);
        if (model is null)
            return new GenericResponse<User>()
            {
                StatusCode = 404,
                Message = "User is not found",
                Value = null
            };

        await this.userRepository.DeleteAsync(id);
        return new GenericResponse<User>()
        {
            StatusCode = 200,
            Message = "Success",
            Value = model
        };
    }

    public async Task<GenericResponse<List<User>>> GetAllAsync()
    {
        var result = await this.userRepository.GetAllAsync();
        return new GenericResponse<List<User>>()
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }

    public async Task<GenericResponse<User>> GetAsync(long id)
    {
        var model = await this.userRepository.GetAsync(id);
        if (model is null)
            return new GenericResponse<User>()
            {
                StatusCode = 404,
                Message = "User is not found",
                Value = null
            };

        return new GenericResponse<User>()
        {
            StatusCode = 200,
            Message = "Success",
            Value = model
        };
    }

    public async Task<GenericResponse<User>> UpdateAsync(long id, UserDto userDto)
    {
        var user = (await userRepository.GetAllAsync()).FirstOrDefault(c => c.Id == id);

        if (user is null)
            return new GenericResponse<User>
            {
                StatusCode = 404,
                Message = "User is not found",
                Value = null
            };

        user.UpdatedAt = DateTime.UtcNow;
        var result = await userRepository.UpdateAsync(user);

        return new GenericResponse<User>
        {
            StatusCode = 200,
            Message = "Succes",
            Value = result
        };
    }
}
