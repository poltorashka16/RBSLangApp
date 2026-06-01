using RBSLangApp.Models;

namespace RBSLangApp.Views;

[QueryProperty(nameof(LessonId), "lessonId")]
public partial class LessonTestsPage : ContentPage
{
    private int _lessonId;
    private List<TestQuestion> _questions = new();
    private int _index = 0;
    private int _score = 0;

    public string LessonId
    {
        set
        {
            if (int.TryParse(value, out int id))
                _lessonId = id;
        }
    }

    public LessonTestsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _questions = await App.Database.GetQuestionsByLessonAsync(_lessonId);
        _index = 0;
        _score = 0;

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (_questions.Count == 0)
        {
            QuestionLabel.Text = "Нет вопросов";
            return;
        }

        if (_index >= _questions.Count)
        {
            QuestionLabel.Text = $"Результат: {_score}/{_questions.Count}";
            Option1.IsVisible = false;
            Option2.IsVisible = false;
            Option3.IsVisible = false;
            Option4.IsVisible = false;
            ResultLabel.Text = "Тест завершён 🎉";
            return;
        }

        var q = _questions[_index];

        QuestionLabel.Text = q.Question;
        Option1.Text = q.Option1;
        Option2.Text = q.Option2;
        Option3.Text = q.Option3;
        Option4.Text = q.Option4;
    }

    private async void Answer_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        var q = _questions[_index];

        if (btn.Text == q.CorrectAnswer)
        {
            _score++;
            ResultLabel.Text = "Правильно ✅";
        }
        else
        {
            ResultLabel.Text = $"Неверно ❌ (Правильно: {q.CorrectAnswer})";
        }

        _index++;
        await Task.Delay(800);
        ShowQuestion();
    }

    private async void Restart_Clicked(object sender, EventArgs e)
    {
        _index = 0;
        _score = 0;
        ResultLabel.Text = "";
        Option1.IsVisible = true;
        Option2.IsVisible = true;
        Option3.IsVisible = true;
        Option4.IsVisible = true;

        ShowQuestion();
    }
}