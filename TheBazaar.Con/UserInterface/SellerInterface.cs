using TheBazaar.Domain.Entities;
using TheBazaar.Domain.Enums;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Interfaces;
using TheBazaar.Service.Services;

namespace TheBazaar.Con.UserInterface;

public class SellerInterface
{
    private User seller;
    private IOrderService orderService = new OrderService();
    private IProductService productService = new ProductService();
    private ICategoryService categoryService = new CategoryService();
    private IQuestionService questionService = new QuestionService();
    public SellerInterface(User user)
    {
        seller = user;
    }
    public void Start()
    {
        Console.Clear();

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Orders");
        Console.WriteLine("                                            2 - Products");
        Console.WriteLine("                                            3 - Categories");
        Console.WriteLine("                                            4 - Questions");

        string input = Console.ReadLine();

        if (input == "1")
        {
            Orders();
        }
        else if (input == "2")
        {
            Products();
        }
        else if (input == "3")
        {
            Categories();
        }
        else if (input == "4")
        {
            Questions();
        }
        else
        {
            Start();
        }
    }
    private async void Questions()
    {
        Console.Clear();

        var questions = (await questionService.GetAllAsync(q => q.Progress == QuestionProgressType.Pending)).Value;

        if (questions.Count == 0)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            There is no any question. Press ENTER to continue.");
            Console.ReadLine();
            Start();
            return;
        }

        foreach (var que in questions)
        {
            Console.WriteLine($"                                            ID: {que.Id} || Question: {que.QuestionText}");
        }
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Answer question");
        Console.WriteLine("                                            2 - Back");

        string inp = Console.ReadLine();

        if (inp == "1")
        {
            Console.Write("                                            ID of the question: ");
            long queId = long.Parse(Console.ReadLine());
            Console.Write("                                            Answer: ");
            string answer = Console.ReadLine();

            var response = await questionService.AsnwerAsync(queId, answer);

            if (response.StatusCode == 200)
            {
                Console.WriteLine("                                            Successfully answered. Press ENTER to continue.");
            }
            else
            {
                Console.WriteLine("                                            Could not find. Press ENTER to continue.");
            }
            Console.ReadLine();
            Questions();
        }
        else if (inp == "2")
        {
            Start();
        }
        else
        {
            Questions();
        }
    }
    private async void Categories()
    {
        Console.Clear();

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Create category");
        Console.WriteLine("                                            2 - Delete category");
        Console.WriteLine("                                            3 - Get all categories information");
        Console.WriteLine("                                            4 - Back");

        var input = Console.ReadLine();

        if (input == "1")
        {
            Console.Clear();

            var catDto = new CategoryDto();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Name: ");
            catDto.Name = Console.ReadLine();
            Console.Write("                                            Description: ");
            catDto.Description = Console.ReadLine();

            var response = await categoryService.CreateAsync(catDto);

            Console.WriteLine("                                            " + response.Message + ". Press ENTER to continue.");
            Console.ReadLine();
            Categories();
        }
        else if (input == "2")
        {
            Console.Clear();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Enter ID of the category: ");
            long catId = long.Parse(Console.ReadLine());

            var response = await categoryService.DeleteAsync(catId);

            if (response.StatusCode == 200)
            {
                Console.WriteLine("                                            Successfully deleted. Press ENTER to continue.");
            }
            else
            {
                Console.WriteLine("                                            Could not find! Press ENTER to continue.");
            }
            Console.ReadLine();
            Categories();
        }
        else if (input == "3")
        {
            Console.Clear();

            var categories = (await categoryService.GetAllAsync(p => true)).Value;

            foreach (var cat in categories)
            {          
                Console.WriteLine($"         ID: {cat.Id} || Name: {cat.Name} || Description: {cat.Description} || Created at: {cat.CreatedAt}");
            }
            Console.WriteLine("                                            Press ENTER to continue.");
            Console.ReadLine();
            Categories();
        }
        else if (input == "4")
        {
            Start();
        }
        else
        {
            Categories();
        }
    }
    private async void Products()
    {
        Console.Clear();

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Create product");
        Console.WriteLine("                                            2 - Update product");
        Console.WriteLine("                                            3 - Delete product");
        Console.WriteLine("                                            4 - Get product information");
        Console.WriteLine("                                            5 - Get all products information");
        Console.WriteLine("                                            6 - Back");

        string input = Console.ReadLine();

        if (input == "1")
        {
            Console.Clear();

            var prodDto = new ProductDto();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Name: ");
            prodDto.Name = Console.ReadLine();
            Console.Write("                                            Description: ");
            prodDto.Description = Console.ReadLine();
            Console.Write("                                            Count: ");
            prodDto.Count = int.Parse(Console.ReadLine());
            Console.Write("                                            Price: ");
            prodDto.Price = decimal.Parse(Console.ReadLine());
            Console.Write("                                            Category ID: ");
            prodDto.CategoryId = long.Parse(Console.ReadLine());
            Console.Write("                                            Search tags: ");
            prodDto.SearchTags = Console.ReadLine().Split().ToList();

            var response = await productService.CreateAsync(prodDto);

            Console.WriteLine("                                            " + response.Message + ". Press ENTER to continue.");
            Console.ReadLine();
            Products();
        }
        else if (input == "2")
        {
            Console.Clear();

            var prodDto = new ProductDto();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            ID of the product: ");
            var prodId = long.Parse(Console.ReadLine());
            Console.Write("                                            Name: ");
            prodDto.Name = Console.ReadLine();
            Console.Write("                                            Description: ");
            prodDto.Description = Console.ReadLine();
            Console.Write("                                            Count: ");
            prodDto.Count = int.Parse(Console.ReadLine());
            Console.Write("                                            Price: ");
            prodDto.Price = decimal.Parse(Console.ReadLine());
            Console.Write("                                            Category ID: ");
            prodDto.CategoryId = long.Parse(Console.ReadLine());
            Console.Write("                                            Search tags: ");
            prodDto.SearchTags = Console.ReadLine().Split().ToList();

            var response = await productService.UpdateAsync(prodId, prodDto);

            Console.WriteLine("                                            " + response.Message + ". Press ENTER to continue.");
            Console.ReadLine();
            Products();
        }
        else if (input == "3")
        {
            Console.Clear();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Enter ID of the product: ");
            long prodId = long.Parse(Console.ReadLine());

            var response = await productService.DeleteAsync(prodId);

            if (response.StatusCode == 200)
            {
                Console.WriteLine("                                            Successfully deleted. Press ENTER to continue.");
            }
            else
            {
                Console.WriteLine("                                            Could not find! Press ENTER to continue.");
            }
            Console.ReadLine();
            Products();
        }
        else if (input == "4")
        {
            Console.Clear();

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            Enter ID of the product: ");
            long prodId = long.Parse(Console.ReadLine());

            var response = await productService.GetAsync(prodId);

            if (response.StatusCode == 200)
            {
                var prod = response.Value;

                Console.WriteLine("                                            Name: " + prod.Name);
                Console.WriteLine("                                            Description: " + prod.Description);
                Console.WriteLine("                                            Price: " + prod.Price);
                Console.WriteLine("                                            Count: " + prod.Count);
                Console.WriteLine("                                            Category ID: " + prod.CategoryId);
                Console.WriteLine("                                            Created at: " + prod.CreatedAt);
                Console.WriteLine("                                            Updated at: " + prod.UpdatedAt);
                Console.Write("                                            Search tags: ");
                prod.SearchTags.ForEach(p => Console.Write(p + " "));
                Console.WriteLine();

                Console.WriteLine("                                            Press ENTER to continue.");
                Console.ReadLine();
                Products();
            }
            else
            {
                Console.WriteLine("                                            Counld not find. Press ENTER to continue.");
                Console.ReadLine();
                Products();
            }
        }
        else if (input == "5")
        {
            Console.Clear();

            var products = (await productService.GetAllAsync(p => true)).Value;

            foreach (var prod in products)
            {
                Console.WriteLine($"                                  ID: {prod.Id} || Name: {prod.Name} || Price: {prod.Price} || Count: {prod.Count}");
                Console.WriteLine($"                                  Category ID: {prod.CategoryId} || Description: {prod.Description}");
                Console.WriteLine($"                                  Created at: {prod.CreatedAt} || Updated at: {prod.UpdatedAt}\n");
            }
            Console.WriteLine("                                            Press ENTER to continue.");
            Console.ReadLine();
            Products();
        }
        else if (input == "6")
        {
            Start();
        }
        else
        {
            Products();
        }
    }
    private async void Orders()
    {
        Console.Clear();

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            1 - Pending orders");
        Console.WriteLine("                                            2 - Processing orders");
        Console.WriteLine("                                            3 - Back");

        string input = Console.ReadLine();

        if (input == "1")
        {
        flag:
            var orders = (await orderService.GetAllAsync(o => o.Progress == OrderProgressType.Pending)).Value;
            Console.Clear();

            if (orders.Count == 0)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            There is no any pending orders. Press ENTER to continue.");
                Console.ReadLine();
                Orders();
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"                                  ID: {order.Id} || Address: {order.Address} || Ordered time: {order.CreatedAt}");

                var products = order.Items;

                Console.WriteLine("                                            Ordered products: " + products.Count);

                for (int i = 0; i < order.Items.Count; i++)
                {
                    Console.WriteLine($"                                  {i + 1}: Product ID: {products[i].Id} || Name: {products[i].Name} || Count: {products[i].Count}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("                                            1 - Move to processing");
            Console.WriteLine("                                            2 - Back");

            string inp = Console.ReadLine();

            if (inp == "1")
            {
                Console.Write("                                            Enter ID of the order: ");
                long inpId = long.Parse(Console.ReadLine());

                var response = await orderService.UpdateToNextProcessAsync(inpId);

                Console.WriteLine("                                            " + $"{response.Message}. Press ENTER to continue.");
                Console.ReadLine();
                goto flag;

            }
            else if (inp == "2")
            {
                Orders();
            }
            else
            {
                goto flag;
            }
        }
        else if (input == "2")
        {
        flag:
            var orders = (await orderService.GetAllAsync(o => o.Progress == OrderProgressType.Processing)).Value;
            Console.Clear();

            if (orders.Count == 0)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n                                            There is no any processing orders. Press ENTER to continue.");
                Console.ReadLine();
                Orders();
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"                                  ID: {order.Id} || Address: {order.Address} || Ordered time: {order.CreatedAt}");

                var products = order.Items;

                Console.WriteLine("                                            Ordered products: " + products.Count);

                for (int i = 0; i < order.Items.Count; i++)
                {
                    Console.WriteLine($"                                  {i + 1}: Product ID: {products[i].Id} || Name: {products[i].Name} || Count: {products[i].Count}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("                                            1 - Move to delivered");
            Console.WriteLine("                                            2 - Back");

            string inp = Console.ReadLine();

            if (inp == "1")
            {
                Console.Write("                                            Enter ID of the order: ");
                long inpId = long.Parse(Console.ReadLine());

                var response = await orderService.UpdateToNextProcessAsync(inpId);

                Console.WriteLine($"                                            {response.Message}. Press ENTER to continue.");
                Console.ReadLine();
                goto flag;

            }
            else if (inp == "2")
            {
                Orders();
            }
            else
            {
                goto flag;
            }
        }
        else if (input == "3")
        {
            Start();
        }
        else
        {
            Orders();
        }
    }
}