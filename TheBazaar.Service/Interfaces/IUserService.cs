﻿using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;

namespace TheBazaar.Service.Interfaces;

public interface IUserService
{
    Task<GenericResponse<User>> CreateAsync(UserDto userDto);
    Task<GenericResponse<User>> DeleteAsync(long id);
    Task<GenericResponse<User>> UpdateAsync(long id, UserDto userDto);
    Task<GenericResponse<User>> GetAsync(long id);
    Task<GenericResponse<User>> CheckLogin(string username, string password);
    Task<GenericResponse<List<User>>> GetAllAsync(Predicate<User> predicate);
}
