using SQLite;

namespace RBSLangApp.Models;

public class Progress
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(50)]
    public string UserLogin { get; set; } = string.Empty;

    public int LessonId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime CompletionDate { get; set; }
}
