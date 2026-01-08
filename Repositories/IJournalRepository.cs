using PersonalPages.Models;
using System.Collections.Generic;

public interface IJournalRepository
{
    void AddJournal(Journal journal);
    List<Journal> GetMyJournals(string userEmail);
    List<Journal> GetPublicJournals();
}
