using SQLite;

namespace Shortee.Models;

public class ShortURLModel : BaseModel
{
    public string OriginalUrl { get; set; } = string.Empty;
    [Unique]
    public string ShortenedUrl { get; set; } = string.Empty;
    public int ClickCount { get; set; } = 0;
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddDays(30);
    public bool IsActive { get; set; } = true;
}
