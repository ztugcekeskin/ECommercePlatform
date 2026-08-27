using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using System.Text.Json;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatMessageRepository _chatRepository;
    private readonly ChatWebSocketHandler _webSocketHandler;

    public ChatController(
    IChatMessageRepository chatRepository,
    ChatWebSocketHandler webSocketHandler)
{
    _chatRepository = chatRepository;
    _webSocketHandler = webSocketHandler;
}

    [HttpPost]
    public async Task<IActionResult> SendMessage(
    [FromBody] ChatMessage message)
{
    if (string.IsNullOrWhiteSpace(message.Message))
    {
        return BadRequest(new
        {
            message = "Mesaj boş bırakılamaz."
        });
    }

    // Kullanıcı kendisine mesaj gönderemez
    if (message.SenderId == message.ReceiverId)
    {
        return BadRequest(new
        {
            message = "Kullanıcı kendisine mesaj gönderemez."
        });
    }

    message.Id = "";
    message.CreatedAt = DateTime.UtcNow;

    await _chatRepository.AddAsync(message);

    var jsonMessage = JsonSerializer.Serialize(message);

    await _webSocketHandler.SendMessageAsync(
        message.ReceiverId,
        jsonMessage
    );

    return Ok(message);
}

    [HttpGet]
    public async Task<IActionResult> GetConversation(
        int userId,
        int otherUserId,
        int productId)
    {
        var messages =
            await _chatRepository.GetConversationAsync(
                userId,
                otherUserId,
                productId
            );

        return Ok(messages);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserMessages(int userId)
    {
    var messages =
        await _chatRepository.GetMessagesForUserAsync(userId);

    return Ok(messages);
    }
}