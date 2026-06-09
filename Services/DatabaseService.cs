using SQLite;
using RBSLangApp.Models;

namespace RBSLangApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private readonly string _dbPath;
    private bool _initialized;

    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
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


        var savedLessons = await _database.Table<Lesson>().ToListAsync();

        var lesson1 = savedLessons[0].Id;
        var lesson2 = savedLessons[1].Id;
        var lesson3 = savedLessons[2].Id;


        var terms = new List<Term>
        {

            new Term { LessonId = lesson1, EnglishWord = "Meeting", Translation = "Встреча", Example = "We have a meeting today." },
            new Term { LessonId = lesson1, EnglishWord = "Deadline", Translation = "Срок выполнения", Example = "The deadline is tomorrow." },
            new Term { LessonId = lesson1, EnglishWord = "Client", Translation = "Клиент", Example = "The client approved the project." },
            new Term { LessonId = lesson1, EnglishWord = "Proposal", Translation = "Предложение", Example = "We sent a proposal to the client." },
            new Term { LessonId = lesson1, EnglishWord = "Negotiation", Translation = "Переговоры", Example = "The negotiation was successful." },
            new Term { LessonId = lesson1, EnglishWord = "Invoice", Translation = "Счёт-фактура", Example = "Please send the invoice by Friday." },
            new Term { LessonId = lesson1, EnglishWord = "Schedule", Translation = "Расписание", Example = "Let me check my schedule." },
            new Term { LessonId = lesson1, EnglishWord = "Feedback", Translation = "Обратная связь", Example = "We appreciate your feedback." },
            new Term { LessonId = lesson1, EnglishWord = "Agenda", Translation = "Повестка дня", Example = "The agenda includes three main topics." },
            new Term { LessonId = lesson1, EnglishWord = "Stakeholder", Translation = "Заинтересованная сторона", Example = "All stakeholders must attend the briefing." },
            new Term { LessonId = lesson1, EnglishWord = "Follow-up", Translation = "Последующее действие", Example = "I will send a follow-up email after the call." },
            new Term { LessonId = lesson1, EnglishWord = "Quarterly report", Translation = "Квартальный отчёт", Example = "The quarterly report shows growth in Q2." },
            new Term { LessonId = lesson1, EnglishWord = "Conference call", Translation = "Конференц-звонок", Example = "We have a conference call with the London office." },
            new Term { LessonId = lesson1, EnglishWord = "Minutes", Translation = "Протокол собрания", Example = "Could you take the minutes during the meeting?" },
            new Term { LessonId = lesson1, EnglishWord = "Briefing", Translation = "Инструктаж", Example = "The manager gave a short briefing before the launch." },
            new Term { LessonId = lesson1, EnglishWord = "Lead", Translation = "Потенциальный клиент", Example = "Our sales team has found a new lead in Berlin." },
            new Term { LessonId = lesson1, EnglishWord = "Milestone", Translation = "Ключевой этап", Example = "We reached an important milestone this month." },
            new Term { LessonId = lesson1, EnglishWord = "Pitch", Translation = "Презентация идеи", Example = "He prepared a strong pitch for the investors." },
            new Term { LessonId = lesson1, EnglishWord = "Turnaround", Translation = "Оборот/изменение ситуации", Example = "We expect a fast turnaround on this request." },
            new Term { LessonId = lesson1, EnglishWord = "Deliverable", Translation = "Результат проекта", Example = "The final deliverable is due next Monday." },

            new Term { LessonId = lesson2, EnglishWord = "Contract", Translation = "Договор", Example = "The contract was signed yesterday." },
            new Term { LessonId = lesson2, EnglishWord = "Partition", Translation = "Перегородка", Example = "We need a glass partition here." },
            new Term { LessonId = lesson2, EnglishWord = "Layout", Translation = "Планировка", Example = "The office layout is very modern." },
            new Term { LessonId = lesson2, EnglishWord = "Floor plan", Translation = "План этажа", Example = "Can you send me the floor plan?" },
            new Term { LessonId = lesson2, EnglishWord = "Fit-out", Translation = "Отделка помещений", Example = "The fit-out will be completed next month." },
            new Term { LessonId = lesson2, EnglishWord = "Premises", Translation = "Помещение", Example = "We are looking for new premises." },
            new Term { LessonId = lesson2, EnglishWord = "Renovation", Translation = "Ремонт", Example = "The office is under renovation." },
            new Term { LessonId = lesson2, EnglishWord = "Open-plan", Translation = "Открытая планировка", Example = "The team works in an open-plan area." },
            new Term { LessonId = lesson2, EnglishWord = "Ceiling", Translation = "Потолок", Example = "We've installed a suspended ceiling." },
            new Term { LessonId = lesson2, EnglishWord = "Flooring", Translation = "Напольное покрытие", Example = "Vinyl flooring is chosen for high-traffic zones." },
            new Term { LessonId = lesson2, EnglishWord = "Lighting", Translation = "Освещение", Example = "LED lighting reduces energy consumption." },
            new Term { LessonId = lesson2, EnglishWord = "Acoustics", Translation = "Акустика", Example = "Room acoustics are improved with sound panels." },
            new Term { LessonId = lesson2, EnglishWord = "Workstation", Translation = "Рабочее место", Example = "Each workstation has an adjustable desk." },
            new Term { LessonId = lesson2, EnglishWord = "Breakout area", Translation = "Зона отдыха", Example = "Staff can relax in the breakout area." },
            new Term { LessonId = lesson2, EnglishWord = "Meeting room", Translation = "Переговорная", Example = "We've booked a meeting room for 3 PM." },
            new Term { LessonId = lesson2, EnglishWord = "Reception", Translation = "Ресепшн", Example = "Visitors must sign in at reception." },
            new Term { LessonId = lesson2, EnglishWord = "Access control", Translation = "Контроль доступа", Example = "Access control is managed with ID badges." },
            new Term { LessonId = lesson2, EnglishWord = "Ergonomics", Translation = "Эргономика", Example = "Ergonomics is essential for employee well-being." },
            new Term { LessonId = lesson2, EnglishWord = "HVAC", Translation = "ОВКВ", Example = "HVAC maintenance is scheduled for Friday." },
            new Term { LessonId = lesson2, EnglishWord = "Turnkey", Translation = "Под ключ", Example = "A turnkey solution includes design and construction." },


            new Term { LessonId = lesson3, EnglishWord = "Ventilation", Translation = "Вентиляция", Example = "The building has proper ventilation." },
            new Term { LessonId = lesson3, EnglishWord = "Power supply", Translation = "Электроснабжение", Example = "Power supply is stable in this area." },
            new Term { LessonId = lesson3, EnglishWord = "Wiring", Translation = "Электропроводка", Example = "Check the wiring before installation." },
            new Term { LessonId = lesson3, EnglishWord = "Ductwork", Translation = "Воздуховоды", Example = "The ductwork was installed incorrectly." },
            new Term { LessonId = lesson3, EnglishWord = "Circuit breaker", Translation = "Автоматический выключатель", Example = "The circuit breaker tripped again." },
            new Term { LessonId = lesson3, EnglishWord = "Chiller", Translation = "Чиллер (охладитель)", Example = "The chiller provides cooling for the building." },
            new Term { LessonId = lesson3, EnglishWord = "Generator", Translation = "Генератор", Example = "We have a backup generator." },
            new Term { LessonId = lesson3, EnglishWord = "Transformer", Translation = "Трансформатор", Example = "The transformer steps down the voltage." },
            new Term { LessonId = lesson3, EnglishWord = "Switchgear", Translation = "Распределительное устройство", Example = "Switchgear is installed in the electrical room." },
            new Term { LessonId = lesson3, EnglishWord = "Pump", Translation = "Насос", Example = "A circulation pump maintains water flow." },
            new Term { LessonId = lesson3, EnglishWord = "Compressor", Translation = "Компрессор", Example = "The compressor supplies compressed air to the system." },
            new Term { LessonId = lesson3, EnglishWord = "Thermostat", Translation = "Термостат", Example = "Set the thermostat to 22 degrees." },
            new Term { LessonId = lesson3, EnglishWord = "Fire alarm", Translation = "Пожарная сигнализация", Example = "The fire alarm system is tested weekly." },
            new Term { LessonId = lesson3, EnglishWord = "Sprinkler", Translation = "Спринклер (ороситель)", Example = "Sprinklers activate automatically during a fire." },
            new Term { LessonId = lesson3, EnglishWord = "Elevator", Translation = "Лифт", Example = "The elevator serves all five floors." },
            new Term { LessonId = lesson3, EnglishWord = "UPS", Translation = "Источник бесперебойного питания", Example = "UPS protects servers during power outages." },
            new Term { LessonId = lesson3, EnglishWord = "Pipe fitting", Translation = "Трубопроводная арматура", Example = "Pipe fittings must comply with the standard." },
            new Term { LessonId = lesson3, EnglishWord = "Insulation", Translation = "Изоляция", Example = "Thermal insulation reduces heat loss." },
            new Term { LessonId = lesson3, EnglishWord = "Load", Translation = "Нагрузка", Example = "The structure can handle the additional load." },
            new Term { LessonId = lesson3, EnglishWord = "Conduit", Translation = "Кабельный канал", Example = "Wires run through a protective conduit." }
        };

        await _database.InsertAllAsync(terms);


        var tests = new List<TestQuestion>
        {

            new TestQuestion
            {
                LessonId = lesson1,
                Question = "What is 'Deadline'?",
                Option1 = "Встреча",
                Option2 = "Срок выполнения",
                Option3 = "Клиент",
                Option4 = "Менеджер",
                CorrectAnswer = "Срок выполнения"
            },
            new TestQuestion
            {
                LessonId = lesson1,
                Question = "What is 'Proposal'?",
                Option1 = "Предложение",
                Option2 = "Проблема",
                Option3 = "Проект",
                Option4 = "План",
                CorrectAnswer = "Предложение"
            },
            new TestQuestion
            {
                LessonId = lesson1,
                Question = "What is 'Invoice'?",
                Option1 = "Письмо",
                Option2 = "Счёт-фактура",
                Option3 = "Визитка",
                Option4 = "Документ",
                CorrectAnswer = "Счёт-фактура"
            },
            new TestQuestion
            {
                LessonId = lesson1,
                Question = "What is 'Stakeholder'?",
                Option1 = "Акционер",
                Option2 = "Заинтересованная сторона",
                Option3 = "Поставщик",
                Option4 = "Сотрудник",
                CorrectAnswer = "Заинтересованная сторона"
            },
            new TestQuestion
            {
                LessonId = lesson1,
                Question = "What is 'Milestone'?",
                Option1 = "Камень",
                Option2 = "Ключевой этап",
                Option3 = "Метка",
                Option4 = "Граница",
                CorrectAnswer = "Ключевой этап"
            },

            new TestQuestion
            {
                LessonId = lesson2,
                Question = "What is 'Contract'?",
                Option1 = "Договор",
                Option2 = "Срок",
                Option3 = "Встреча",
                Option4 = "Офис",
                CorrectAnswer = "Договор"
            },
            new TestQuestion
            {
                LessonId = lesson2,
                Question = "What is 'Partition'?",
                Option1 = "Перегородка",
                Option2 = "Потолок",
                Option3 = "Дверь",
                Option4 = "Окно",
                CorrectAnswer = "Перегородка"
            },
            new TestQuestion
            {
                LessonId = lesson2,
                Question = "What is 'Renovation'?",
                Option1 = "Переезд",
                Option2 = "Ремонт",
                Option3 = "Продажа",
                Option4 = "Аренда",
                CorrectAnswer = "Ремонт"
            },
            new TestQuestion
            {
                LessonId = lesson2,
                Question = "What is 'Ergonomics'?",
                Option1 = "Экономика",
                Option2 = "Эргономика",
                Option3 = "Энергетика",
                Option4 = "Экология",
                CorrectAnswer = "Эргономика"
            },
            new TestQuestion
            {
                LessonId = lesson2,
                Question = "What is 'Turnkey'?",
                Option1 = "Ключ",
                Option2 = "Под ключ",
                Option3 = "Поворот",
                Option4 = "Замок",
                CorrectAnswer = "Под ключ"
            },


            new TestQuestion
            {
                LessonId = lesson3,
                Question = "What is 'Ventilation'?",
                Option1 = "Окно",
                Option2 = "Вентиляция",
                Option3 = "Дверь",
                Option4 = "Свет",
                CorrectAnswer = "Вентиляция"
            },
            new TestQuestion
            {
                LessonId = lesson3,
                Question = "What is 'HVAC'?",
                Option1 = "Система безопасности",
                Option2 = "Система пожаротушения",
                Option3 = "Отопление, вентиляция и кондиционирование",
                Option4 = "Электроснабжение",
                CorrectAnswer = "Отопление, вентиляция и кондиционирование"
            },
            new TestQuestion
            {
                LessonId = lesson3,
                Question = "What is 'Generator'?",
                Option1 = "Трансформатор",
                Option2 = "Генератор",
                Option3 = "Двигатель",
                Option4 = "Насос",
                CorrectAnswer = "Генератор"
            },
            new TestQuestion
            {
                LessonId = lesson3,
                Question = "What is 'UPS'?",
                Option1 = "Почтовая служба",
                Option2 = "Источник бесперебойного питания",
                Option3 = "Блок предохранителей",
                Option4 = "Система отопления",
                CorrectAnswer = "Источник бесперебойного питания"
            },
            new TestQuestion
            {
                LessonId = lesson3,
                Question = "What is 'Thermostat'?",
                Option1 = "Термометр",
                Option2 = "Термостат",
                Option3 = "Трансформатор",
                Option4 = "Терминал",
                CorrectAnswer = "Термостат"
            }
        };

        await _database.InsertAllAsync(tests);
    }

    public Task<User?> GetUserAsync(string login, string password) =>
        _database.Table<User>()
            .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

    public Task<List<Lesson>> GetLessonsAsync() =>
        _database.Table<Lesson>().ToListAsync();

    public Task<Lesson?> GetLessonAsync(int id) =>
        _database.Table<Lesson>()
            .FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<Term>> GetTermsByLessonAsync(int lessonId) =>
        _database.Table<Term>()
            .Where(x => x.LessonId == lessonId)
            .ToListAsync();

    public Task<List<Term>> SearchTermsAsync(string search) =>
        string.IsNullOrWhiteSpace(search)
            ? _database.Table<Term>().ToListAsync()
            : _database.Table<Term>()
                .Where(x => x.EnglishWord.Contains(search) || x.Translation.Contains(search))
                .ToListAsync();

    public Task<List<TestQuestion>> GetQuestionsByLessonAsync(int lessonId) =>
        _database.Table<TestQuestion>()
            .Where(x => x.LessonId == lessonId)
            .ToListAsync();

    public async Task<bool> ToggleFavoriteAsync(string userLogin, Term term)
    {
        var existing = await _database.Table<FavoriteTerm>()
            .FirstOrDefaultAsync(x => x.UserLogin == userLogin && x.TermId == term.Id);

        if (existing == null)
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

        await _database.DeleteAsync(existing);
        return false;
    }

    public Task<List<FavoriteTerm>> GetFavoritesAsync(string userLogin) =>
        _database.Table<FavoriteTerm>()
            .Where(x => x.UserLogin == userLogin)
            .ToListAsync();

    public async Task<int> GetFavoriteCountAsync(string userLogin)
    {
        return await _database.Table<FavoriteTerm>()
            .Where(x => x.UserLogin == userLogin)
            .CountAsync();
    }


    public async Task SaveProgressAsync(string userLogin, int lessonId, int score, int total)
    {
        await _database.InsertAsync(new Progress
        {
            UserLogin = userLogin,
            LessonId = lessonId,
            Score = score,
            TotalQuestions = total,
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
        var data = await GetProgressAsync(userLogin);
        if (data.Count == 0) return 0;

        return data.Average(x =>
            x.TotalQuestions == 0 ? 0 : (double)x.Score / x.TotalQuestions * 100);
    }
}