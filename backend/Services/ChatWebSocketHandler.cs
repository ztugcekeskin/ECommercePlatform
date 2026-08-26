using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebAPI.Services
{
    public class ChatWebSocketHandler
    {
        private readonly ConcurrentDictionary<int, WebSocket> _connections = new();

        public void AddConnection(int userId, WebSocket socket)
        {
            _connections[userId] = socket;
        }

        public void RemoveConnection(int userId)
        {
            _connections.TryRemove(userId, out _);
        }

        public bool TryGetConnection(int userId, out WebSocket? socket)
        {
            return _connections.TryGetValue(userId, out socket);
        }

        public async Task SendMessageAsync(int receiverId, string message)
        {
            if (!_connections.TryGetValue(receiverId, out var socket))
            {
                return;
            }

            if (socket.State != WebSocketState.Open)
            {
                RemoveConnection(receiverId);
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(message);

            await socket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }
}