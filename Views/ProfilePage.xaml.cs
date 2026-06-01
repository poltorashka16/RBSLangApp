namespace RBSLangApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = AppState.CurrentUserName;
        UserLoginLabel.Text = $"Логин: {AppState.CurrentUserLogin}";
        FavoritesCountLabel.Text = (await App.Database.GetFavoriteCountAsync(AppState.CurrentUserLogin)).ToString();
        AverageResultLabel.Text = $"{await App.Database.GetAveragePercentAsync(AppState.CurrentUserLogin):F0}%";
    }

    private async void OpenFavorites_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavoritesPage));
    }

    private async void OpenProgress_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProgressPage));
    }

    private void Logout_Clicked(object sender, EventArgs e)
    {
        AppState.Clear();

        Application.Current!.MainPage = new NavigationPage(new LoginPage())
        {
            BarBackgroundColor = Color.FromArgb("#141217"),
            BarTextColor = Colors.White
        };
    }
}
