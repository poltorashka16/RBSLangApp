namespace RBSLangApp;

public static class AppState
{
    public static string CurrentUserLogin { get; set; } = string.Empty;
    public static string CurrentUserName { get; set; } = string.Empty;

    public static bool IsAuthorized => !string.IsNullOrWhiteSpace(CurrentUserLogin);

    public static void Clear()
    {
        CurrentUserLogin = string.Empty;
        CurrentUserName = string.Empty;
    }
}
