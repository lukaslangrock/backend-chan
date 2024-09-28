using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class RegisterUserRequest(string username, string password, string displayName) : Serializer
{
    public readonly string Username = username;
    public readonly string Password = password;
    public readonly string DisplayName = displayName;

    public static RegisterUserRequest FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new RegisterUserRequest(
            obj["Username"].Value<string>(),
            obj["Password"].Value<string>(),
            obj["DisplayName"].Value<string>());
    }
}