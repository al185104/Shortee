using Shortee.ViewModels.Modals;

namespace Shortee.Views.Modals;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}