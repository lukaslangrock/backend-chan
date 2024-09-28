using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class UserLogin(bool success, int clientId) : Serializer
{
    public readonly bool Success = success;
    public readonly int ClientId = clientId;

    public static UserLogin FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new UserLogin(
            obj["success"]!.Value<bool>(),
            obj["clientId"]!.Value<int>());
    }
}