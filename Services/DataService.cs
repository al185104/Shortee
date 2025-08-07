namespace Shortee.Services;

public class DataService : IDataService
{
    private Repository<ShortURLModel> _shortURLRepo;

    public DataService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "shortee.db3");
        _shortURLRepo = new Repository<ShortURLModel>(dbPath);
    }

    public IRepository<ShortURLModel> ShortURLRepo => _shortURLRepo;
}
