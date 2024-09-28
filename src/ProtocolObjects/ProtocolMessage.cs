using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class ProtocolMessage(int id, int timestamp, int roomId, int senderId, string text)
{
    public readonly int Id = id;
    public readonly int Timestamp = timestamp;
    public readonly int RoomId = roomId;
    public readonly int SenderId = senderId;
    public readonly string Text = text;

    public static ProtocolMessage FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new ProtocolMessage(
            obj["id"].Value<int>(), 
            obj["timestamp"].Value<int>(),
            obj["roomId"].Value<int>(),
            obj["senderId"].Value<int>(),
            obj["text"].Value<String>());
    }
}