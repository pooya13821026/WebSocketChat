using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebSocket.WebSocketSetting;

public class WebSocketClass
{
    public async Task ListenAccept(HttpContext context)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        string userName = context.Request.Query["Name"];
        var userWebSocket = new UserWebSocket();
        userWebSocket.UserName = userName;
        userWebSocket.UserWS = webSocket;
        UserList.ListDic.Add(userName, userWebSocket);
        await Receive(userWebSocket);
    }

    public async Task Receive(UserWebSocket userWebSocket)
    {
        var webSocket = userWebSocket.UserWS;
        var userName = userWebSocket.UserName;

        while (webSocket.State == WebSocketState.Open)
        {
            var buffer = new byte[WebSocketSetting.BufferSize];
            WebSocketReceiveResult result =
                await webSocket.ReceiveAsync(new ArraySegment<byte>(array: buffer, offset: 0, count: buffer.Length),
                    CancellationToken.None);
            if (result != null)
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var massage = Encoding.UTF8.GetString(buffer);
                    var receiveMassage = JsonSerializer.Deserialize<ReceiveMassage>(massage);

                    UserWebSocket receiveUWS = UserList.ListDic[receiveMassage.Receive];

                    SenderMassage senderMassage = new SenderMassage
                    {
                        Sender = userName,
                        Massage = receiveMassage.Massage,
                    };
                }
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                UserList.ListDic.Remove(userName);
                return;
            }
        }
    }

    public async Task Sender(System.Net.WebSockets.WebSocket webSocket, SenderMassage massage)
    {
        var senderMassage = JsonSerializer.Serialize(massage);
        byte[] buff = Encoding.UTF8.GetBytes(senderMassage);
        await webSocket.SendAsync(new ArraySegment<byte>(array: buff, offset: 0, count: buff.Length),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }
}