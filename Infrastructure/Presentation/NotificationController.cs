using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SmartInventoryDbContext _context;
    public NotificationController(IUnitOfWork unitOfWork, SmartInventoryDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _unitOfWork.Notifications.GetAllAsync();
        return Ok(notifications.OrderByDescending(n => n.Timestamp));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string message)
    {
        var notification = new Notification { Message = message };
        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
        if (notification == null) return NotFound();

        notification.IsRead = true;
        _unitOfWork.Notifications.Update(notification);
        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("chat")]
    public IActionResult GetChatNotifications()
    {
        var notifications = _context.Notifications
            .Where(n => n.Type == "Chat")
            .OrderByDescending(n => n.Timestamp)
            .ToList();

        return Ok(notifications);
    }

}
