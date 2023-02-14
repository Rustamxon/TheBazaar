using System.Diagnostics.Metrics;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;

namespace TheBazaar.Con.UserInterface;

public class CustomerInterface
{
    private User customer;
    private IUserService userService = new UserService();
    private IProductService productService = new ProductService();
    private ICartService cartService = new CartService();
    public CustomerInterface(User user)
    {
        customer = user;
    }
    public void Start()
    {
        Console.Clear();
        Console.WriteLine("1 - Search");
        Console.WriteLine("2 - Recommendations");
        Console.WriteLine("3 - My cart");
        Console.WriteLine("4 - Questions");

        string input = Console.ReadLine();

        if (input == "1")
        {
            Search();
        }
        else if (input == "2")
        {

        }
        else if (input == "3")
        {

        }
        else if (input == "4")
        {

        }
        else
        {
            Start();
        }
    }
    private void Recommendation()
    {

    }
    private async void Search()
    {
        Console.Clear();
        Console.WriteLine("!!!Press ENTER to skip one of the filtering!!!");

        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Category name: ");
        string categoryName = Console.ReadLine();
        Console.Write("Min price: ");
        string minPrice = Console.ReadLine();
        Console.Write("Max nprice: ");
        string maxPrice = Console.ReadLine();

        var response = await productService.SearchAsync(name, categoryName, minPrice, maxPrice);

        if (response.Value.Count == 0)
        {
            Console.WriteLine("Could not find anything. Press ENTER to continue.");
            Console.ReadLine();
            Start();
        }
        else
        {
            while (true)
            {
                Console.Clear();
                var products = response.Value;
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("1 - Add product to my cart");
                Console.WriteLine("2 - Back to the menu");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count || count > products[placeNumber - 1].Count)
                    {
                        Console.WriteLine("Something is wrong! Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        var cart = (await cartService.GetAsync(customer.Id)).Value;

                        var p = products[placeNumber - 1];

                        cart.Items.Add(new Product
                        {
                            Id = p.Id,
                            CategoryId = p.CategoryId,
                            Description = p.Description,
                            Count = count,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt,
                            Name = p.Name,
                            Price = p.Price,
                            SearchTags = p.SearchTags,
                        });

                        await cartService.UpdateAsync(cart);

                        Console.WriteLine("Added successfully. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }
                else if (input == "2")
                {
                    Start();
                    break;
                }
            }
        }
    }
    private void MyCart()
    {

    }
    private void Question()
    {

    }
}
