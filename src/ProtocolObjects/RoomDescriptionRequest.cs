using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class RoomDescriptionRequest (int roomId) : Serializer
{
    public readonly int RoomId = roomId;

    public static RoomDescriptionRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new RoomDescriptionRequest(obj["id"].Value<int>());
    }
}