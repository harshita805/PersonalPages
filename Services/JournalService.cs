using PersonalPages.Models;

public class JournalService : IJournalService
{
    private readonly IJournalRepository _repo;

    public JournalService(IJournalRepository repo)
    {
        _repo = repo;
    }

    public void CreateJournal(string email, CreateJournalDto dto)
    {
        var journal = new Journal
        {
            UserEmail = email,
            Title = dto.Title,
            Content = dto.Content,
            Mood = dto.Mood,
            IsPublic = dto.IsPublic
        };

        _repo.AddJournal(journal);
    }

    public List<Journal> GetMyJournals(string email)
    {
        return _repo.GetMyJournals(email);
    }

    public List<Journal> GetPublicJournals()
    {
        return _repo.GetPublicJournals();
    }
}
