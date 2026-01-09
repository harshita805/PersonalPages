using PersonalPages.Models;

public interface IJournalRepository
{
    void AddJournal(int userId, CreateJournalDto dto);
    List<JournalWithUserDto> GetMyJournals(int userId);
    List<JournalWithUserDto> GetPublicJournals();
}
