using System.Data.Entity;
using System.Numerics;
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
        { "RegisterUserRequest", json => RegisterUserRequest.FromJson(json)},
    };

    private static readonly Dictionary<int, int> clientUserMapping = new Dictionary<int, int>();
    
    public static List<(string?, bool)> OnReceive(string json, int clientId)
    {
        try
        {
            var jObject = JObject.Parse(json);
            
            Console.WriteLine("[ProtocolHandler] Deserialized json object...");
            foreach (var entry in ProtocolTypeMapping)
            {
                if (jObject.ContainsKey(entry.Key))
                {
                    Console.WriteLine(entry.Key + ", " + jObject[entry.Key]);
                    Serializer obj = entry.Value(jObject[entry.Key].ToString());
                    Console.WriteLine("Handling object type " + obj.GetType().Name);
                    return Handle(obj, clientId);
                }
            }
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Json parsing error: {e.Message}");
        }
        List<(string?, bool)> ret = new List<(string?, bool)>();
        ret.Add((null, false));
        return ret;
    }

    public static List<(string?, bool)> Handle(Serializer obj, int clientId)
    {
        switch (obj.GetType().Name)
        {
            case "RegisterUserRequest":
            {
                RegisterUserRequest rur = (RegisterUserRequest)obj;

                if (DB.GetUserByUsername(rur.Username) == null)
                {
                    DB.AddUser(new User(DB.GetFreeUserId(), rur.Username, rur.Password, rur.DisplayName, OnlineStatus.Offline));
                    
                    List<(string?, bool)> o = new List<(string?, bool)>();
                    o.Add((JsonConvert.SerializeObject(new RegisterUserResponse(true)), false));
                    return o;
                }

                List<(string?, bool)> o2 = new List<(string?, bool)>();
                o2.Add((JsonConvert.SerializeObject(new RegisterUserResponse(false)), false));
                return o2;
            }
            case "UserLoginRequest":
            {
                UserLoginRequest ulr = (UserLoginRequest)obj;
                bool valid = DB.CheckUserCredentials(ulr.Username, ulr.Password);
                
                if (valid)
                {
                    Console.Write("[ProtocolHandler] Valid credentials");
                    User? user = DB.GetUserByUsername(ulr.Username);
                    if(user == null)
                        Console.WriteLine("User logged in with valid credentials, but is not found in database.");
                    clientUserMapping.Add(clientId, user.Id);
                    
                    List<(string?, bool)> o3 = new List<(string?, bool)>();
                    o3.Add((JsonConvert.SerializeObject(new UserLogin(true, clientId )), false));
                    return o3;
                }
                else
                {
                    Console.WriteLine("[ProtocolHandler] Invalid credentials");
                    
                    List<(string?, bool)> o3 = new List<(string?, bool)>();
                    o3.Add((JsonConvert.SerializeObject(new UserLogin(false, -1)), false));
                    return o3;
                }
            } break;
            case "UserDescriptionRequest":
            {
                UserDescriptionRequest udr = (UserDescriptionRequest)obj;
                User? user = DB.GetUserById(udr.UserId);
                
                List<(string?, bool)> o4 = new List<(string?, bool)>();
                o4.Add((JsonConvert.SerializeObject(new UserDescription(user.Id, user.DisplayName, user.OnlineStatus == OnlineStatus.Online ? UserOnlineStatus.Online : UserOnlineStatus.Offline)), false));
                return o4;
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
                
                List<(string?, bool)> o5 = new List<(string?, bool)>();
                o5.Add((JsonConvert.SerializeObject(new ServerRoomLayout(roomDescriptions.ToArray())), false));
                return o5;
            } break;
            case "RoomDescriptionRequest":
            {
                RoomDescriptionRequest rdr = (RoomDescriptionRequest)obj;
                
                List<(string?, bool)> o6 = new List<(string?, bool)>();
                o6.Add((JsonConvert.SerializeObject(DB.GetRoomById(rdr.RoomId)), false));
                return o6;
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

                List<(string?, bool)> o7 = new List<(string?, bool)>();
                o7.Add((JsonConvert.SerializeObject(new MessageBlock(messageBlock)), false));
                return o7;
            } break;
            case "MessageSendRequest":
            {
                MessageSendRequest msr = (MessageSendRequest)obj;

                int currentMilliseconds = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                DB.AddMessage(new Message(DB.GetFreeMessageId(), currentMilliseconds, msr.RoomId, clientUserMapping[msr.ClientId], msr.Text));
                
                List<(string?, bool)> o8 = new List<(string?, bool)>();
                o8.Add((JsonConvert.SerializeObject(new MessageSendResponse(true)), false));

                Message[] messages = DB.GetLastMessages(msr.RoomId, 20);
                List<ProtocolMessage> protMessages = new List<ProtocolMessage>();

                foreach (var message in messages)
                {
                    protMessages.Add(new ProtocolMessage(message.Id, message.Timestamp, message.RoomId, message.SenderId, message.Text));
                }
                
                o8.Add((JsonConvert.SerializeObject(new MessageBlock(protMessages.ToArray())), true));
                return o8;
            } break;
            default:
            {
                List<(string?, bool)> o10 = new List<(string?, bool)>();
                o10.Add(("Unknown object type", false));
                return o10;
            }
        }
    }
}
