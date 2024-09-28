using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class UserLoginRequest(string username, string password) : Serializer
{
    public readonly string Username = username;
    public readonly string Password = password;

    public static UserLoginRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new UserLoginRequest(
            obj["Username"]!.Value<string>()!,
            obj["Password"]!.Value<string>()!);
    }
}