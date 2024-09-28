using System.Data.Entity;
using backend.database;
using backend.ProtocolObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Message = backend.database.Message;

namespace backend;

public static class ProtocolHandler
{
    private static readonly Dictionary<string, Func<string, Serializer>> ProtocolTypeMapping = new()
    {
        { "UserLoginRequest", json => UserLoginRequest.FromJson(json) },
        { "UserDescriptionRequest", json =>  UserDescriptionRequest.FromJson(json)},
        { "ServerRoomLayoutRequest", json => ServerRoomLayoutRequest.FromJson(json)},
        { "RoomDescriptionRequest", json => RoomDescriptionRequest.FromJson(json) },
        { "MessageBlockRequest", json => MessageBlockRequest.FromJson(json)},
        { "MessageSendRequest", json => MessageSendRequest.FromJson(json) },
    };

    private static readonly Dictionary<int, int> clientUserMapping = new Dictionary<int, int>();
    
    public static string? OnReceive(string json, int clientId)
    {
        try
        {
            var jObject = JObject.Parse(json);

            if (jObject.ContainsKey() // Check if the json object has a "Type" field
            {
                var typeName = jObject["Type"]?.ToString();

                if (typeName != null && ProtocolTypeMapping.TryGetValue(typeName, out var objectCreator))
                {
                    Serializer obj = objectCreator(json);
                    return Handle(obj, clientId);
                }
            }
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Json parsing error: {e.Message}");
        }
        return null;
    }

    public static string? Handle(Serializer obj, int clientId)
    {
        switch (obj.GetType().Name)
        {
            case "UserLoginRequest":
            {
                UserLoginRequest ulr = (UserLoginRequest)obj;
                bool valid = DB.CheckUserCredentials(ulr.Username, ulr.Password);
                
                if (valid)
                {
                    clientUserMapping.Add(clientId, DB.GetUserByUsername(ulr.Username).Id);
                    return JsonConvert.SerializeObject(new UserLogin(true, clientId ));
                }
                else
                {
                    return JsonConvert.SerializeObject(new UserLogin(false, -1));
                }
            } break;
            case "UserDescriptionRequest":
            {
                UserDescriptionRequest udr = (UserDescriptionRequest)obj;
                return JsonConvert.SerializeObject(new UserDescription(DB.GetUserById(udr.UserId), )); //Missing Implementation from DB
            } break;
            case "ServerRoomLayoutRequest":
            {
                ServerRoomLayoutRequest srlr = (ServerRoomLayoutRequest)obj;
                
                return JsonConvert.SerializeObject(srlr); // //Missing Implementation from DB
            } break;
            case "RoomDescriptionRequest":
            {
                RoomDescriptionRequest rdr = (RoomDescriptionRequest)obj;
                return JsonConvert.SerializeObject(DB.GetRoomById(rdr.RoomId));
            } break;
            case "MessageBlockRequest":
            {
                MessageBlockRequest mbr = (MessageBlockRequest)obj;
                return JsonConvert.SerializeObject(DB.GetMessages(mbr.RoomId, mbr.StartTs, mbr.EndTs));
            } break;
            case "MessageSendRequest":
            {
                MessageSendRequest msr = (MessageSendRequest)obj;

                if (clientUserMapping[msr.ClientId] == msr.UserId)
                {
                    int currentMilliseconds = Environment.TickCount;
                    DB.AddMessage(new Message(DB.GetFreeMessageId(), currentMilliseconds, msr.RoomId, msr.UserId, msr.Text));
                    return JsonConvert.SerializeObject(new MessageSendResponse(true)); // Missing Implementation from DB

                }

                return JsonConvert.SerializeObject(new MessageSendResponse(false)); // Missing Implementation from DB
            } break;
            default:
            {
                return "Unknown object type";
            }
        }
    }
}