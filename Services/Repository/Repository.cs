using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace Shortee.Services.Repository;

public class Repository<T> : IRepository<T> where T : class, new()
{
    private readonly SQLiteAsyncConnection _database;

    public Repository(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<T>().Wait();
    }

    public Task<int> DeleteAsync(T entity)
    {
        return _database.DeleteAsync(entity);
    }

    public async Task<List<T>> GetAllAsync(int? skip = null, int? take = null, DateOnly? start = null, DateOnly? end = null)
    {
        var allDataWithChildren = await _database.GetAllWithChildrenAsync<T>(recursive:true);

        if(start.HasValue || end.HasValue)
        {
            allDataWithChildren = allDataWithChildren
                .Where(item => { 
                    var property = item.GetType().GetProperty("CreatedDate"); 
                    if(property == null) return true; // Skip if no CreatedDate property

                    var createdDate = (DateOnly?)property.GetValue(item);

                    return (!start.HasValue || createdDate >= start.Value) &&
                           (!end.HasValue || createdDate <= end.Value);
                })
                .ToList();
        }

        // use skip and take for pagination
        if(skip.HasValue)
            allDataWithChildren = allDataWithChildren.Skip(skip.Value).ToList();
        if(take.HasValue)
            allDataWithChildren = allDataWithChildren.Take(take.Value).ToList();

        return allDataWithChildren;
    }

    public Task<T> GetByIdAsync(Guid id)
    {
        return _database.FindAsync<T>(id);
    }

    public Task<int> InsertAsync(T entity)
    {
        return _database.InsertAsync(entity);
    }

    public Task<int> UpdateAsync(T entity)
    {
        return _database.UpdateAsync(entity);
    }
}
