namespace WebSocket.WebSocketSetting;

public class WebSocketSetting
{
    public static int BufferSize = 4 * 1024;

    public static WebSocketOptions GetOptions()
    {
        var wso = new WebSocketOptions()
        {
            ReceiveBufferSize = BufferSize,
            KeepAliveInterval = TimeSpan.FromSeconds(120)
        };
        return wso; 
    }
}