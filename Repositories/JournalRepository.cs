using Microsoft.Data.SqlClient;
using PersonalPages.Models;

public class JournalRepository : IJournalRepository
{
    private readonly IConfiguration _config;

    public JournalRepository(IConfiguration config)
    {
        _config = config;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    }

    // ✅ SAVE JOURNAL
    public void AddJournal(int userId, CreateJournalDto dto)
    {
        using var con = GetConnection();
        var cmd = new SqlCommand(@"
            INSERT INTO Journals (UserId, Title, Content, Mood, IsPublic)
            VALUES (@UserId, @Title, @Content, @Mood, @IsPublic)", con);

        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@Title", dto.Title);
        cmd.Parameters.AddWithValue("@Content", dto.Content);
        cmd.Parameters.AddWithValue("@Mood", dto.Mood ?? "");
        cmd.Parameters.AddWithValue("@IsPublic", dto.IsPublic);

        con.Open();
        cmd.ExecuteNonQuery();
    }

    // ✅ FETCH MY JOURNALS + USER DETAILS
    public List<JournalWithUserDto> GetMyJournals(int userId)
    {
        var list = new List<JournalWithUserDto>();
        using var con = GetConnection();

        var cmd = new SqlCommand(@"
            SELECT j.*, u.FullName, u.Email
            FROM Journals j
            JOIN Users u ON j.UserId = u.UserId
            WHERE j.UserId = @UserId
            ORDER BY j.CreatedAt DESC", con);

        cmd.Parameters.AddWithValue("@UserId", userId);

        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new JournalWithUserDto
            {
                JournalId = (int)r["JournalId"],
                Title = r["Title"].ToString(),
                Content = r["Content"].ToString(),
                Mood = r["Mood"].ToString(),
                IsPublic = (bool)r["IsPublic"],
                CreatedAt = (DateTime)r["CreatedAt"],
                UserId = (int)r["UserId"],
                FullName = r["FullName"].ToString(),
                Email = r["Email"].ToString()
            });
        }
        return list;
    }

    // ✅ FETCH PUBLIC JOURNALS + CREATOR NAME
    public List<JournalWithUserDto> GetPublicJournals()
    {
        var list = new List<JournalWithUserDto>();
        using var con = GetConnection();

        var cmd = new SqlCommand(@"
            SELECT j.*, u.FullName
            FROM Journals j
            JOIN Users u ON j.UserId = u.UserId
            WHERE j.IsPublic = 1
            ORDER BY j.CreatedAt DESC", con);

        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new JournalWithUserDto
            {
                JournalId = (int)r["JournalId"],
                Title = r["Title"].ToString(),
                Content = r["Content"].ToString(),
                Mood = r["Mood"].ToString(),
                CreatedAt = (DateTime)r["CreatedAt"],
                FullName = r["FullName"].ToString()
            });
        }
        return list;
    }
}
