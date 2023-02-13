namespace TheBazaar.Data.IRepositories;

public interface IGenericRepo<TEntity>
{
    Task<TEntity> CreateAsync(TEntity model);
    Task<TEntity> UpdateAsync(TEntity model);
    Task<bool> DeleteAsync(long id);
    Task<TEntity> GetAsync(long id);
    Task<List<TEntity>> GetAllAsync();
}
