using RBSLangApp.Models;

namespace RBSLangApp.Views;

public partial class LessonsPage : ContentPage
{
    public LessonsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LessonsCollection.ItemsSource = await App.Database.GetLessonsAsync();
    }

    private async void OpenLesson_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int lessonId)
        {
            await Shell.Current.GoToAsync($"{nameof(LessonDetailPage)}?lessonId={lessonId}");
            return;
        }

        if (sender is Button btn && int.TryParse(btn.CommandParameter?.ToString(), out int id))
        {
            await Shell.Current.GoToAsync($"{nameof(LessonDetailPage)}?lessonId={id}");
        }
    }
}