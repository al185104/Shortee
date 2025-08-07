namespace Shortee.ViewModels.Base;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool _isBusy = false;
}
