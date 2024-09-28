using backend.ProtocolObjects;
using Newtonsoft.Json.Linq;

namespace backend;

public class UserDescription(int id, string displayName, UserOnlineStatus onlineStatus) : ObjectSerialization
{
    public readonly int Id = id;
    public readonly string DisplayName = displayName;
    public readonly UserOnlineStatus OnlineStatus = onlineStatus;

    public static UserDescription FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new UserDescription(
            obj["id"].Value<int>(), 
            obj["displayName"].Value<string>(),
            obj["onlineStatus"].Value<int>() == 1 ? UserOnlineStatus.Online : UserOnlineStatus.Offline);
    }
}