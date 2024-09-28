using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageSendResponse (bool success) : Serializer
{
    public readonly bool Success = success;

    public MessageSendResponse FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new MessageSendResponse(
            obj["success"].Value<bool>());
    }
}