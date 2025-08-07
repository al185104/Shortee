using SQLite;

namespace Shortee.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IUrlShortenerService _urlService;
    private readonly IDataService _dataService;

    [ObservableProperty]
    string? _longURL = null;

    [ObservableProperty]
    string? _shortURL = null;

    public HomeViewModel(IUrlShortenerService urlService, IDataService dataService)
    {
        _urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }

    [RelayCommand]
    async Task ShortenAsync()
    {
        if (string.IsNullOrWhiteSpace(LongURL))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a valid URL.", "OK");
            return;
        }
        try
        {
            ShortURL = _urlService.ShortenUrl(LongURL, 6);
            
            await _dataService.ShortURLRepo.InsertAsync(new ShortURLModel
            {
                Id = Guid.NewGuid(),
                OriginalUrl = LongURL,
                ShortenedUrl = ShortURL,
                ClickCount = 0,
            });
        }
        catch (SQLiteException ex) when (ex.Result == SQLite3.Result.Constraint)
        {
            await Shell.Current.DisplayAlert("Error", "This URL has already been shortened.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to shorten URL: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task CopyToClipboardAsync()
    {
        if (string.IsNullOrWhiteSpace(ShortURL))
        {
            await Shell.Current.DisplayAlert("Error", "Short URL is empty.", "OK");
            return;
        }
        await Clipboard.SetTextAsync(ShortURL);
        await Shell.Current.DisplayAlert("Success", "Short URL copied to clipboard.", "OK");
    }

}
