using PersonalPages.Models;

public interface IJournalService
{
    void CreateJournal(int userId, CreateJournalDto dto);
    List<JournalWithUserDto> GetMyJournals(int userId);
    List<JournalWithUserDto> GetPublicJournals();
}
