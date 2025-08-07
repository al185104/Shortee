using SQLite;

namespace Shortee.Models.Base;

public abstract class BaseModel
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}
