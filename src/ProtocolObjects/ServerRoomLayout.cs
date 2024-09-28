using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class ServerRoomLayout(RoomDescription[] roomDescription) : ObjectSerialization
{
    public readonly RoomDescription[] RoomDescriptions = roomDescription;

    public static ServerRoomLayout FromJson(string json)
    {
        var obj = JArray.Parse(json);
        List<RoomDescription> roomDescriptions = new List<RoomDescription>();
        
        for(int i = 0; i < obj.Count; i++)
            roomDescriptions.Add(RoomDescription.FromJson(obj[i].ToString()));

        return new ServerRoomLayout(roomDescriptions.ToArray());
    }
}