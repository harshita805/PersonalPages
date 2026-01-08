using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPages.Models;
using System.Security.Claims;

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

    // ✍️ SAVE JOURNAL
    [HttpPost]
    public IActionResult Create(CreateJournalDto dto)
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        _service.CreateJournal(email, dto);
        return Ok(new { message = "Journal saved successfully" });
    }

    // 📘 MY JOURNALS
    [HttpGet("my")]
    public IActionResult MyJournals()
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        return Ok(_service.GetMyJournals(email));
    }

    // 🌍 PUBLIC JOURNALS
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult PublicJournals()
    {
        return Ok(_service.GetPublicJournals());
    }
}
