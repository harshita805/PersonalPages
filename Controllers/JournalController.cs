using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPages.Models;

[Authorize]
[ApiController]
[Route("api/journal")]
public class JournalController : ControllerBase
{
    private readonly IJournalService _service;

    public JournalController(IJournalService service)
    {
        _service = service;
    }

    // ✅ SAVE JOURNAL
    [HttpPost]
    public IActionResult Create(CreateJournalDto dto)
    {
        int userId = int.Parse(User.FindFirst("UserId").Value);
        _service.CreateJournal(userId, dto);
        return Ok(new { message = "Journal saved successfully" });
    }

    // ✅ FETCH MY JOURNALS
    [HttpGet("my")]
    public IActionResult MyJournals()
    {
        int userId = int.Parse(User.FindFirst("UserId").Value);
        return Ok(_service.GetMyJournals(userId));
    }

    // ✅ FETCH PUBLIC JOURNALS
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult PublicJournals()
    {
        return Ok(_service.GetPublicJournals());
    }
}
