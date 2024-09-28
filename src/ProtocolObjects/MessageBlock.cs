using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MessageBlock (Message[] messageBlock) : Serializer
{
    public readonly Message[] messages = messageBlock;

    public static MessageBlock FromJson(string json)
    {
        var obj = JArray.Parse(json);
        List<Message> messages = new List<Message>();
        
        for(int i = 0; i < obj.Count; i++)
            messages.Add(Message.FromJson(obj[i].ToString()));
        
        return new MessageBlock(messages.ToArray());
    }
}