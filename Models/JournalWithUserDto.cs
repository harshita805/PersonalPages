namespace PersonalPages.Models
{
    public class JournalWithUserDto
    {
        public int JournalId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Mood { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }

        // User info
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

}
