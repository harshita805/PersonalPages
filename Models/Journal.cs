namespace PersonalPages.Models
{
    public class Journal
    {
        public int JournalId { get; set; }
        public string UserEmail { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Mood { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
