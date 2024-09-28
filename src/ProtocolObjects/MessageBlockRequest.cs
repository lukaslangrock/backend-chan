using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageBlockRequest (int roomId, long startTs, long endTs) : ObjectSerialization
{
    public readonly int RoomId = roomId;
    public readonly long StartTs = startTs;
    public readonly long EndTs = endTs;

    public static MessageBlockRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new MessageBlockRequest(
            obj["roomId"].Value<int>(),
            obj["startTs"].Value<long>(),
            obj["endTs"].Value<long>());
    }
}