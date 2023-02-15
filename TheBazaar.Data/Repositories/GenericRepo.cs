using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TheBazaar.Data.Configurations;
using TheBazaar.Data.IRepositories;
using TheBazaar.Domain.Commons;
using TheBazaar.Domain.Entities;

namespace TheBazaar.Data.Repositories
{
#pragma warning disable
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : Auditable
    {
        private string Path;
        private long LastId = 0;
        
        public GenericRepo()
        {
            StartUp();
        }

        private async void StartUp()
        {
            if (typeof(TEntity) == typeof(Category))
            {
                Path = DatabasePaths.CATEGORY_PATH;
            }
            else if (typeof(TEntity) == typeof(User)) 
            {
                Path = DatabasePaths.USER_PATH;
            }
            else if (typeof(TEntity) == typeof(Order))
            {
                Path = DatabasePaths.ORDER_PATH;
            }
            else if (typeof(TEntity) == typeof(Question))
            {
                Path = DatabasePaths.QUESTION_PATH;
            }
            else if (typeof(TEntity) == typeof(Product))
            {
                Path = DatabasePaths.PRODUCT_PATH;
            }
            else if (typeof(TEntity) == typeof(Cart))
            {
                Path = DatabasePaths.CART_PATH;
            }

            foreach (var model in await GetAllAsync())
            {
                if (model.Id > LastId)
                    LastId = model.Id;
            }

        }

        public async Task<TEntity> CreateAsync(TEntity model)
        {
            model.Id = ++LastId;
            var models = await GetAllAsync();
            models.Add(model);

            File.WriteAllText(Path, JsonConvert.SerializeObject(models, Formatting.Indented));

            return model;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            List<TEntity> models = await GetAllAsync();
            var model = models.FirstOrDefault(x => x.Id == id);
            if (model is null)
            {
                return false;
            }

            models.Remove(model);

            File.WriteAllText(Path, JsonConvert.SerializeObject(models, Formatting.Indented));
            return true;
        }

        public async Task<List<TEntity>> GetAllAsync(Predicate<TEntity> predicate = null)
        {
            string text = File.ReadAllText(Path);
            if (string.IsNullOrEmpty(text))
            {
                text = "[]";
            }

            var result = JsonConvert.DeserializeObject<List<TEntity>>(text);

            if (predicate is null)
                return result;

            return result.FindAll(predicate);     
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return (await GetAllAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task<TEntity> UpdateAsync(TEntity model)
        {
            var models = await GetAllAsync();
            var updatingModel = models.FirstOrDefault(x => x.Id == model.Id);

            if (updatingModel == null) 
                return null;
            
            int index = models.IndexOf(updatingModel);

            models.Remove(updatingModel);

            model.CreatedAt = updatingModel.CreatedAt;
            models.Insert(index, model);

            File.WriteAllText(Path, JsonConvert.SerializeObject(models, Formatting.Indented));

            return model;
        }
    }
}
