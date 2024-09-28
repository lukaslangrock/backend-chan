using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Connichiwa, backuendo-chan heru. Pleasu connectu on webu sokketsu to interufacu.");

app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var bytes = Encoding.UTF8.GetBytes("Hello from Server");
        await ws.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

await app.RunAsync();
