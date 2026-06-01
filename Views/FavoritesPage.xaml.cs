namespace RBSLangApp.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        FavoritesCollection.ItemsSource = await App.Database.GetFavoritesAsync(AppState.CurrentUserLogin);
    }
}
