using SQLite;

namespace RBSLangApp.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Login { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
}
