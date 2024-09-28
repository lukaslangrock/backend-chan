using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageSendRequest(int roomId, string text, int clientId) : Serializer
{
    public readonly int RoomId = roomId;
    public readonly string Text = text;
    public readonly int ClientId = clientId;

    public static MessageSendRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new MessageSendRequest(
            obj["RoomId"].Value<int>(),
            obj["Text"].Value<string>(),
            obj["ClientId"].Value<int>());
    }
}
