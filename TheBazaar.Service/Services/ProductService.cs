using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services;

public class ProductService : IProductService
{
    private IGenericRepo<Product> genericRepo;
    private ICategoryService categoryService;
    private IGenericRepo<Order> orderRepo;
    private ICartService cartService;
    public ProductService()
    {
        genericRepo = new GenericRepo<Product>();
        categoryService = new CategoryService();
        cartService = new CartService();
        orderRepo = new GenericRepo<Order>();
    }
    public async Task<GenericResponse<List<Product>>> RecommendationsAsync(User user)
    {
        var keys = new List<string>();
        var result = new List<Product>();

        foreach (var i in await orderRepo.GetAllAsync(o => o.UserId == user.Id))
        {
            foreach (var j in i.Items)
            {
                keys.Add(j.Name);
                foreach (var k in j.SearchTags)
                {
                    keys.Add(k);
                }
            }
        }

        foreach (var i in (await cartService.GetAsync(user.Id)).Value.Items)
        {
            keys.Add(i.Name);
        }

        foreach (var pro in (await GetAllAsync(p => true)).Value)
        {

            foreach (var key in keys)
            {
                if (result.Exists(p => p == pro))
                    break;
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
        var products = (await GetAllAsync(p => true)).Value;

        var result = new List<Product>();

        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (!product.Name.ToLower().Contains(name.ToLower()) && !CheckTags(product, name))
                    continue;
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                if ((await categoryService.GetAllAsync(p => p.Name.ToLower().Contains(categoryName.ToLower()))).Value.FirstOrDefault() is null)
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
        var model = (await this.genericRepo.GetAllAsync(x => x.Name == product.Name)).FirstOrDefault();
        var category = (await categoryService.GetAsync(product.CategoryId)).Value;

        if (model is not null || category is null)
        {
            return new GenericResponse<Product>
            {
                StatusCode = 405,
                Message = "This product is already exists",
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
            CategoryId = product.CategoryId,
            CreatedAt = DateTime.Now
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

    public async Task<GenericResponse<List<Product>>> GetAllAsync(Predicate<Product> predicate)
    {
        var models = await genericRepo.GetAllAsync(predicate);

        return new GenericResponse<List<Product>>
        {
            StatusCode = 200,
            Message = "Success",
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
