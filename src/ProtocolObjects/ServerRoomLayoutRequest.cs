using Newtonsoft.Json;

namespace backend.ProtocolObjects;

public class ServerRoomLayoutRequest
{
    public static ServerRoomLayoutRequest FromJson(string json)
    {
        return new ServerRoomLayoutRequest();
    }
}