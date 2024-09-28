using Newtonsoft.Json;

namespace backend.ProtocolObjects;

public class Serializer
{
    public string SerializeObject()
    {
        var wrappedObject = new Dictionary<string, object>
        {
            { GetType().Name, this }
        };
        return JsonConvert.SerializeObject(wrappedObject);
    }
}
    