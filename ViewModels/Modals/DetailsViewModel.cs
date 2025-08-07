using CommunityToolkit.Maui.Storage;
using SkiaSharp;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Shortee.ViewModels.Modals;

[QueryProperty(nameof(ShortURL), "ShortURL")]
public partial class DetailsViewModel : BaseViewModel
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ShortURLModel _shortURL = null!;
    
    public DetailsViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    [RelayCommand]
    async Task DeleteHistoryItemAsync()
    {
        try
        {
            if(await Shell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete this URL?", "Delete", "Cancel") is not true)
                return;

            if(await _dataService.ShortURLRepo.DeleteAsync(_shortURL) > 0)
            {
                await Shell.Current.DisplayAlert("Success", "URL deleted successfully.", "OK");
                await Shell.Current.GoToAsync(".."); // Navigate back
            }
            else
                await Shell.Current.DisplayAlert("Error", "Failed to delete URL.", "OK");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting history item: {e.Message}"); 
            throw;
        }
    }

    [RelayCommand]
    async Task DownloadQRCodeAsync() 
    {
        try
        {
            if (ShortURL is null || string.IsNullOrEmpty(ShortURL.ShortenedUrl))
                return;

            await SaveQrCodeToDownloadsAsync(ShortURL.ShortenedUrl);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [RelayCommand]
    async Task CopyToClipboardAsync()
    {
        if (string.IsNullOrWhiteSpace(ShortURL.ShortenedUrl))
        {
            await Shell.Current.DisplayAlert("Error", "Short URL is empty.", "OK");
            return;
        }
        await Clipboard.SetTextAsync(ShortURL.ShortenedUrl);
        await Shell.Current.DisplayAlert("Success", "Short URL copied to clipboard.", "OK");
    }

    private byte[] GenerateQrCodePng(string value, int size = 512)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = size,
                Width = size,
                Margin = 1
            }
        };

        var pixelData = writer.Write(value);

        // Create SKBitmap from pixel data
        using var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        bitmap.Pixels = pixelData.Pixels
            .Chunk(4)
            .Select(c => new SKColor(c[2], c[1], c[0], c[3])) // BGRA to RGBA
            .ToArray();

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private async Task SaveQrCodeToDownloadsAsync(string value)
    {
        var pngBytes = GenerateQrCodePng(value);
        var fileName = $"QRCode_{DateTime.Now:yyyyMMddHHmmss}.png";
        using var stream = new MemoryStream(pngBytes);

        var result = await FileSaver.Default.SaveAsync(fileName, stream, new CancellationToken());
        
        if (result is null)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to save QR code.", "OK");
            return;
        }
        await Shell.Current.DisplayAlert("Success", $"QR code saved to {result.FilePath}", "OK");
    }
}
