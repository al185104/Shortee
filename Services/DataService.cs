namespace Shortee.Services;

public class DataService : IDataService
{
    private ScannerService _shortURLRepo;

    public DataService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "shortee.db3");
        _shortURLRepo = new ScannerService(dbPath);
    }

    public IScannerService ShortURLRepo => _shortURLRepo;
}
