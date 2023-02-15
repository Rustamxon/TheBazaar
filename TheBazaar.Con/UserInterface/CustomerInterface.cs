﻿using System.Data;
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
            Recommendation();
        }
        else if (input == "3")
        {
            MyCart();
        }
        else if (input == "4")
        {
            Question();
        }
        else
        {
            Start();
        }
    }
    private async void Question()
    {
        Console.Clear();
        Console.WriteLine("1 - Give question");
        Console.WriteLine("2 - Answered questions");
        Console.WriteLine("3 - Not answered question");
        Console.WriteLine("4 - Back");

        string input1 = Console.ReadLine();

        Console.Clear();
        if (input1 == "1")
        {
            Console.Write("Your question: ");
            string questionText = Console.ReadLine();

            await questionService.CreateAsync(new QuestionDto
            {
                QuestionText = questionText,
                UserId = customer.Id
            });

            Console.WriteLine("You will get your answer ASAP. Press ENTER to continue.");
            Console.ReadLine();
            Question();
        }
        else if (input1 == "2")
        {
            var questions = (await questionService.GetAllUserQuestionsAsync(customer.Id)).Value.
                FindAll(q => q.Progress == QuestionProgressType.Answered);

            foreach (var que in questions)
            {
                Console.WriteLine($"Question: {que.QuestionText}");
                Console.WriteLine($"Answer: {que.AnswerText}\n");
            }
            Console.WriteLine("Press ENTER to continue.");
            Console.ReadLine();
            Question();
        }
        else if (input1 == "3")
        {
            var questions = (await questionService.GetAllUserQuestionsAsync(customer.Id)).Value.
                FindAll(q => q.Progress == QuestionProgressType.Pending);

            foreach (var que in questions)
            {
                Console.WriteLine($"Question: {que.QuestionText}");
                Console.WriteLine($"Written time: {que.CreatedAt}\n");
            }
            Console.WriteLine("Press ENTER to continue.");
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
        var cart = (await cartService.GetAsync(customer.Id)).Value;
        var products = cart.Items;

        if (products.Count == 0)
        {
            Console.WriteLine("Your cart is empty. Press ENTER to continue.");
            Console.ReadLine();
            Start();
            return;
        }

        Console.WriteLine("1 - All products in the cart");
        Console.WriteLine("2 - Make order");
        Console.WriteLine("3 - Back");

        var input = Console.ReadLine();

        if (input == "1")
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("1 - Remove product");
                Console.WriteLine("2 - Change the count");
                Console.WriteLine("3 - Back");
                string input2 = Console.ReadLine();

                Console.Clear();
                if (input2 == "1")
                {
                    Console.Write("Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count)
                    {
                        Console.WriteLine("Something is wrong! Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        var p = products[placeNumber - 1];

                        cart.Items.Remove(p);

                        await cartService.UpdateAsync(cart);

                        Console.WriteLine("Removed successfully. Press ENTER to continue.");
                        Console.ReadLine();
                    }
                }
                else if (input2 == "2")
                {
                    Console.Write("Product place number: ");
                    int placeNumber = int.Parse(Console.ReadLine());
                    Console.Write("Count: ");
                    int count = int.Parse(Console.ReadLine());

                    if (placeNumber < 1 || placeNumber > products.Count)
                    {
                        Console.WriteLine("Place number is wrong! Press ENTER to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        var productLive = (await productService.GetAsync(products[placeNumber - 1].Id)).Value;
                        if (productLive.Count < count)
                        {
                            Console.WriteLine("No enough product! Press ENTER to continue.");
                            Console.ReadLine();
                        }
                        else
                        {
                            var p = products[placeNumber - 1];

                            p.Count = count;

                            await cartService.UpdateAsync(cart);

                            Console.WriteLine("Changed successfully. Press ENTER to continue.");
                            Console.ReadLine();
                        }
                    }
                }
                else if (input2 == "3")
                {
                    Start();
                    break;
                }
            }
        }
        else if (input == "2")
        {
            var totalPrice = (await cartService.GetTotalPriceAsync(cart));
            Console.WriteLine($"Total price with deleviring: {totalPrice}");
            Console.WriteLine("Y/y to pay: ");
            string payInput = Console.ReadLine();

            if (payInput.ToLower() == "y")
            {
                Console.Clear();
                Console.WriteLine("1 - Cash");
                Console.WriteLine("2 - Payme");
                Console.WriteLine("3 - Click");
                Console.WriteLine("4 - UzCard");
                var payType = (PaymentType)(byte.Parse(Console.ReadLine()) * 10);
                Console.WriteLine("Your address: ");
                string address = Console.ReadLine();

                var response = await orderService.CreateAsync(new OrderDto
                {
                    Address = address,
                    PaymentType = payType,
                    Progress = OrderProgressType.Pending,
                    UserId = customer.Id,
                    Items = cart.Items
                });
                Console.WriteLine("Your order is accepted. Press ENTER to continue.");
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
            Console.WriteLine("Could not find anything. Press ENTER to continue.");
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
                    Console.WriteLine($"Place number: {i + 1} || Name: {products[i].Name} || Price: {products[i].Price}");
                    Console.WriteLine($"Amount: {products[i].Count} || Description: {products[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("1 - Add product to my cart");
                Console.WriteLine("2 - Back to the menu");
                string input = Console.ReadLine();

                Console.Clear();
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
}