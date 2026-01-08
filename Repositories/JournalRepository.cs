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
        return new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));
    }

    public void AddJournal(Journal journal)
    {
        using var con = GetConnection();
        var cmd = new SqlCommand(
            @"INSERT INTO Journals
              (UserEmail, Title, Content, Mood, IsPublic)
              VALUES
              (@UserEmail, @Title, @Content, @Mood, @IsPublic)", con);

        cmd.Parameters.AddWithValue("@UserEmail", journal.UserEmail);
        cmd.Parameters.AddWithValue("@Title", journal.Title);
        cmd.Parameters.AddWithValue("@Content", journal.Content);
        cmd.Parameters.AddWithValue("@Mood", journal.Mood ?? "");
        cmd.Parameters.AddWithValue("@IsPublic", journal.IsPublic);

        con.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Journal> GetMyJournals(string userEmail)
    {
        var list = new List<Journal>();
        using var con = GetConnection();

        var cmd = new SqlCommand(
            "SELECT * FROM Journals WHERE UserEmail=@Email ORDER BY CreatedAt DESC", con);
        cmd.Parameters.AddWithValue("@Email", userEmail);

        con.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Journal
            {
                JournalId = (int)reader["JournalId"],
                UserEmail = reader["UserEmail"].ToString(),
                Title = reader["Title"].ToString(),
                Content = reader["Content"].ToString(),
                Mood = reader["Mood"].ToString(),
                IsPublic = (bool)reader["IsPublic"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }
        return list;
    }

    public List<Journal> GetPublicJournals()
    {
        var list = new List<Journal>();
        using var con = GetConnection();

        var cmd = new SqlCommand(
            "SELECT * FROM Journals WHERE IsPublic=1 ORDER BY CreatedAt DESC", con);

        con.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Journal
            {
                JournalId = (int)reader["JournalId"],
                Title = reader["Title"].ToString(),
                Content = reader["Content"].ToString(),
                Mood = reader["Mood"].ToString(),
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }
        return list;
    }
}
