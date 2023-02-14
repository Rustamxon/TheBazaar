using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IUserInterface
{
    Task<GenericResponse<UserDto>> CreateAsync(UserDto userDto);
    Task<GenericResponse<UserDto>> DeleteAsync(long id);
    Task<GenericResponse<UserDto>> UpdateAsync(long id, UserDto userDto);
    Task<GenericResponse<UserDto>> GetAsync(long id);
    Task<GenericResponse<List<UserDto>>> GetAllAsync();
}
