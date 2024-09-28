using backend.ProtocolObjects;
using Newtonsoft.Json.Linq;

namespace backend;

public class UserDescription(int id, string displayName, OnlineStatus onlineStatus) : Serializer
{
    public readonly int Id = id;
    public readonly string DisplayName = displayName;
    public readonly OnlineStatus OnlineStatus = onlineStatus;

    public static UserDescription FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new UserDescription(
            obj["id"].Value<int>(), 
            obj["displayName"].Value<string>(),
            obj["onlineStatus"].Value<int>() == 1 ? OnlineStatus.Online : OnlineStatus.Offline);
    }
}