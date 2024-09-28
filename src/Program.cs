using backend;
using backend.database;

using System.Net;
using System.Net.WebSockets;
using System.Text;

LlmController.Init();

DB.CreateDB();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var connections = new Dictionary<WebSocket, int>();

app.MapGet("/", () => "Connichiwa, backuendo-chan heru. Pleasu connectu on webu sokketsu to interufacu.");

app.UseWebSockets();
app.Map("/ws", async context =>
{
    Console.WriteLine("[WS] Trying to handle data incoming to endpoint '/ws'");
    if (context.WebSockets.IsWebSocketRequest)
    {
        WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
        connections.Add(ws, DateTimeOffset.UtcNow.ToUnixTimeSeconds().GetHashCode()); // very secure, very mindful
        Console.WriteLine("[WebSocket] Accepted new WebSocket connection. " + ws.ToString());

        await ReceiveMessage(ws,
            async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // incoming websocket message
                        string incomingMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine("[WebSocket] Received message from client identified as " + connections[ws] + ": " + incomingMessage);

                        // handle incoming message via JSON serialization
                        string? outgoingMessage = ProtocolHandler.OnReceive(incomingMessage, connections[ws]);
                        if (outgoingMessage is not null) { await SendMessage(ws, outgoingMessage); }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                    {
                        // websocket connection of client was lost
                        Console.WriteLine("[WebSocket] Lost clientidentified as " + connections[ws] + ": " + ws.ToString());
                        connections.Remove(ws);
                    }
                });
    }
    else
    {
        Console.WriteLine("[WebSocket] Endpoint '/ws' has received a non-WebSocket request, responding with 400 go fuck yourself.");
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

async static Task ReceiveMessage(WebSocket ws, Action<WebSocketReceiveResult, byte[]> handleMessage)
{
    byte[] buffer = new byte[1024 * 4];
    while (ws.State == WebSocketState.Open)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }
}

async static Task SendMessage(WebSocket ws, string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    if (ws.State == WebSocketState.Open)
    {
        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
        await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}

Console.WriteLine("[WebHandler] Running server...");
await app.RunAsync();
