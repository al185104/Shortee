namespace Shortee
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute($"{nameof(HistoryPage)}/{nameof(DetailsPage)}", typeof(DetailsPage));
            Routing.RegisterRoute($"{nameof(HomePage)}/{nameof(SettingsPage)}", typeof(SettingsPage));
        }
    }
}
