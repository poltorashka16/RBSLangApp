using Microsoft.Maui.Media;
using RBSLangApp.Models;

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
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(AppState.CurrentUserLogin))
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован.", "OK");
                return;
            }

            var favorites = await App.Database.GetFavoritesAsync(AppState.CurrentUserLogin);
            FavoritesCollection.ItemsSource = favorites;

            System.Diagnostics.Debug.WriteLine($"Загружено избранных слов: {favorites.Count}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить избранное: {ex.Message}", "OK");
        }
    }

    private async void SpeakButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not FavoriteTerm term)
            return;

        try
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            var english = locales.FirstOrDefault(x =>
                x.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase));

            var options = new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            };

            if (english != null)
            {
                options.Locale = english;
            }

            await TextToSpeech.Default.SpeakAsync(term.EnglishWord, options);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось озвучить: {ex.Message}", "OK");
        }
    }

    private async void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not FavoriteTerm term)
            return;

        try
        {
            if (string.IsNullOrWhiteSpace(AppState.CurrentUserLogin))
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован.", "OK");
                return;
            }

            bool confirm = await DisplayAlert(
                "Удалить из избранного",
                $"Удалить \"{term.EnglishWord}\" из избранного?",
                "Да", "Нет");

            if (!confirm)
                return;

            var termObj = new Term
            {
                Id = term.TermId,
                EnglishWord = term.EnglishWord,
                Translation = term.Translation,
                Example = term.Example
            };

            await App.Database.ToggleFavoriteAsync(AppState.CurrentUserLogin, termObj);
            await LoadFavoritesAsync(); 
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось удалить: {ex.Message}", "OK");
        }
    }
}