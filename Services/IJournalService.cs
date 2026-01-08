using PersonalPages.Models;
using System.Collections.Generic;

public interface IJournalService
{
    void CreateJournal(string email, CreateJournalDto dto);
    List<Journal> GetMyJournals(string email);
    List<Journal> GetPublicJournals();
}
