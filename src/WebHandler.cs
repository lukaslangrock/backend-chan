using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace backend;

public class WebHandler
{
    public static async void Run(string[] args)
    {
        int receiveBufferSize = 0xFFFF;
        int sendBufferSize = 0xFFFF;
        
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        
        app.MapGet("/", () => "Connichiwa, backuendo-chan heru. Pleasu connectu on webu sokketsu to interufacu.");
        
        WebSocket? ws = null;
        
        app.UseWebSockets();
        app.Map("/ws", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                ws = await context.WebSockets.AcceptWebSocketAsync();
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        });
        
        app.RunAsync();
        
        var serverBuffer = WebSocket.CreateServerBuffer(receiveBufferSize);
        
        async Task<string?> Receive()
        {
            var result = await ws.ReceiveAsync(serverBuffer, CancellationToken.None);
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                return System.Text.Encoding.UTF8.GetString(serverBuffer.Array, serverBuffer.Offset, result.Count);
            }
            return null;
        }
        
        Task<string?>? receiveTask = null;
        Task? sendTask = null;
        
        while (true)
        {
            if (ws == null)
                continue;
        
            if(receiveTask == null)
                receiveTask = Receive();
        
            if (receiveTask.GetAwaiter().IsCompleted)
            {
                var result = receiveTask.GetAwaiter().GetResult();
        
                if (result != null)
                {
                    string? response = ProtocolHandler.OnReceive(result);
                    if (response != null)
                    {
                        var bytes = Encoding.UTF8.GetBytes(response);
        
                        if (sendTask != null && !sendTask.IsCompleted)
                            await sendTask;
        
                        sendTask = ws.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length),
                            System.Net.WebSockets.WebSocketMessageType.Text,
                            true, CancellationToken.None);
                    }
                }
            }
        }
    }
}