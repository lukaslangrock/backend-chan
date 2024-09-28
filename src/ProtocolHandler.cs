using System.Data.Entity;
using backend.database;
using backend.ProtocolObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        Console.WriteLine("[PH] Received json \"" + json + "\"" + " from client " + clientId);
        try
        {
            var jObject = JObject.Parse(json);
            
            Console.WriteLine("[PH] Deserialized json object: " + jObject.ToString());

            foreach (var entry in ProtocolTypeMapping)
            {
                if (jObject.ContainsKey(entry.Key))
                {
                    Serializer obj = entry.Value(jObject[entry.Key].Value<String>());
                    Console.WriteLine("Handling object type " + obj.GetType().Name);
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
                    Console.Write("[PH] Valid credentials");
                    clientUserMapping.Add(clientId, DB.GetUserByUsername(ulr.Username).Id);
                    return JsonConvert.SerializeObject(new UserLogin(true, clientId ));
                }
                else
                {
                    Console.WriteLine("[PH] Invalid credentials");
                    return JsonConvert.SerializeObject(new UserLogin(false, -1));
                }
            } break;
            case "UserDescriptionRequest":
            {
                UserDescriptionRequest udr = (UserDescriptionRequest)obj;
                User? user = DB.GetUserById(udr.UserId);
                
                return JsonConvert.SerializeObject(new UserDescription(user.Id, user.DisplayName, user.OnlineStatus == OnlineStatus.Online ? UserOnlineStatus.Online : UserOnlineStatus.Offline));
            } break;
            case "ServerRoomLayoutRequest":
            {
                ServerRoomLayoutRequest srlr = (ServerRoomLayoutRequest)obj;
                Room[] rooms = DB.GetRooms();
                List<RoomDescription> roomDescriptions = new List<RoomDescription>();
                
                foreach (Room room in rooms)
                {
                    User[] members = DB.GetRoomMembers(room.Id);
                    List<UserDescription> memberDescriptions = new List<UserDescription>();
                    
                    foreach(User member in members)
                    {
                        memberDescriptions.Add(new UserDescription(member.Id, member.DisplayName, member.OnlineStatus == OnlineStatus.Online ? UserOnlineStatus.Online : UserOnlineStatus.Offline));
                    }
                    
                    roomDescriptions.Add(new RoomDescription(room.DisplayName, room.Id, new MemberList(memberDescriptions.ToArray())));
                }
                
                return JsonConvert.SerializeObject(new ServerRoomLayout(roomDescriptions.ToArray()));
            } break;
            case "RoomDescriptionRequest":
            {
                RoomDescriptionRequest rdr = (RoomDescriptionRequest)obj;
                return JsonConvert.SerializeObject(DB.GetRoomById(rdr.RoomId));
            } break;
            case "MessageBlockRequest":
            {
                MessageBlockRequest mbr = (MessageBlockRequest)obj;
                Message[] dbMessageBlock = DB.GetMessages(mbr.RoomId, mbr.StartTs, mbr.EndTs);
                ProtocolMessage[] messageBlock = new ProtocolMessage[dbMessageBlock.Length];
                
                int index = 0;
                foreach (var message in dbMessageBlock)
                {
                    messageBlock[index++] = new ProtocolMessage(message.Id, message.Timestamp, message.Id, message.SenderId, message.Text);
                }

                return JsonConvert.SerializeObject(new MessageBlock(messageBlock));
            } break;
            case "MessageSendRequest":
            {
                MessageSendRequest msr = (MessageSendRequest)obj;

                if (clientUserMapping[msr.ClientId] == msr.UserId)
                {
                    int currentMilliseconds = Environment.TickCount;
                    DB.AddMessage(new Message(DB.GetFreeMessageId(), currentMilliseconds, msr.RoomId, msr.UserId, msr.Text));
                    return JsonConvert.SerializeObject(new MessageSendResponse(true));

                }

                return JsonConvert.SerializeObject(new MessageSendResponse(false)); 
            } break;
            default:
            {
                return "Unknown object type";
            }
        }
    }
}