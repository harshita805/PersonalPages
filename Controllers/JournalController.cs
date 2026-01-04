using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/journal")]
public class JournalController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMyJournals()
    {
        return Ok("User journal entries fetched securely");
    }
}
