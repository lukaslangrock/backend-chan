using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class RoomDescription(string displayName, int id, MemberList memberList) : ObjectSerialization
{
    public readonly string DisplayName = displayName;
    public readonly int Id = id;
    public readonly MemberList MemberList = memberList;

    public static RoomDescription FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new RoomDescription(
            obj["displayName"].Value<string>(),
            obj["id"].Value<int>(),
            ProtocolObjects.MemberList.FromJson(obj["memberList"].ToString()));
    }
}