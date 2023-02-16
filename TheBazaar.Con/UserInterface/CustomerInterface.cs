using System.Data;
using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;

namespace TheBazaar.Con.UserInterface;

public class CustomerInterface
{
    private User customer;
    private IUserService userService = new UserService();
    private IProductService productService = new ProductService();
    private ICartService cartService = new CartService();
    private IQuestionService questionService = new QuestionService();
    private IOrderService orderService = new OrderService();
    public CustomerInterface(User user)
    {
        customer = user;
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n                                            1 - Search");
        Console.WriteLine("                                            2 - Recommendations");
        Console.WriteLine("                                            3 - See all products");
        Console.WriteLine("                                            4 - Questions");
        Console.WriteLine("                                            5 - My cart");
        Console.WriteLine("                                            6 - My orders");
        Console.WriteLine("                                            7 - My profile");

        string input = Console.ReadLine();

        if (input == "1")
        {
            Search();
        }
        else if (input == "2")
        {
            Recommendation();
        }
        else if (input == "3")
        {
            AllProducts();
        }
        else if (input == "4")
        {
            Question();
        }
        else if (input == "5")
        {
            MyCart();
        }
        else if (input == "6")
        {
            MyOrders();
        }
        else if (input == "7")
        {
            MyProfile();
        }
        else
        {
            Start();
        }
    }
    private async void AllProducts()
    {
        Console.Clear();
        var products = (await productService.GetAllAsync(p => true)).Value;

        if (products.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n                                            Could not find anything. Press ENTER to continue.");
            Console.ReadLine();
            Start();
        }
        else
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < products.Count; i++)          
                {
                    Console.WriteLine($"                              Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"                              Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("                                            1 - Add product to my cart");
                Console.WriteLine("                                            2 - Back to the menu");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("                                            Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("                                            Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count || count > products[placeNumber - 1].Count)
                    {
                        Console.WriteLine("                                            Something is wrong! Press ENTER to continue.");
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

                        Console.WriteLine("                                            Added successfully. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }
                else if (input == "2")
                {
                    Start();
                    break;
                }
                else
                {
                    AllProducts();
                    break;
                }
            }
        }
    }
    private async void MyProfile()
    {
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n                                            First name: " + customer.FirstName);
        Console.WriteLine("                                            Last name: " + customer.LastName);
        Console.WriteLine("                                            Username: " + customer.Username);
        Console.WriteLine("                                            Password: " + customer.Password);
        Console.WriteLine("                                            Phone: " + customer.Phone);
        Console.WriteLine("                                            Updated at: " + customer.UpdatedAt);
        Console.WriteLine("                                            Created at: " + customer.CreatedAt);

        Console.WriteLine();
        Console.WriteLine("                                            1 - Update profile");
        Console.WriteLine("                                            2 - Back");

        string input = Console.ReadLine();

        if (input == "1")
        {
            Console.Clear();

            var infos = new UserDto();
            Console.Write("\n\n\n\n\n\n\n\n\n\n                                            First name: ");
            infos.FirstName = Console.ReadLine();
            Console.Write("                                            Last name: ");
            infos.LastName = Console.ReadLine();
            Console.Write("                                            Username: ");
            infos.Username = Console.ReadLine();
            Console.Write("                                            Password: ");
            infos.Password = Console.ReadLine();
            Console.Write("                                            Phone: ");
            infos.Phone = Console.ReadLine();

            var response = await userService.UpdateAsync(customer.Id, infos);

            if (response.StatusCode == 200)
            {
                Console.Write("                                            Successfully updated.");
                customer = response.Value;
            }
            else
            {
                Console.Write("                                            Something is wrong!");
            }
            Console.WriteLine("                                            Press ENTER to continue.");
            Console.ReadLine();

            MyProfile();
        }
        else if (input == "2")
        {
            Start();
        }
        else
        {
            MyProfile();
        }
    }
    private async void MyOrders()
    {
        Console.Clear();

        var orders = await orderService.GetAllAsync(o => o.UserId == customer.Id);

        if (orders.Value.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n                                            You have not any order. Press ENTER to continue.");
        }
        else
        {
            foreach (var order in orders.Value)
            {
                Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n                                            Order progress: {order.Progress} Created at: {order.CreatedAt}");
                Console.WriteLine("                                            Ordered products:");
                foreach (var prod in order.Items)
                {
                    Console.WriteLine($"                                            Product name: {prod.Name} Number: {prod.Count}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("                                            Press ENTER to continue.");
        }
        Console.ReadLine();
        Start();
    }
    private async void Question()
    {
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n                                            1 - Give question");
        Console.WriteLine("                                            2 - Answered questions");
        Console.WriteLine("                                            3 - Not answered question");
        Console.WriteLine("                                            4 - Back");

        string input1 = Console.ReadLine();

        Console.Clear();
        if (input1 == "1")
        {
            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Your question: ");
            string questionText = Console.ReadLine();

            await questionService.CreateAsync(new QuestionDto
            {
                QuestionText = questionText,
                UserId = customer.Id
            });

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            You will get your answer ASAP. Press ENTER to continue.");
            Console.ReadLine();
            Question();
        }
        else if (input1 == "2")
        {
            var questions = (await questionService.GetAllAsync(q => q.UserId == customer.Id)).Value.
                FindAll(q => q.Progress == QuestionProgressType.Answered);

            foreach (var que in questions)
            {
                Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Question: {que.QuestionText}");
                Console.WriteLine($"                                            Answer: {que.AnswerText}");
                Console.WriteLine($"                                            Answered at: {que.UpdatedAt}\n");
            }
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Press ENTER to continue.");
            Console.ReadLine();
            Question();
        }
        else if (input1 == "3")
        {
            var questions = (await questionService.GetAllAsync(q => q.UserId == customer.Id)).Value.
                FindAll(q => q.Progress == QuestionProgressType.Pending);

            foreach (var que in questions)
            {
                Console.WriteLine($"                                                      Question: {que.QuestionText}");
                Console.WriteLine($"                                                      Written time: {que.CreatedAt}\n");
            }
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Press ENTER to continue.");
            Console.ReadLine();
            Question();
        }
        else if (input1 == "4")
        {
            Start();
        }
        else
        {
            Question();
        }
    }
    private async void MyCart()
    {
        Console.Clear();

        var cart = (await cartService.GetAsync(customer.Id)).Value;
        var products = cart.Items;

        if (products.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Your cart is empty. Press ENTER to continue.");
            Console.ReadLine();
            Start();
            return;
        }

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - All products in the cart");
        Console.WriteLine("                                            2 - Make order");
        Console.WriteLine("                                            3 - Back");

        var input = Console.ReadLine();

        if (input == "1")
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"                              Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"                              Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }
                var totalPrice = (await cartService.GetTotalPriceAsync(cart)).Value;
                Console.WriteLine($"                                            Total price with delivering: {totalPrice}");

                Console.WriteLine("                                            1 - Remove product");
                Console.WriteLine("                                            2 - Change the count");
                Console.WriteLine("                                            3 - Back");
                string input2 = Console.ReadLine();

                if (input2 == "1")
                {
                    Console.Write("                                            Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count)
                    {
                        Console.WriteLine("                                            Something is wrong! Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        var p = products[placeNumber - 1];

                        cart.Items.Remove(p);

                        await cartService.UpdateAsync(cart);

                        Console.WriteLine("                                            Removed successfully. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }
                else if (input2 == "2")
                {
                    Console.Write("                                            Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("                                            Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count)
                    {
                        Console.WriteLine("                                            Place number is wrong! Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        var productLive = (await productService.GetAsync(products[placeNumber - 1].Id)).Value;
                        if (productLive.Count < count)
                        {
                            Console.WriteLine("                                            No enough product! Press ENTER to continue.");
                            Console.ReadLine();
                        }
                        else
                        {
                            var p = products[placeNumber - 1];

                            p.Count = count;

                            await cartService.UpdateAsync(cart);

                            Console.WriteLine("                                            Changed successfully. Press ENTER to continue.");
                            Console.ReadLine();
                        }
                    }
                }
                else if (input2 == "3")
                {
                    MyCart();
                    break;
                }
            }
        }
        else if (input == "2")
        {
            var totalPrice = (await cartService.GetTotalPriceAsync(cart)).Value;
            Console.WriteLine($"                                            Total price with delivering: {totalPrice}");
            Console.Write("                                            Y/y to pay: ");
            string payInput = Console.ReadLine();

            if (payInput.ToLower() == "y")
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Cash");
                Console.WriteLine("                                            2 - Payme");
                Console.WriteLine("                                            3 - Click");
                Console.WriteLine("                                            4 - UzCard");
                var payType = (PaymentType)(byte.Parse(Console.ReadLine()) * 10);
                Console.Write("                                            Your address: ");
                string address = Console.ReadLine();

                var response = await orderService.CreateAsync(new OrderDto
                {
                    Address = address,
                    PaymentType = payType,
                    Progress = OrderProgressType.Pending,
                    UserId = customer.Id,
                    Items = cart.Items
                });
                Console.WriteLine("                                            Your order is accepted. Press ENTER to continue.");
                Console.ReadLine();
                Start();
            }
            else
            {
                MyCart();
            }
        }
        else if (input == "3")
        {
            Start();
        }
        else
        {
            MyCart();
        }

    }
    private async void Recommendation()
    {
        Console.Clear();
        var products = (await productService.RecommendationsAsync(customer)).Value;

        if (products.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Could not find anything. Press ENTER to continue.");
            Console.ReadLine();
            Start();
        }
        else
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"                              Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"                              Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Add product to my cart");
                Console.WriteLine("                                            2 - Back to the menu");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("                                            Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("                                            Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count || count > products[placeNumber - 1].Count)
                    {
                        Console.WriteLine("                                            Something is wrong! Press ENTER to continue.");
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

                        Console.WriteLine("                                            Added successfully. Press ENTER to continue.");
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
    private async void Search()
    {
        Console.Clear();
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            !!!Press ENTER to skip one of the filtering!!!");

        Console.Write("                                            Name: ");
        string name = Console.ReadLine();
        Console.Write("                                            Category name: ");
        string categoryName = Console.ReadLine();
        Console.Write("                                            Min price: ");
        string minPrice = Console.ReadLine();
        Console.Write("                                            Max nprice: ");
        string maxPrice = Console.ReadLine();

        var response = await productService.SearchAsync(name, categoryName, minPrice, maxPrice);

        if (response.Value.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Could not find anything. Press ENTER to continue.");
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
                    Console.WriteLine($"                              Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"                              Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Add product to my cart");
                Console.WriteLine("                                            2 - Back to the menu");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("                                            Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("                                            Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count || count > products[placeNumber - 1].Count)
                    {
                        Console.WriteLine("                                            Something is wrong! Press ENTER to continue.");
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

                        Console.WriteLine("                                            Added successfully. Press ENTER to continue.");
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
}
