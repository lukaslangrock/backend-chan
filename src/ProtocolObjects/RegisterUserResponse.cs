using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class RegisterUserResponse(bool success)
{
    public readonly bool Success = success;

    public static RegisterUserResponse FromJson(string json)
    {
        var obj = JObject.Parse(json);
        return new RegisterUserResponse(
            obj["Success"].Value<bool>());
    }
}