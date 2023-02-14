using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services;

public class ProductService : IProductService
{
    private IGenericRepo<Product> genericRepo = new GenericRepo<Product>();
    private ICategoryService categoryService = new CategoryService();
    private IOrderService orderService = new OrderService();
    private ICartService cartService = new CartService();

    public async Task<GenericResponse<List<Product>>> RecommendationsAsync(User user)
    {
        var keys = new List<string>();
        var result = new List<Product>();

        foreach (var i in (await orderService.GetUsersAllOrdersAsync(user.Id)).Value)
        {
            foreach (var j in i.Items)
            {
                keys.Add(j.Name);
            }
        }

        foreach (var i in (await cartService.GetAsync(user.Id)).Value.Items)
        {
            keys.Add(i.Name);
        }

        foreach (var pro in (await GetAllAsync()).Value)
        {
            foreach (var key in keys)
            {
                if (pro.Name.ToLower().Contains(key.ToLower()) || CheckTags(pro, key))
                    result.Add(pro);
            }
        }

        return new GenericResponse<List<Product>>
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }
    private bool CheckTags(Product product, string key)
    {
        foreach (string tag in product.SearchTags)
        {
            if (tag.ToLower().Contains(key.ToLower()))
            {
                return true;
            }
        }

        return false;
    }
    public async Task<GenericResponse<List<Product>>> SearchAsync
        (string name, string categoryName, string minPrice, string maxPrice)
    {
        var products = (await GetAllAsync()).Value;

        var result = new List<Product>();

        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (!product.Name.ToLower().Contains(name.ToLower()))
                    continue;

                if (!CheckTags(product, name))
                    continue;
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                var response = await categoryService.GetAsync(categoryName);

                if (!response.Value.Name.ToLower().Contains(categoryName) && response.StatusCode != 404)
                    continue;
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                if (product.Price < decimal.Parse(minPrice))
                    continue;
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                if (product.Price > decimal.Parse(minPrice))
                    continue;
            }

            result.Add(product);
        }

        return new GenericResponse<List<Product>>
        {
            StatusCode = 200,
            Message = "Success",
            Value = result
        };
    }
    public async Task<GenericResponse<Product>> CreateAsync(ProductDto product)
    {
        var models = await this.genericRepo.GetAllAsync();
        var model = models.FirstOrDefault(x => x.Name == product.Name);

        if (model is not null)
        {
            return new GenericResponse<Product>
            {
                StatusCode = 404,
                Message = "This category is already exist",
                Value = null,
            };
        }

        var mappedModel = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            SearchTags = product.SearchTags,
            Price = product.Price,
            Count = product.Count,
        };

        await genericRepo.CreateAsync(mappedModel);

        return new GenericResponse<Product>
        {
            StatusCode = 200,
            Message = "New product created",
            Value = mappedModel
        };
    }

    public async Task<GenericResponse<Product>> DeleteAsync(long id)
    {
        var models = await this.genericRepo.GetAllAsync();
        var model = models.FirstOrDefault(x => x.Id == id);

        if (model is null)
        {
            return new GenericResponse<Product>
            {
                StatusCode = 404,
                Message = "Not found",
                Value = null
            };
        }

        await this.genericRepo.DeleteAsync(model.Id);

        return new GenericResponse<Product>
        {
            StatusCode = 200,
            Message = "Successfully deleted",
            Value = model,
        };
    }

    public async Task<GenericResponse<List<Product>>> GetAllAsync()
    {
        var models = await genericRepo.GetAllAsync();
        if (models is null)
        {
            return new GenericResponse<List<Product>>
            {
                StatusCode = 404,
                Message = "Empty",
                Value = null,
            };
        }
        return new GenericResponse<List<Product>>
        {
            StatusCode = 200,
            Message = "Ok",
            Value = models,
        };
    }

    public async Task<GenericResponse<Product>> GetAsync(long id)
    {
        var model = await this.genericRepo.GetAsync(id);

        if (model is null)
        {
            return new GenericResponse<Product>
            {
                StatusCode = 404,
                Message = "Not found",
                Value = null,
            };
        }

        return new GenericResponse<Product>
        {
            StatusCode = 200,
            Message = "Ok )",
            Value = model,

        };
    }

    public async Task<GenericResponse<Product>> UpdateAsync(long id, ProductDto product)
    {
        var models = await this.genericRepo.GetAllAsync();
        var model = models.FirstOrDefault(x => x.Id == id);

        if (model is null)
        {
            return new GenericResponse<Product>
            {
                StatusCode = 404,
                Message = "Not found",
                Value = null,
            };
        }

        var mappedmodel = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            SearchTags = product.SearchTags,
            Price = product.Price,
            Count = product.Count,
            Id = model.Id,
            CreatedAt = model.CreatedAt,
            UpdatedAt = DateTime.Now,
        };

        await this.genericRepo.UpdateAsync(mappedmodel);

        return new GenericResponse<Product>
        {
            StatusCode = 200,
            Message = "Successfully updated",
            Value = mappedmodel,
        };
    }
}
