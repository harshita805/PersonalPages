using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPages.Models;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        return Ok(_service.GetProfile(email));
    }

    [HttpPut("profile")]
    public IActionResult UpdateProfile(UpdateUserDto dto)
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        _service.UpdateProfile(email, dto);
        return Ok(new { message = "Profile updated successfully" });
    }
}
