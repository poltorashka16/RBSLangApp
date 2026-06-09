using Microsoft.Maui.Media;
using RBSLangApp.Models;

namespace RBSLangApp.Views;

[QueryProperty(nameof(LessonId), "lessonId")]
public partial class LessonDetailPage : ContentPage
{
    private int _lessonId;
    private List<Term> _terms = new();
    private int _currentIndex = 0;
    private bool _isFlipped = false;
    private bool _loaded = false;


    public string LessonId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                _lessonId = id;
                System.Diagnostics.Debug.WriteLine($"LessonId received = {id}");
            }
        }
    }

    public LessonDetailPage()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        if (_loaded)
            return;

        if (_lessonId <= 0)
        {
            System.Diagnostics.Debug.WriteLine("LessonId is invalid");
            return;
        }

        _loaded = true;

        var lesson = await App.Database.GetLessonAsync(_lessonId);

        if (lesson != null)
        {
            LessonTitleLabel.Text = lesson.Title;
            LessonSubtitleLabel.Text = lesson.Subtitle;
        }

        _terms = await App.Database.GetTermsByLessonAsync(_lessonId);

        System.Diagnostics.Debug.WriteLine($"Terms loaded: {_terms.Count}");

        _currentIndex = 0;
        _isFlipped = false;

        ShowCurrentCard();
    }


    private void ShowCurrentCard()
    {
        if (_terms == null || _terms.Count == 0)
        {
            CounterLabel.Text = "0 / 0";
            CardMainTextLabel.Text = "Слова не найдены";
            CardExampleLabel.IsVisible = false;
            return;
        }

        if (_currentIndex < 0)
            _currentIndex = 0;

        if (_currentIndex >= _terms.Count)
            _currentIndex = _terms.Count - 1;

        var term = _terms[_currentIndex];

        CounterLabel.Text = $"{_currentIndex + 1} / {_terms.Count}";

        if (_isFlipped)
        {
            CardMainTextLabel.Text = term.Translation;
            CardMainTextLabel.TextColor = Color.FromArgb("#1CB0F6");
            CardFrame.BackgroundColor = Color.FromArgb("#E8F4FF");
            CardExampleLabel.Text = term.Example;
            CardExampleLabel.IsVisible = true;
        }
        else
        {
            CardMainTextLabel.Text = term.EnglishWord;
            CardMainTextLabel.TextColor = Color.FromArgb("#3C3C3C");
            CardFrame.BackgroundColor = Color.FromArgb("#F0FAE8");
            CardExampleLabel.IsVisible = false;
        }
    }


    private async Task AnimateCardFlip()
    {
        await CardFrame.ScaleTo(0.92, 90);
        await CardFrame.ScaleTo(1.0, 90);
    }

    private async void FlipCard_Clicked(object sender, EventArgs e)
    {
        if (_terms.Count == 0)
            return;

        _isFlipped = !_isFlipped;
        await AnimateCardFlip();
        ShowCurrentCard();
    }


    private async void PrevCard_Clicked(object sender, EventArgs e)
    {
        if (_terms.Count == 0)
            return;

        if (_currentIndex > 0)
        {
            _currentIndex--;
            _isFlipped = false;
            await AnimateCardFlip();
            ShowCurrentCard();
        }
    }


    private async void NextCard_Clicked(object sender, EventArgs e)
    {
        if (_terms.Count == 0)
            return;

        if (_currentIndex < _terms.Count - 1)
        {
            _currentIndex++;
            _isFlipped = false;
            await AnimateCardFlip();
            ShowCurrentCard();
        }
    }


    private async void SpeakButton_Clicked(object sender, EventArgs e)
    {
        if (_terms.Count == 0)
            return;

        var term = _terms[_currentIndex];

        try
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();

            string textToSpeak;
            Locale? locale = null;

            if (_isFlipped)
            {

                textToSpeak = term.Translation;
                locale = locales.FirstOrDefault(x =>
                    x.Language.StartsWith("ru", StringComparison.OrdinalIgnoreCase));

                if (locale == null)
                {
                    System.Diagnostics.Debug.WriteLine("Русская локаль не найдена, используется автоопределение языка");
                }
            }
            else
            {

                textToSpeak = term.EnglishWord;
                locale = locales.FirstOrDefault(x =>
                    x.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase));

                if (locale == null)
                {
                    System.Diagnostics.Debug.WriteLine("Английская локаль не найдена, используется автоопределение языка");
                }
            }

            var options = new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            };


            if (locale != null)
            {
                options.Locale = locale;
            }

            await TextToSpeech.Default.SpeakAsync(textToSpeak, options);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }


    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (_terms.Count == 0)
            return;

        string login = AppState.CurrentUserLogin;

        if (string.IsNullOrWhiteSpace(login))
        {
            await DisplayAlert("Ошибка", "Пользователь не авторизован.", "OK");
            return;
        }

        var term = _terms[_currentIndex];

        bool added = await App.Database.ToggleFavoriteAsync(login, term);

        await DisplayAlert(
            "Избранное",
            added
                ? $"Добавлено: {term.EnglishWord}"
                : $"Удалено: {term.EnglishWord}",
            "OK");
    }


    private async void StartTest_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LessonTestsPage)}?lessonId={_lessonId}");
    }
}