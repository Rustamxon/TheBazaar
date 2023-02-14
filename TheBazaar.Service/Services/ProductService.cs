using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class ProductService : IProductService
    {
        private IGenericRepo<Product> genericRepo = new GenericRepo<Product>();

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

        public async Task<GenericResponse<Product>> GetAsync(string name)
        {
            var models = await this.genericRepo.GetAllAsync();
            var model = models.FirstOrDefault(c => c.Name == name);

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
}
