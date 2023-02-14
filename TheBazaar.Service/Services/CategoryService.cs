using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Data.IRepositories;
using TheBazaar.Domain.Entities;
using TheBazaar.Service.DTOs;
using TheBazaar.Service.Helpers;
using TheBazaar.Service.Interfaces;

namespace TheBazaar.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private IGenericRepo
        public Task<GenericResponse<Category>> CreateAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Category>> DeleteAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<List<Category>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Category>> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Category>> UpdateAsync(string name, CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
