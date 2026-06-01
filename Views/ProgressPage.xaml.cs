namespace RBSLangApp.Views;

public partial class ProgressPage : ContentPage
{
    public ProgressPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ProgressCollection.ItemsSource = await App.Database.GetProgressAsync(AppState.CurrentUserLogin);
    }
}
