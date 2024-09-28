using System.Net;
using System.Text;
using backend.ProtocolObjects;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Connichiwa, backuendo-chan heru. Pleasu connectu on webu sokketsu to interufacu.");

var userLogin = new UserLogin(true, 1);
var output = JsonConvert.SerializeObject(userLogin, Formatting.Indented);
Console.WriteLine(output);

var message = new Message(3, 10, "Hello, world!");
Console.WriteLine(message.SerializeObject());

MessageSendRequest msr = new MessageSendRequest(3, 10, "Mama mia!");
Console.WriteLine(msr.SerializeObject());
