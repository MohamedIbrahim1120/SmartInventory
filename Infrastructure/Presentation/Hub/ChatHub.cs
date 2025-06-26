using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Persistence;

public class ChatHub : Hub
{
    private readonly SmartInventoryDbContext _context;

    public ChatHub(SmartInventoryDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string user, string message)
    {
        var chatMessage = new ChatMessage
        {
            Sender = user,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", user, message, chatMessage.Timestamp);
    }
}
