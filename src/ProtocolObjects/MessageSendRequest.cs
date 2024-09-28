using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageSendRequest(int clientId, int userId, int roomId, string text) : Serializer
{
    public readonly int ClientId = clientId;
    public readonly int UserId = userId;
    public readonly int RoomId = roomId;
    public readonly string Text = text;

    public static MessageSendRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new MessageSendRequest(
            obj["ClientId"].Value<int>(),
            obj["UserId"].Value<int>(),
            obj["RoomId"].Value<int>(),
            obj["Text"].Value<string>());
    }
}
