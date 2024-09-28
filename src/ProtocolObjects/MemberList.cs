using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.ProtocolObjects;

public class MemberList(UserDescription[] userDescriptions) : ObjectSerialization
{
    public readonly UserDescription[] UserDescriptions = userDescriptions;

    public static MemberList FromJson(string json)
    {
        var obj = JArray.Parse(json);
        List<UserDescription> userDescriptions = new List<UserDescription>();
        
        for(int i = 0; i < obj.Count; i++)
            userDescriptions.Add(UserDescription.FromJson(obj[i].ToString()));
        
        return new MemberList(userDescriptions.ToArray());
    }
}