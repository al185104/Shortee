using ZXing.Net.Maui.Controls;

namespace Shortee.Views;

public partial class ScanQRPage : ContentPage
{

    bool _handling;
    DateTime _lastHandledUtc;
    readonly TimeSpan _cooldown = TimeSpan.FromSeconds(1);

    public ScanQRPage(ScanQRViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    private async void qrReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (_handling) return;

        var value = e.Results?.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(value)) return;

        // simple cooldown to avoid rapid duplicates
        var now = DateTime.UtcNow;
        if (now - _lastHandledUtc < _cooldown) return;

        _handling = true;
        _lastHandledUtc = now;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var reader = (CameraBarcodeReaderView)sender;
                reader.IsDetecting = false; // pause further events

                if (BindingContext is ScanQRViewModel vm)
                {
                    var result = e.Results.FirstOrDefault();
                    if (result != null)
                        await vm.BarcodeScannedCommand.ExecuteAsync(result.Value);
                }
            }
            finally
            {
                // short delay so camera settles, then resume
                await Task.Delay(500);
                ((CameraBarcodeReaderView)sender).IsDetecting = true;
                _handling = false;
            }
        });


    }

    //protected override async void OnAppearing()
    //{
    //    if (BindingContext is ScanQRViewModel vm)
    //    {
    //        await vm.BarcodeScannedCommand.ExecuteAsync("https://short.ee/lIjlPQ");
    //    }
    //}
}