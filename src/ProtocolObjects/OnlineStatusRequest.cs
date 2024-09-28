namespace backend.ProtocolObjects;

public class OnlineStatusRequest : ObjectSerialization
{
    public static OnlineStatusRequest FromJson(string json)
    {
        return new OnlineStatusRequest();
    }
}