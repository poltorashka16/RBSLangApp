using SQLite;
using RBSLangApp.Models;

namespace RBSLangApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitAsync()
    {
        if (_initialized) return;

        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<Lesson>();
        await _database.CreateTableAsync<Term>();
        await _database.CreateTableAsync<TestQuestion>();
        await _database.CreateTableAsync<FavoriteTerm>();
        await _database.CreateTableAsync<Progress>();

        await SeedAsync();
        _initialized = true;
    }

    private async Task SeedAsync()
    {
        if (await _database.Table<User>().CountAsync() == 0)
        {
            await _database.InsertAllAsync(new List<User>
            {
                new User { FullName = "Анна Смирнова", Login = "anna", Password = "12345" },
                new User { FullName = "Илья Петров", Login = "ilya", Password = "12345" }
            });
        }

        if (await _database.Table<Lesson>().CountAsync() > 0)
            return;

        var lessons = new List<Lesson>
        {
            new Lesson
            {
                Title = "Business Communication",
                Subtitle = "Клиенты, письма, встречи",
                Level = "A2-B1",
                Content = "In this lesson you will learn key words for client communication, meetings, deadlines, and project discussion."
            },
            new Lesson
            {
                Title = "Fit-Out and Office Spaces",
                Subtitle = "Офисные пространства и отделка",
                Level = "A2-B1",
                Content = "This lesson covers office space, partition, finishing works, commercial premises, and design solutions."
            },
            new Lesson
            {
                Title = "Engineering Systems",
                Subtitle = "Инженерные системы",
                Level = "B1",
                Content = "This lesson includes ventilation, air conditioning, power supply, engineering systems, and installation."
            }
        };
        await _database.InsertAllAsync(lessons);

        var terms = new List<Term>
        {
            new Term { LessonId = 1, EnglishWord = "Meeting", Translation = "Встреча", Example = "We have a meeting with the client today." },
            new Term { LessonId = 1, EnglishWord = "Deadline", Translation = "Срок сдачи", Example = "The project deadline is next Friday." },
            new Term { LessonId = 1, EnglishWord = "Client", Translation = "Клиент", Example = "The client approved the design." },
            new Term { LessonId = 1, EnglishWord = "Contractor", Translation = "Подрядчик", Example = "The contractor prepared the estimate." },

            new Term { LessonId = 2, EnglishWord = "Office space", Translation = "Офисное пространство", Example = "The company rents a new office space." },
            new Term { LessonId = 2, EnglishWord = "Partition", Translation = "Перегородка", Example = "Glass partitions divide the open space." },
            new Term { LessonId = 2, EnglishWord = "Finishing works", Translation = "Отделочные работы", Example = "Finishing works were completed on time." },
            new Term { LessonId = 2, EnglishWord = "Commercial premises", Translation = "Коммерческое помещение", Example = "The design was created for commercial premises." },

            new Term { LessonId = 3, EnglishWord = "Ventilation", Translation = "Вентиляция", Example = "The ventilation system improves air quality." },
            new Term { LessonId = 3, EnglishWord = "Air conditioning", Translation = "Кондиционирование", Example = "Air conditioning is required for the office." },
            new Term { LessonId = 3, EnglishWord = "Power supply", Translation = "Электроснабжение", Example = "Power supply must be stable at all times." },
            new Term { LessonId = 3, EnglishWord = "Installation", Translation = "Монтаж", Example = "The installation of cables started yesterday." }
        };
        await _database.InsertAllAsync(terms);

        await _database.InsertAllAsync(new List<TestQuestion>
        {
            new TestQuestion { LessonId = 1, Question = "What is the translation of 'Deadline'?", Option1 = "Встреча", Option2 = "Срок сдачи", Option3 = "Клиент", Option4 = "Подрядчик", CorrectAnswer = "Срок сдачи" },
            new TestQuestion { LessonId = 1, Question = "What does 'Client' mean?", Option1 = "Письмо", Option2 = "Клиент", Option3 = "Смета", Option4 = "Договор", CorrectAnswer = "Клиент" },
            new TestQuestion { LessonId = 2, Question = "What is 'Partition'?", Option1 = "Перегородка", Option2 = "Потолок", Option3 = "Окно", Option4 = "Лифт", CorrectAnswer = "Перегородка" },
            new TestQuestion { LessonId = 2, Question = "What does 'Finishing works' mean?", Option1 = "Документы", Option2 = "Отделочные работы", Option3 = "Проверка", Option4 = "Поставка", CorrectAnswer = "Отделочные работы" },
            new TestQuestion { LessonId = 3, Question = "What is 'Power supply'?", Option1 = "Монтаж", Option2 = "Электроснабжение", Option3 = "Вентиляция", Option4 = "Отделка", CorrectAnswer = "Электроснабжение" },
            new TestQuestion { LessonId = 3, Question = "What does 'Ventilation' mean?", Option1 = "Проверка", Option2 = "Вентиляция", Option3 = "Смета", Option4 = "Проект", CorrectAnswer = "Вентиляция" }
        });
    }

    public Task<User?> GetUserAsync(string login, string password) =>
        _database.Table<User>().FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

    public Task<List<Lesson>> GetLessonsAsync() =>
        _database.Table<Lesson>().ToListAsync();

    public Task<Lesson?> GetLessonAsync(int id) =>
        _database.Table<Lesson>().FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<Term>> GetTermsByLessonAsync(int lessonId) =>
        _database.Table<Term>().Where(x => x.LessonId == lessonId).ToListAsync();

    public Task<List<Term>> SearchTermsAsync(string search) =>
        string.IsNullOrWhiteSpace(search)
            ? _database.Table<Term>().ToListAsync()
            : _database.Table<Term>()
                .Where(x => x.EnglishWord.Contains(search) || x.Translation.Contains(search))
                .ToListAsync();

    public Task<List<TestQuestion>> GetQuestionsByLessonAsync(int lessonId) =>
        _database.Table<TestQuestion>().Where(x => x.LessonId == lessonId).ToListAsync();

    public async Task<bool> ToggleFavoriteAsync(string userLogin, Term term)
    {
        var current = await _database.Table<FavoriteTerm>()
            .FirstOrDefaultAsync(x => x.UserLogin == userLogin && x.TermId == term.Id);

        if (current is null)
        {
            await _database.InsertAsync(new FavoriteTerm
            {
                UserLogin = userLogin,
                TermId = term.Id,
                EnglishWord = term.EnglishWord,
                Translation = term.Translation,
                Example = term.Example
            });
            return true;
        }

        await _database.DeleteAsync(current);
        return false;
    }

    public Task<List<FavoriteTerm>> GetFavoritesAsync(string userLogin) =>
        _database.Table<FavoriteTerm>().Where(x => x.UserLogin == userLogin).ToListAsync();

    public async Task<int> GetFavoriteCountAsync(string userLogin) =>
        await _database.Table<FavoriteTerm>().Where(x => x.UserLogin == userLogin).CountAsync();

    public async Task SaveProgressAsync(string userLogin, int lessonId, int score, int totalQuestions)
    {
        await _database.InsertAsync(new Progress
        {
            UserLogin = userLogin,
            LessonId = lessonId,
            Score = score,
            TotalQuestions = totalQuestions,
            CompletionDate = DateTime.Now
        });
    }

    public Task<List<Progress>> GetProgressAsync(string userLogin) =>
        _database.Table<Progress>()
            .Where(x => x.UserLogin == userLogin)
            .OrderByDescending(x => x.CompletionDate)
            .ToListAsync();

    public async Task<double> GetAveragePercentAsync(string userLogin)
    {
        var rows = await GetProgressAsync(userLogin);
        if (rows.Count == 0) return 0;

        return rows.Average(x => x.TotalQuestions == 0 ? 0 : (double)x.Score / x.TotalQuestions * 100.0);
    }
}
