using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;

namespace TheBazaar.Con.UserInterface;

public class LoginOrRegistration
{
    private IUserService userService = new UserService();

    public void Start()
    {
        Console.ReadKey();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.CursorVisible= false;

        Console.Clear();
        Console.WriteLine("\n\n\n\r\n\r\n                    \r\n                    ██████╗░██╗███╗░░██╗░█████╗░██████╗░██╗░░░██╗  ████████╗███████╗░█████╗░███╗░░░███╗\r\n                    ██╔══██╗██║████╗░██║██╔══██╗██╔══██╗╚██╗░██╔╝  ╚══██╔══╝██╔════╝██╔══██╗████╗░████║\r\n                    ██████╦╝██║██╔██╗██║███████║██████╔╝░╚████╔╝░  ░░░██║░░░█████╗░░███████║██╔████╔██║\r\n                    ██╔══██╗██║██║╚████║██╔══██║██╔══██╗░░╚██╔╝░░  ░░░██║░░░██╔══╝░░██╔══██║██║╚██╔╝██║\r\n                    ██████╦╝██║██║░╚███║██║░░██║██║░░██║░░░██║░░░  ░░░██║░░░███████╗██║░░██║██║░╚═╝░██║\r\n                    ╚═════╝░╚═╝╚═╝░░╚══╝╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░  ░░░╚═╝░░░╚══════╝╚═╝░░╚═╝╚═╝░░░░░╚═╝\r\n\r\n                    ██████╗░██████╗░███████╗░██████╗███████╗███╗░░██╗████████╗░██████╗\r\n                    ██╔══██╗██╔══██╗██╔════╝██╔════╝██╔════╝████╗░██║╚══██╔══╝██╔════╝\r\n                    ██████╔╝██████╔╝█████╗░░╚█████╗░█████╗░░██╔██╗██║░░░██║░░░╚█████╗░\r\n                    ██╔═══╝░██╔══██╗██╔══╝░░░╚═══██╗██╔══╝░░██║╚████║░░░██║░░░░╚═══██╗\r\n                    ██║░░░░░██║░░██║███████╗██████╔╝███████╗██║░╚███║░░░██║░░░██████╔╝\r\n                    ╚═╝░░░░░╚═╝░░╚═╝╚══════╝╚═════╝░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░╚═════╝░");
        Console.Beep(100, 2000);
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n                     \r\n                    ██╗░░░░░░█████╗░░██████╗░██╗███╗░░██╗\r\n                    ██║░░░░░██╔══██╗██╔════╝░██║████╗░██║\r\n                    ██║░░░░░██║░░██║██║░░██╗░██║██╔██╗██║\r\n                    ██║░░░░░██║░░██║██║░░╚██╗██║██║╚████║\r\n                    ███████╗╚█████╔╝╚██████╔╝██║██║░╚███║\r\n                    ╚══════╝░╚════╝░░╚═════╝░╚═╝╚═╝░░╚══╝");
        Console.WriteLine("                                                     \r\n                    ██████╗░███████╗░██████╗░██╗░██████╗████████╗██████╗░░█████╗░████████╗██╗░█████╗░███╗░░██╗\r\n                    ██╔══██╗██╔════╝██╔════╝░██║██╔════╝╚══██╔══╝██╔══██╗██╔══██╗╚══██╔══╝██║██╔══██╗████╗░██║\r\n                    ██████╔╝█████╗░░██║░░██╗░██║╚█████╗░░░░██║░░░██████╔╝███████║░░░██║░░░██║██║░░██║██╔██╗██║\r\n                    ██╔══██╗██╔══╝░░██║░░╚██╗██║░╚═══██╗░░░██║░░░██╔══██╗██╔══██║░░░██║░░░██║██║░░██║██║╚████║\r\n                    ██║░░██║███████╗╚██████╔╝██║██████╔╝░░░██║░░░██║░░██║██║░░██║░░░██║░░░██║╚█████╔╝██║░╚███║\r\n                    ╚═╝░░╚═╝╚══════╝░╚═════╝░╚═╝╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░╚═╝░╚════╝░╚═╝░░╚══╝");
        int input = Convert.ToInt32(Console.ReadKey().Key);


        if (input == 49)
            Login();
        else if (input == 50)
            Registration();
        else 
            Start();
    }
    private async void Login()
    {
        Console.Clear();
        Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Username: ");
        string username = Console.ReadLine();
        Console.Write("                                            Password: ");
        string password = Console.ReadLine();

        var response = await userService.CheckLogin(username, password);

        if (response.StatusCode == 200)
        {
            if (response.Value.Role == UserRole.Customer)
            {
                var customerInterface = new CustomerInterface(response.Value);
                customerInterface.Start();
            }
            else if (response.Value.Role == UserRole.Seller)
            {
                var sellerInterface = new SellerInterface(response.Value);
                sellerInterface.Start();
            }
        }
        else
        {
            Console.WriteLine("                                            Username or password was wrong! Press enter to continue.");
            Console.ReadLine();
            this.Start();
        }
    }
    private async void Registration()
    {
        Console.Clear();
        var userDto = new UserDto();
        
        Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Phone: ");
        userDto.Phone = Console.ReadLine();
        Console.Write("                                            Username: ");
        userDto.Username = Console.ReadLine();
        Console.Write("                                            Password: ");
        userDto.Password = Console.ReadLine();
        Console.Write("                                            Firstname: ");
        userDto.FirstName = Console.ReadLine();
        Console.Write("                                            Lastname: ");
        userDto.LastName = Console.ReadLine();

        var response = await userService.CreateAsync(userDto);

        if (response.StatusCode == 200)
        {
            var customerInterface = new CustomerInterface(response.Value);
            customerInterface.Start();
        }
        else
        {
            Console.WriteLine("                                            Username was taken! Press enter to continue.");
            Console.ReadLine();
            this.Start();
        }
    }
}
