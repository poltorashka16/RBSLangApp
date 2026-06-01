using SQLite;

namespace RBSLangApp.Models;

public class FavoriteTerm
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(50)]
    public string UserLogin { get; set; } = string.Empty;

    public int TermId { get; set; }

    public string EnglishWord { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
}
