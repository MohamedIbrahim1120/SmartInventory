using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class LoginHistoryController : ControllerBase
{
    private readonly SmartInventoryDbContext _context;

    public LoginHistoryController(SmartInventoryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetLoginHistory()
    {
        var history = _context.UserLoginHistories
            .OrderByDescending(x => x.LoginTime)
            .Take(100)
            .ToList();

        return Ok(history);
    }
}
