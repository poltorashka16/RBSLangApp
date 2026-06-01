using RBSLangApp.Views;

namespace RBSLangApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LessonDetailPage), typeof(LessonDetailPage));
        Routing.RegisterRoute(nameof(LessonTestsPage), typeof(LessonTestsPage));
    }
}