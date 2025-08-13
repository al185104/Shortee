
namespace Shortee.Services;

public interface IScannerService : IRepository<ShortURLModel>
{
    Task<ShortURLModel> GetByShortenedUrlAsync(string shortenedUrl);
}
