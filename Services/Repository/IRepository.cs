namespace Shortee.Services.Repository;

public interface IRepository<T> where T : class 
{
    //GetAllAsync
    Task<List<T>> GetAllAsync(int? skip = null, int? take = null, DateOnly? start = null, DateOnly? end = null);
    Task<T> GetByIdAsync(Guid id);
    Task<int> InsertAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
}
