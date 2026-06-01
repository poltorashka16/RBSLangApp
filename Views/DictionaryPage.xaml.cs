using Microsoft.Maui.Media;
using RBSLangApp.Models;

namespace RBSLangApp.Views;

public partial class DictionaryPage : ContentPage
{
    public DictionaryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        TermsCollection.ItemsSource = await App.Database.SearchTermsAsync(string.Empty);
    }

    private async void SearchBarTerms_TextChanged(object sender, TextChangedEventArgs e)
    {
        TermsCollection.ItemsSource = await App.Database.SearchTermsAsync(e.NewTextValue ?? string.Empty);
    }

    private async void SpeakButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Term term)
            return;

        var locales = await TextToSpeech.Default.GetLocalesAsync();
        var english = locales.FirstOrDefault(x => x.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase));

        await TextToSpeech.Default.SpeakAsync(term.EnglishWord, new SpeechOptions
        {
            Locale = english,
            Pitch = 1.0f,
            Volume = 1.0f
        });
    }

    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Term term)
            return;

        bool added = await App.Database.ToggleFavoriteAsync(AppState.CurrentUserLogin, term);
        string text = added
            ? $"Слово \"{term.EnglishWord}\" добавлено в избранное."
            : $"Слово \"{term.EnglishWord}\" удалено из избранного.";

        await DisplayAlert("Избранное", text, "OK");
    }
}
