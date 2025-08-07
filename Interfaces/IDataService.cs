namespace Shortee.Interfaces;

public interface IDataService
{
    public IRepository<ShortURLModel> ShortURLRepo { get; }
}
