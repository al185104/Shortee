namespace Shortee.Views;

public partial class HistoryPage : ContentPage
{
	public HistoryPage(HistoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is HistoryViewModel vm)
		{
			vm.LoadHistoryCommand.Execute(null);
		}
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (BindingContext is HistoryViewModel vm) { 
			if (e.CurrentSelection.FirstOrDefault() is ShortURLModel selectedItem)
			{
				vm.SelectedHistoryItemCommand.Execute(selectedItem);
			}
			// Clear selection
			((CollectionView)sender).SelectedItem = null;
        }
    }
}