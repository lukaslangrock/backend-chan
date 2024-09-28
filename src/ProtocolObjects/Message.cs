using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class Message(int userId, int roomId, string text) : ObjectSerialization
{
    public readonly int UserId = userId;
    public readonly int RoomId = roomId;
    public readonly string Text = text;

    public static Message FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new Message(
            obj["userId"].Value<int>(), 
            obj["roomId"].Value<int>(),
            obj["text"].Value<String>());
    }
}