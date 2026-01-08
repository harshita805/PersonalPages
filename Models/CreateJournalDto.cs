namespace PersonalPages.Models
{
    public class CreateJournalDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Mood { get; set; }
        public bool IsPublic { get; set; }
    }
}
