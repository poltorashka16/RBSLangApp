using RBSLangApp.Models;

namespace RBSLangApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        string login = LoginEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Ошибка", "Введите логин и пароль.", "OK");
            return;
        }

        await App.Database.InitAsync();
        User? user = await App.Database.GetUserAsync(login, password);

        if (user is null)
        {
            await DisplayAlert("Ошибка", "Неверный логин или пароль.", "OK");
            return;
        }

        AppState.CurrentUserLogin = user.Login;
        AppState.CurrentUserName = user.FullName;

        Application.Current!.MainPage = new AppShell();
    }
}
