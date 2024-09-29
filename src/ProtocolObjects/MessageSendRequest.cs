using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageSendRequest(int roomId, string text, int clientId, bool isShakespeare) : Serializer
{
    public readonly int RoomId = roomId;
    public readonly string Text = text;
    public readonly int ClientId = clientId;
    public readonly bool IsShakespeare = isShakespeare;

    public static MessageSendRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);

        var isShakespeareObj = obj["IsShakespeare"];
        bool isShakespeare = (isShakespeareObj == null) ? false : isShakespeareObj.Value<bool>();
        
        return new MessageSendRequest(
            obj["RoomId"].Value<int>(),
            obj["Text"].Value<string>(),
            obj["ClientId"].Value<int>(),
            isShakespeare);
    }
}
