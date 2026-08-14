using MongoDB.Driver;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly IMongoCollection<ChatMessage> _messages;
    public ChatMessageRepository(IMongoDatabase database)
    {
        _messages = database.GetCollection<ChatMessage>("ChatMessages");
    }
    public async Task AddAsync(ChatMessage message)
    {
        await _messages.InsertOneAsync(message);
    }
    public async Task<List<ChatMessage>> GetConversationAsync(
        int userId,
        int otherUserId,
        int productId)
    {
        return await _messages
            .Find(m =>
                m.ProductId == productId &&
                (
                    (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                    (m.SenderId == otherUserId && m.ReceiverId == userId)
                )
            )
            .SortBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<ChatMessage>> GetMessagesForUserAsync(int userId)
    {
    return await _messages
        .Find(m =>
            m.SenderId == userId ||
            m.ReceiverId == userId
        )
        .SortByDescending(m => m.CreatedAt)
        .ToListAsync();
    }
}