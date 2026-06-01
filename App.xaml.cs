using RBSLangApp.Services;
using RBSLangApp.Views;

namespace RBSLangApp;

public partial class App : Application
{
    public static DatabaseService Database { get; private set; } = null!;

    public App(DatabaseService databaseService)
    {
        InitializeComponent();

        Database = databaseService;
        MainPage = new NavigationPage(new LoginPage())
        {
            BarBackgroundColor = Color.FromArgb("#141217"),
            BarTextColor = Colors.White
        };

        Task.Run(async () => await Database.InitAsync());
    }
}
