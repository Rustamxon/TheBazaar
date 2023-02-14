using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
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
                Path = Configurations.DatabasePaths.CATEGORY_PATH;
            }
            else if (typeof(TEntity) == typeof(User)) 
            {
                Path = Configurations.DatabasePaths.USER_PATH;
            }
            else if (typeof(TEntity) == typeof(Order))
            {
                Path = Configurations.DatabasePaths.USER_PATH;
            }
            else if (typeof(TEntity) == typeof(Question))
            {
                Path = Configurations.DatabasePaths.QUESTION_PATH;
            }
            else if (typeof(TEntity) == typeof(Product))
            {
                Path = Configurations.DatabasePaths.PRODUCT_PATH;
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

        public async Task<List<TEntity>> GetAllAsync()
        {
            string text = File.ReadAllText(Path);
            if (string.IsNullOrEmpty(text))
            {
                text = "[]";
            }
            return JsonConvert.DeserializeObject<List<TEntity>>(text);     
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return (await GetAllAsync()).FirstOrDefault(x => x.Id == id);
        }

        public async Task<TEntity> UpdateAsync(TEntity model)
        {
            var models = await GetAllAsync();
            var UpdatingModel = models.FirstOrDefault(x => x.Id == model.Id);

            if (UpdatingModel == null) 
                return null;
            
            int index = models.IndexOf(model);

            models.Remove(UpdatingModel);

            model.CreatedAt = UpdatingModel.CreatedAt;
            model.UpdatedAt = UpdatingModel.UpdatedAt;
            models.Insert(index, model);

            File.WriteAllText(Path, JsonConvert.SerializeObject(models, Formatting.Indented));

            return model;
        }
    }
}
