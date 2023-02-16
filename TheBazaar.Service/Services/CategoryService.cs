using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Data.IRepositories;
using TheBazaar.Data.Repositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private IGenericRepo<Category> genericRepo;
        public CategoryService()
        {
            genericRepo = new GenericRepo<Category>();
        }
        public async Task<GenericResponse<Category>> CreateAsync(CategoryDto categoryDto)
        {
            var models = await this.genericRepo.GetAllAsync();
            var model = models.FirstOrDefault(x => x.Name == categoryDto.Name);

            if (model is not null)
            {
                return new GenericResponse<Category>
                {
                    StatusCode = 404,
                    Message = "This category is already exists",
                    Value = null,
                };
            }

            var mappedModel = new Category()
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                CreatedAt = DateTime.Now
            };

            await genericRepo.CreateAsync(mappedModel);

            return new GenericResponse<Category>
            {
                StatusCode = 200,
                Message = "New category created",
                Value = mappedModel
            };
        }

        public async Task<GenericResponse<Category>> DeleteAsync(long id)
        {
            var model = await this.genericRepo.GetAsync(id);

            if (model is null)
            {
                return new GenericResponse<Category>
                {
                    StatusCode = 404,
                    Message = "Not found",
                    Value = null
                };
            }

            await this.genericRepo.DeleteAsync(model.Id);

            return new GenericResponse<Category>
            {
                StatusCode = 200,
                Message = "Successfully deleted )",
                Value = model,
            };
        }

        public async Task<GenericResponse<List<Category>>> GetAllAsync(Predicate<Category> predicate)
        {
            var models = await genericRepo.GetAllAsync(predicate);
            if (models is null)
            {
                return new GenericResponse<List<Category>>
                {
                    StatusCode = 404,
                    Message = "Empty",
                    Value = null,
                };
            }
            return new GenericResponse<List<Category>>
            {
                StatusCode = 200,
                Message = "Ok )",
                Value = models
            };
        }

        public async Task<GenericResponse<Category>> GetAsync(long id)
        {
            var model = await this.genericRepo.GetAsync(id);
            
            if (model is null)
            {
                return new GenericResponse<Category>
                {
                    StatusCode = 404,
                    Message = "Not found",
                    Value = null,
                };
            }

            return new GenericResponse<Category>
            {
                StatusCode = 200,
                Message = "Ok )",
                Value = model,

            };

        }

        public async Task<GenericResponse<Category>> UpdateAsync(long id, CategoryDto categoryDto)
        {
            var model = await this.genericRepo.GetAsync(id);

            if (model is null)
            {
                return new GenericResponse<Category>
                {
                    StatusCode = 404,
                    Message = "Not found",
                    Value = null,
                };
            }

            var mappedmodel = new Category()
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                Name = model.Name,
                Description = model.Description,
                UpdatedAt = DateTime.Now
            };

            var res = await this.genericRepo.UpdateAsync(mappedmodel);

            return new GenericResponse<Category>
            {
                StatusCode = 200,
                Message = "Successfully updated )",
                Value = mappedmodel,
            };
        }
    }
}
