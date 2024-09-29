using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class UserDescriptionRequest(int userId) : Serializer
{
    public readonly int UserId = userId;

    public static UserDescriptionRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new UserDescriptionRequest(obj["UserId"]!.Value<int>());
    }
}