using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatMessageRepository _chatRepository;

    public ChatController(IChatMessageRepository chatRepository)
    {
    _chatRepository = chatRepository;
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

        message.Id = "";
        message.CreatedAt = DateTime.UtcNow;

        await _chatRepository.AddAsync(message);

        return Ok(new
        {
            message = "Mesaj başarıyla gönderildi."
        });
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