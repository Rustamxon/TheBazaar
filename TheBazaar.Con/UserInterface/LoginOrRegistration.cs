using TheBazaar.Service.DTOs;
using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;

namespace TheBazaar.Con.UserInterface;

public class LoginOrRegistration
{
    private IUserService userService = new UserService();
    public void Start()
    {
        Console.Clear();
        Console.WriteLine("1 - Login");
        Console.WriteLine("2 - Registration");
        string input = Console.ReadLine();

        if (input == "1")
            Login();
        else if (input == "2")
            Registration();
        else 
            Start();
    }
    private async void Login()
    {
        Console.Clear();
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        var response = await userService.CheckLogin(username, password);

        if (response.StatusCode == 200)
        {
            var customerInterface = new CustomerInterface(response.Value);
            customerInterface.Start();
        }
        else
        {
            Console.WriteLine("Username or password was wrong! Press enter to continue.");
            Console.ReadLine();
            this.Start();
        }
    }
    private async void Registration()
    {
        Console.Clear();
        var userDto = new UserDto();
        
        Console.Write("Phone: ");
        userDto.Phone = Console.ReadLine();
        Console.Write("Username: ");
        userDto.Username = Console.ReadLine();
        Console.Write("Password: ");
        userDto.Password = Console.ReadLine();
        Console.Write("Firstname: ");
        userDto.FirstName = Console.ReadLine();
        Console.Write("Lastname: ");
        userDto.LastName = Console.ReadLine();

        var response = await userService.CreateAsync(userDto);

        if (response.StatusCode == 200)
        {
            var customerInterface = new CustomerInterface(response.Value);
            customerInterface.Start();
        }
        else
        {
            Console.WriteLine("Username was taken! Press enter to continue.");
            Console.ReadLine();
            this.Start();
        }
    }
}
