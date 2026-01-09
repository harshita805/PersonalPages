using PersonalPages.Models;

public class JournalService : IJournalService
{
    private readonly IJournalRepository _repo;

    public JournalService(IJournalRepository repo)
    {
        _repo = repo;
    }

    public void CreateJournal(int userId, CreateJournalDto dto)
    {
        _repo.AddJournal(userId, dto);
    }

    public List<JournalWithUserDto> GetMyJournals(int userId)
    {
        return _repo.GetMyJournals(userId);
    }

    public List<JournalWithUserDto> GetPublicJournals()
    {
        return _repo.GetPublicJournals();
    }
}
