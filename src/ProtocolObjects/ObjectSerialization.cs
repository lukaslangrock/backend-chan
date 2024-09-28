﻿using Newtonsoft.Json;

namespace backend.ProtocolObjects;

public class ObjectSerialization
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
    