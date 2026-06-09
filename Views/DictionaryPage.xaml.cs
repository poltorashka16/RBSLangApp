using Microsoft.Maui.Media;
using RBSLangApp.Models;

namespace RBSLangApp.Views;

public partial class DictionaryPage : ContentPage
{
    private CancellationTokenSource? _searchCancellationToken;

    public DictionaryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAllTermsAsync();
    }

    private async Task LoadAllTermsAsync()
    {
        try
        {
            var terms = await App.Database.SearchTermsAsync(string.Empty);
            TermsCollection.ItemsSource = terms;

            System.Diagnostics.Debug.WriteLine($"Загружено терминов: {terms.Count}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить слова: {ex.Message}", "OK");
        }
    }

    private async void SearchBarTerms_TextChanged(object sender, TextChangedEventArgs e)
    {

        _searchCancellationToken?.Cancel();
        _searchCancellationToken = new CancellationTokenSource();
        var token = _searchCancellationToken.Token;

        try
        {

            await Task.Delay(300, token);

            if (token.IsCancellationRequested)
                return;

            var searchText = e.NewTextValue ?? string.Empty;

            System.Diagnostics.Debug.WriteLine($"Поиск: '{searchText}'");

            var results = await App.Database.SearchTermsAsync(searchText);

            if (!token.IsCancellationRequested)
            {
                TermsCollection.ItemsSource = results;
                System.Diagnostics.Debug.WriteLine($"Найдено: {results.Count}");
            }
        }
        catch (TaskCanceledException)
        {

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка поиска: {ex.Message}");
        }
    }

    private async void SpeakButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Term term)
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

    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Term term)
            return;

        try
        {
            if (string.IsNullOrWhiteSpace(AppState.CurrentUserLogin))
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован.", "OK");
                return;
            }

            bool added = await App.Database.ToggleFavoriteAsync(AppState.CurrentUserLogin, term);

            string text = added
                ? $"Слово \"{term.EnglishWord}\" добавлено в избранное."
                : $"Слово \"{term.EnglishWord}\" удалено из избранного.";

            await DisplayAlert("Избранное", text, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось обновить избранное: {ex.Message}", "OK");
        }
    }
}