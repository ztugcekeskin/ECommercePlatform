using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Services
{
    public class ChatWebSocketHandler
    {
        private readonly ConcurrentDictionary<int, WebSocket> _connections = new();
        private readonly IServiceScopeFactory _scopeFactory;
        public ChatWebSocketHandler(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public void AddConnection(int userId, WebSocket socket)
        {
            _connections[userId] = socket;
        }
        public void RemoveConnection(int userId)
        {
            _connections.TryRemove(userId, out _);
        }
        public bool TryGetConnection(
            int userId,
            out WebSocket? socket)
        {
            return _connections.TryGetValue(userId, out socket);
        }
        public async Task HandleMessageAsync(
            int senderId,
            string message)
        {
            try
            {
                var chatMessage =
                    JsonSerializer.Deserialize<ChatMessage>(message);
                if (chatMessage == null)
                {
                    return;
                }
                // WebSocket'e bağlanan kullanıcı ile mesajı gönderen kullanıcı aynı olmalı.
                if (chatMessage.SenderId != senderId)
                {
                    Console.WriteLine(
                        "SenderId ile WebSocket userId uyuşmuyor."
                    );
                    return;
                }
                // Kullanıcı kendisine mesaj gönderemez
                if (chatMessage.SenderId == chatMessage.ReceiverId)
                {
                    Console.WriteLine(
                        "Kullanıcı kendisine mesaj gönderemez."
                    );
                    return;
                }
                if (string.IsNullOrWhiteSpace(chatMessage.Message))
                {
                    return;
                }
                chatMessage.Id = "";
                chatMessage.CreatedAt = DateTime.UtcNow;
                // Scoped repository için yeni scope oluştur
                using (var scope = _scopeFactory.CreateScope())
                {
                    var chatRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IChatMessageRepository>();
                    // MongoDB'ye kaydet
                    await chatRepository.AddAsync(chatMessage);
                }

                Console.WriteLine(
                    $"💾 Mesaj MongoDB'ye kaydedildi: " +
                    $"{chatMessage.SenderId} -> " +
                    $"{chatMessage.ReceiverId} " +
                    $"Product: {chatMessage.ProductId}"
                );
                // Alıcıya gönder
                var jsonMessage =
                    JsonSerializer.Serialize(chatMessage);
                await SendMessageAsync(
                    chatMessage.ReceiverId,
                    jsonMessage
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $" WebSocket mesaj işleme hatası: {ex.Message}"
                );
            }
        }
        public async Task SendMessageAsync(
            int receiverId,
            string message)
        {
            if (!_connections.TryGetValue(
                    receiverId,
                    out var socket))
            {
                Console.WriteLine(
                    $"{receiverId} kullanıcısının WebSocket bağlantısı yok."
                );
                return;
            }

            if (socket.State != WebSocketState.Open)
            {
                RemoveConnection(receiverId);
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(message);
            try
            {
                await socket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                Console.WriteLine(
                    $"Mesaj {receiverId} kullanıcısına gönderildi."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"WebSocket gönderme hatası: {ex.Message}"
                );
                RemoveConnection(receiverId);
            }
        }
    }
}
