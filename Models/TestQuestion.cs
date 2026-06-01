using SQLite;

namespace RBSLangApp.Models;

public class TestQuestion
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string Question { get; set; } = string.Empty;
    public string Option1 { get; set; } = string.Empty;
    public string Option2 { get; set; } = string.Empty;
    public string Option3 { get; set; } = string.Empty;
    public string Option4 { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}
