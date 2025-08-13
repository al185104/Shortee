
using System;

namespace Shortee.ViewModels;

public partial class ScanQRViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    public ScanQRViewModel(IDataService dataService)
    {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }

    [RelayCommand]
    async Task BarcodeScannedAsync(string shortUrl)
    {
		try
		{
            if (string.IsNullOrEmpty(shortUrl)) return;

            var entity = await _dataService.ShortURLRepo.GetByShortenedUrlAsync(shortUrl);
            if (entity == null) return;

            // update the entity
            entity.ClickCount++;

            await _dataService.ShortURLRepo.UpdateAsync(entity);

            // open phone browser browsing the long url
            await Launcher.OpenAsync(entity.OriginalUrl);

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

		}
		catch (Exception)
		{
			throw;
		}
    }
}
