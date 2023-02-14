using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services;

public class UserService : IUserService
{
    private readonly IGenericRepo<User> userRepository = new GenericRepo<User>();
    private readonly ICartService cartService = new CartService();
    public async Task<GenericResponse<User>> CheckLogin(string username, string password)
    {
        var users = await userRepository.GetAllAsync();

        foreach (var user in users)
        {
            if (user.Username == username && user.Password == password)
            {
                return new GenericResponse<User>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Value = user
                };
            }
        }

        return new GenericResponse<User>
        {
            StatusCode = 404,
            Message = "Not found",
            Value = null
        };
    }
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

        var newUser = new User
        {
            CreatedAt = DateTime.Now,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Password = userDto.Password,
            Username = userDto.Username,
            Phone = userDto.Phone,
            Role = UserRole.Customer
        };

        var  result = await userRepository.CreateAsync(newUser);

        await cartService.CreateAsync(result.Id);

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
        var users = await userRepository.GetAllAsync();
        var user = users.FirstOrDefault(c => c.Id == id);
        
        if (user is null)
            return new GenericResponse<User>
            {
                StatusCode = 404,
                Message = "User is not found",
                Value = null
            };

        if (user.Username != userDto.Username)
        {
            var userWithUsername = users.FirstOrDefault(c => c.Username == userDto.Username);

            if (userWithUsername is not null)
                return new GenericResponse<User>
                {
                    StatusCode = 405,
                    Message = "Username is taken",
                    Value = null
                };
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Phone = userDto.Phone;
        user.Username = userDto.Username;
        user.Password = userDto.Password;
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
