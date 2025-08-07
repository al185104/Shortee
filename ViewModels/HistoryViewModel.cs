namespace Shortee.ViewModels;

public partial class HistoryViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<ShortURLModel> _history = new();

    public HistoryViewModel(IDataService dataService)
    {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }

    [RelayCommand]
    public async Task LoadHistoryAsync()
    {
        try
        {
            var historyItems = await _dataService.ShortURLRepo.GetAllAsync();
            History = new ObservableCollection<ShortURLModel>(historyItems);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load history: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task SelectedHistoryItemAsync(ShortURLModel item)
    {
        if (item == null) return;

        // go to details page
        var parameters = new Dictionary<string, object>
        {
            { "ShortURL", item }
        };
        await Shell.Current.GoToAsync(nameof(DetailsPage), parameters);
    }
}
