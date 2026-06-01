using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RBSLangApp.Models
{
    public class Term : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string EnglishWord { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;

        private bool _isFlipped;
        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                if (_isFlipped != value)
                {
                    _isFlipped = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(CardColor));
                    OnPropertyChanged(nameof(TextColor));
                }
            }
        }

        public string DisplayText => IsFlipped ? Translation : EnglishWord;

        public Color CardColor => IsFlipped
            ? Color.FromArgb("#E8F4FF")
            : Color.FromArgb("#F0FAE8");

        public Color TextColor => IsFlipped
            ? Color.FromArgb("#1CB0F6")
            : Color.FromArgb("#3C3C3C");

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}