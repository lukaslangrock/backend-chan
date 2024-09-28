using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageBlock (ProtocolMessage[] messageBlock) : Serializer
{
    public readonly ProtocolMessage[] messages = messageBlock;

    public static MessageBlock FromJson(string json)
    {
        var obj = JArray.Parse(json);
        List<ProtocolMessage> messages = new List<ProtocolMessage>();
        
        for(int i = 0; i < obj.Count; i++)
            messages.Add(ProtocolMessage.FromJson(obj[i].ToString()));
        
        return new MessageBlock(messages.ToArray());
    }
}