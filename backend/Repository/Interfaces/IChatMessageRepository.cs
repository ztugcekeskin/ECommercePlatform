using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message);
    Task<List<ChatMessage>> GetConversationAsync(
        int userId,
        int otherUserId,
        int productId
    );
    Task<List<ChatMessage>> GetMessagesForUserAsync(int userId);
}