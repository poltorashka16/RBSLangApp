using SQLite;

namespace RBSLangApp.Models;

public class Lesson
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Subtitle { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Level { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
