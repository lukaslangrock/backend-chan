using Newtonsoft.Json;

namespace backend.ProtocolObjects;

public class ServerRoomLayoutRequest : ObjectSerialization
{
    public static ServerRoomLayoutRequest FromJson(string json)
    {
        return new ServerRoomLayoutRequest();
    }
}