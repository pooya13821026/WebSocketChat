using System.Net.WebSockets;

namespace WebSocket.WebSocketSetting;

public class UserWebSocket
{
    public string UserName { get; set; }
    public System.Net.WebSockets.WebSocket UserWS { get; set; }
}