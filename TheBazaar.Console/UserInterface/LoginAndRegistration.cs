using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;
using System;

namespace TheBazaar.Console.UserInterface;

public class LoginAndRegistration
{
    private IUserService userService = new UserService();
    public void Start()
    {
        Console.WriteLine();
    }
}
