using Microsoft.Maui.ApplicationModel.Communication;

namespace Shortee.Services;

public class ScannerService : Repository<ShortURLModel>, IScannerService
{
    public ScannerService(string dbPath) : base(dbPath)
    {
    }

    public async Task<ShortURLModel> GetByShortenedUrlAsync(string shortenedUrl)
    {
        //return _database.FindAsync<ShortURLModel>(i => i.ShortenedUrl.Equals(shortenedUrl, StringComparison.InvariantCulture));

        return await _database.Table<ShortURLModel>()
                       .Where(u => u.ShortenedUrl == shortenedUrl)
                       .FirstOrDefaultAsync();
    }
}
