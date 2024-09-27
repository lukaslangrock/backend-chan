using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageSendRequest(int clientId, int roomId, string text)
{
    public readonly int ClientId = clientId;
    public readonly int RoomId = roomId;
    public readonly string Text = text;

    public static MessageSendRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new MessageSendRequest(
            obj["clientId"].Value<int>(),
            obj["roomId"].Value<int>(),
            obj["text"].Value<string>());
    }
}