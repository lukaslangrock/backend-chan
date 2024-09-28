namespace backend.ProtocolObjects;

public class OnlineStatusRequest : Serializer
{
    public static OnlineStatusRequest FromJson(string json)
    {
        return new OnlineStatusRequest();
    }
}